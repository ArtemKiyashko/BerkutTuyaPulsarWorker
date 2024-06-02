using System.Security.Cryptography;
using System.Text;

namespace BerkutTuyaPulsarWorker.Helpers;

public static class TuyaCryptoHelper
{
    public static string GeneratePassword(string accessId, string accessKey)
    {
        var md5HexKey = GetMd5Hash(accessKey);
        var mixStr = $"{accessId}{md5HexKey}";
        var md5MixStr = GetMd5Hash(mixStr);
        return md5MixStr.Substring(8, 16);
    }

    public static async Task<string> DecryptPayloadAsync(string payload, string accessKey)
    {
        var decryptKey = accessKey.Substring(8, 16);
        var payloadBytes = Convert.FromBase64String(payload);
        var keyBytes = Encoding.UTF8.GetBytes(decryptKey);

        using var aes = Aes.Create();
        aes.Mode = CipherMode.ECB;
        aes.BlockSize = 128;
        aes.Padding = PaddingMode.Zeros;
        using var decryptor = aes.CreateDecryptor(keyBytes, aes.IV);
        using var memStream = new MemoryStream(payloadBytes);
        using var cryptoStream = new CryptoStream(memStream, decryptor, CryptoStreamMode.Read);
        using var streamReader = new StreamReader(cryptoStream);

        var decrypted = await streamReader.ReadToEndAsync();
        var result = new string(decrypted.Where(c => !char.IsControl(c)).ToArray());
        return result;
    }

    private static string GetMd5Hash(string input)
    {
        byte[] dataHash = MD5.HashData(Encoding.UTF8.GetBytes(input));
        var stringBuilder = new StringBuilder();
        foreach (var b in dataHash)
            stringBuilder.Append(b.ToString("x2").ToLower());
        return stringBuilder.ToString();
    }
}
