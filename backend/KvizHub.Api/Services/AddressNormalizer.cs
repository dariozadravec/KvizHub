using System.Globalization;
using System.Text;

namespace KvizHub.Api.Services
{
    public static class AddressNormalizer
    {
        // Street words that are often abbreviated or left out, so they do not affect the comparison
        private static readonly HashSet<string> IgnoredStreetWords = ["ulica", "ul"];

        // Separates the address from the name; normalization never leaves it inside the address
        private const char NameSeparator = '|';

        public static string NormalizeAddress(string street, string postalCode, string city) =>
            NormalizeText($"{street} {postalCode} {city}", IgnoredStreetWords);

        // Unique venue key: normalized address followed by the normalized name
        public static string VenueKey(string normalizedAddress, string name) =>
            $"{normalizedAddress}{NameSeparator}{NormalizeText(name, [])}";

        // Prefix shared by all venue keys at the given address
        public static string AddressPrefix(string normalizedAddress) => $"{normalizedAddress}{NameSeparator}";

        // Lower case, without diacritics and punctuation, single spaces
        private static string NormalizeText(string text, HashSet<string> ignoredWords)
        {
            var decomposed = text.ToLowerInvariant().Normalize(NormalizationForm.FormD);
            var builder = new StringBuilder();

            foreach (var c in decomposed)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) == UnicodeCategory.NonSpacingMark)
                    continue;
                builder.Append(char.IsLetterOrDigit(c) ? c : ' ');
            }

            var words = builder.ToString()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Where(word => !ignoredWords.Contains(word));

            return string.Join(' ', words);
        }
    }
}