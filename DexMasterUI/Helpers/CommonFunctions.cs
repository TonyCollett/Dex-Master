using System.Globalization;
using System.Text.RegularExpressions;
using DexMasterLibrary.Enums;

namespace DexMasterUI.Helpers
{
    public static class CommonFunctions
    {
        /// <summary>
        /// Capitalise the first letter of a string
        /// </summary>
        public static string Capitalise(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input);
        }
        
        /// <summary>
        /// Convert a string to an enum
        /// </summary>
        public static T? ConvertStringToEnum<T>(string input) where T : struct
        {
            if (string.IsNullOrEmpty(input))
            {
                return null;
            }

            return Enum.TryParse(input, true, out T output) ? output : null;
        }
        
        /// <summary>
        /// Advanced version of Capitalise that also cleans up the string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string CleanAndCapitalise(string input)
        {
            string[] words = input.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < words.Length; i++)
            {
                if (i == 0)
                {
                    // Capitalize the first word regardless
                    words[i] = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(words[i]);
                }
                else
                {
                    // Convert to lowercase and check if it's "the" or "and" or "of"
                    string word = words[i].ToLower();
                    if (word != "the" && word != "and" && word != "of")
                    {
                        // Check if the word is a Roman numeral and capitalize it
                        if (IsRomanNumeral(word))
                        {
                            words[i] = word.ToUpper();
                        }
                        else
                        {
                            words[i] = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(words[i]);
                        }
                    }
                }
            }

            string cleanedString = string.Join(" ", words);

            return cleanedString;
        }

        /// <summary>
        /// Check if a string is a Roman numeral
        /// </summary>
        private static bool IsRomanNumeral(string word)
        {
            // Use a regular expression to check for valid Roman numerals
            return Regex.IsMatch(word, @"^(?i)(?=[MDCLXVI])M*(C[MD]|D?C*)(X[CL]|L?X*)(I[XV]|V?I*)$");
        }
        
        /// <summary>
        /// Convert a decimeter length to a string
        /// </summary>
        public static string ConvertLengthToString(int decimeters)
        {
            double centimeters = decimeters * 10;
            double meters = decimeters / 10.0;
            double inches = centimeters / 2.54;
            int feet = (int)(inches / 12);
            inches %= 12;

            return meters >= 1 ? $"{meters}m ({feet}' {inches:F1}\")" : $"{centimeters}cm ({feet}' {inches:F1}\")";
        }
        
        /// <summary>
        /// Convert a hectogram weight to a string
        /// </summary>
        public static string ConvertWeightToString(int hectograms)
        {
            double kilograms = hectograms / 10.0;
            double pounds = kilograms * 2.20462;
            int wholePounds = (int)pounds;
            double ounces = (pounds - wholePounds) * 16;

            return $"{kilograms}kg ({wholePounds}lb {ounces:F1}oz)";
        }
    }
}
