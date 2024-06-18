using System.Security.Cryptography;
using System.Text;
using CleanArch.Infrastructure.Data.Settings;
using Microsoft.Extensions.Options;

namespace CleanArch.Infrastructure.Data.Security;

public sealed class DataSecurityService(IOptionsSnapshot<DataSettings> settings)
{
    public string Encrypt(string plainText)
    {
        var key = Convert.FromBase64String(settings.Value.EncryptionKey);
        var iv = new byte[12];
        var tag = new byte[16];

        RandomNumberGenerator.Fill(iv);

        var plainBytes = Encoding.UTF8.GetBytes(plainText);
        var cipher = new byte[plainBytes.Length];

        using var aesGcm = new AesGcm(key, 16);
        aesGcm.Encrypt(iv, plainBytes, cipher, tag);

        var buffer = new byte[iv.Length + tag.Length + cipher.Length];
        Buffer.BlockCopy(iv, 0, buffer, 0, iv.Length);
        Buffer.BlockCopy(tag, 0, buffer, iv.Length, tag.Length);
        Buffer.BlockCopy(cipher, 0, buffer, iv.Length + tag.Length, cipher.Length);

        return Convert.ToBase64String(buffer);
    }

    public string Decrypt(string encryptedData)
    {
        var encryptedBytes = Convert.FromBase64String(encryptedData);
        var iv = encryptedBytes[..12];
        var tag = encryptedBytes[12..28];
        var cipher = encryptedBytes[28..];

        var decryptedBytes = new byte[cipher.Length];

        using var aesGcm = new AesGcm(Convert.FromBase64String(settings.Value.EncryptionKey), 16);
        aesGcm.Decrypt(iv, cipher, tag, decryptedBytes);

        return Encoding.UTF8.GetString(decryptedBytes);
    }
}
