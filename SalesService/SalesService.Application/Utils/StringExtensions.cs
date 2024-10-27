using System.Text.RegularExpressions;

namespace SalesService.Application.Utils
{
    internal static partial class StringExtensions
    {
        public static string RemoveSpecialCharacters(this string input)
        {
            return input.IsEmpty() ? input : FilterSpecialCharacters().Replace(input, string.Empty);
        }

        public static bool IsEmpty(this string input) => string.IsNullOrWhiteSpace(input);

        [GeneratedRegex("[^a-zA-Z0-9]")]
        private static partial Regex FilterSpecialCharacters();
    }
}
