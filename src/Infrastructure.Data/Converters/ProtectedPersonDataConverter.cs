using CleanArch.Infrastructure.Data.Security;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CleanArch.Infrastructure.Data.Converters;

public class ProtectedPersonDataConverter(DataSecurityService dataSecurityService)
    : ValueConverter<string, string>(
        v => dataSecurityService.Encrypt(v),
        v => dataSecurityService.Decrypt(v));
