using System.ComponentModel.DataAnnotations;

namespace CleanArch.Infrastructure.Data.Settings;

public class DataSettings
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "ConnectionString is required")]
    public string ConnectionString { get; set; } = string.Empty;

    /// <summary>
    /// To generate a key, run the following command:
    ///
    /// var bytes = new byte[16];
    /// RandomNumberGenerator.Fill(bytes);
    /// var key = Convert.ToBase64String(bytes);
    /// </summary>
    [Required(AllowEmptyStrings = false, ErrorMessage = "EncryptionKey is required")]
    public string EncryptionKey { get; set; } = string.Empty;
}
