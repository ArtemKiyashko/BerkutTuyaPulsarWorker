using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using BerkutTuyaPulsarWorker.Helpers;
using BerkutTuyaPulsarWorker.Interfaces;
using BerkutTuyaPulsarWorker.Models;
using BerkutTuyaPulsarWorker.Options;
using Microsoft.Extensions.Options;

namespace BerkutTuyaPulsarWorker.Managers;

public class TuyaWebSocketManager : ITuyaWebSocket
{
    private readonly ClientWebSocket _webSocket = new();
    private readonly WebSocketOptions _options;
    private const int _receiveChunkSize = 1024 * 5;
    private string _topicUrl => $"{_options.ServerUrl}ws/v2/consumer/persistent/{_options.AccessId}/out/{_options.MqEnv}/{_options.SubscriptionName}{_options.QueryParams}";

    public TuyaWebSocketManager(IOptionsMonitor<WebSocketOptions> options)
    {
        _options = options.CurrentValue;
        options.OnChange(OnSocketOptionsChange);
        InitSocket();
    }

    private void InitSocket()
    {
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
        _webSocket.Options.SetRequestHeader("username", _options.AccessId);
        _webSocket.Options.SetRequestHeader("password", TuyaCryptoHelper.GeneratePassword(_options.AccessId, _options.AccessKey));
        _webSocket.Options.KeepAliveInterval = TimeSpan.FromSeconds(_options.PingIntervalSeconds);
    }

    private void OnSocketOptionsChange(WebSocketOptions newOptions)
    {

    }

    public async Task<Message<T>?> GetMessageAsync<T>()
    {
        using var ms = new MemoryStream();
        while (_webSocket.State == WebSocketState.Open)
        {
            await ReadAllDataToStream(ms);
            using var streamReader = new StreamReader(ms, Encoding.UTF8);
            var text = await streamReader.ReadToEndAsync();
            var message = JsonSerializer.Deserialize<Message<T>>(text);

            await AcknowledgeMessageAsync(message.MessageId);
            
            message.MessageContent = GetMessageContent(message);
            message.Result = await GetContentData(message);
            return message;
        }
        return default;
    }

    private async Task<T?> GetContentData<T>(Message<T>? message)
    {
        var decryptedData = await TuyaCryptoHelper.DecryptPayloadAsync(message.MessageContent.Data, _options.AccessKey);
        var result = JsonSerializer.Deserialize<T>(decryptedData);
        return result;
    }

    private static MessageContent GetMessageContent<T>(Message<T> message)
    {
        var payloadJson = Encoding.UTF8.GetString(Convert.FromBase64String(message.Payload));
        var messageContent = JsonSerializer.Deserialize<MessageContent>(payloadJson);
        return messageContent;
    }

    private async Task ReadAllDataToStream(Stream stream)
    {
        WebSocketReceiveResult result;
        var buffer = new ArraySegment<byte>(new byte[_receiveChunkSize]);
        do
        {
            result = await _webSocket.ReceiveAsync(buffer, CancellationToken.None);
            if (result.MessageType == WebSocketMessageType.Close)
            {
                await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
            }

            await stream.WriteAsync(buffer.Array.AsMemory(buffer.Offset, result.Count));
        }
        while (!result.EndOfMessage);
        stream.Seek(0, SeekOrigin.Begin);
    }

    public Task ConnectAsync() => ConnectAsync(CancellationToken.None);

    public Task ConnectAsync(CancellationToken cancellationToken) => _webSocket.ConnectAsync(new Uri(_topicUrl), cancellationToken);

    public Task AcknowledgeMessageAsync(string messageId)
    {
        var message = new AcknowledgeMessage{
            MessageId = messageId
        };
        var messageString = JsonSerializer.Serialize(message);
        return _webSocket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(messageString)), WebSocketMessageType.Text, true, CancellationToken.None);
    }
}
