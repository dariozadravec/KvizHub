using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace KvizHub.Api.Data.Converters
{
    // Stores enum values as snake_case strings, e.g. RegistrationsClosed -> registrations_closed
    public class SnakeCaseEnumConverter<TEnum>() : ValueConverter<TEnum, string>(
        value => ToSnakeCase(value.ToString()),
        value => Enum.Parse<TEnum>(value.Replace("_", ""), true))
        where TEnum : struct, Enum
    {
        private static string ToSnakeCase(string name) =>
            string.Concat(name.Select((c, i) =>
                i > 0 && char.IsUpper(c) ? "_" + char.ToLowerInvariant(c) : char.ToLowerInvariant(c).ToString()));
    }
}
