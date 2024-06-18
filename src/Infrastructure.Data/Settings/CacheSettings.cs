using System.ComponentModel.DataAnnotations;

namespace CleanArch.Infrastructure.Data.Settings;

public class CacheSettings
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "ConnectionString is required")]
    public string ConnectionString { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false, ErrorMessage = "SchemaName is required")]
    public string SchemaName { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false, ErrorMessage = "TableName is required")]
    public string TableName { get; set; } = string.Empty;
}
