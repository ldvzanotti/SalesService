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

        public static string MaskTaxpayerRegistration(this string input)
        {
            return $"{input[..3]}.{input[3..6]}.{input[6..9]}-{input[9..11]}";
        }

        public static string MaskPhoneNumber(this string input)
        {
            return $"+{input[..2]} ({input[2..4]}) {input[4..7]}-{input[7..10]}-{input[10..13]}";
        }

        [GeneratedRegex("[^a-zA-Z0-9]")]
        private static partial Regex FilterSpecialCharacters();
    }
}
