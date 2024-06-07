using System.Net.Http.Json;
using BerkutTuyaPulsarWorker.Interfaces;
using BerkutTuyaPulsarWorker.Models;
using BerkutTuyaPulsarWorker.Options;
using Microsoft.Extensions.Options;

namespace BerkutTuyaPulsarWorker.Repositories;

public class BroadcasterRepository : IBroadcasterRepository
{
    private readonly HttpClient _httpClient;
    private readonly BroadcasterOptions _options;

    public BroadcasterRepository(HttpClient httpClient, IOptionsMonitor<BroadcasterOptions> options)
    {
        _httpClient = httpClient;
        _options = options.CurrentValue;

        _httpClient.BaseAddress = _options.BaseUrl;

        if(!string.IsNullOrEmpty(_options.AuthHeaderName))
            _httpClient.DefaultRequestHeaders.Add(_options.AuthHeaderName, _options.ApiKey);
    }

    public Task PublishDeviceDataAsync(DeviceData deviceData) =>
        _httpClient.PostAsJsonAsync(_options.PostDeviceStatusEndpoint, deviceData);
}
