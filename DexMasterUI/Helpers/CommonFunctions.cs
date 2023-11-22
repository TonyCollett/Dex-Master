using System.Globalization;
using System.Text.RegularExpressions;
using DexMasterLibrary.Enums;

namespace DexMasterUI.Helpers
{
    public static class CommonFunctions
    {
        public static string Capitalise(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input);
        }

        public static string GetTypeImage(Types type)
        {
            Dictionary<Types, string> typeToImageMap = new Dictionary<Types, string>
            {
                { Types.Bug, "bug.png" },
                { Types.Dark, "dark.png" },
                { Types.Dragon, "dragon.png" },
                { Types.Electric, "electric.png" },
                { Types.Fairy, "fairy.png" },
                { Types.Fighting, "fighting.png" },
                { Types.Fire, "fire.png" },
                { Types.Flying, "flying.png" },
                { Types.Ghost, "ghost.png" },
                { Types.Grass, "grass.png" },
                { Types.Ground, "ground.png" },
                { Types.Ice, "ice.png" },
                { Types.Normal, "normal.png" },
                { Types.Poison, "poison.png" },
                { Types.Psychic, "psychic.png" },
                { Types.Rock, "rock.png" },
                { Types.Steel, "steel.png" },
                { Types.Water, "water.png" }
            };
            
            return typeToImageMap[type];
        }
        
        // Convert string to Type
        public static T? ConvertStringToEnum<T>(string input) where T : struct
        {
            if (string.IsNullOrEmpty(input))
            {
                return null;
            }

            return Enum.TryParse(input, true, out T output) ? output : null;
        }
        
        // Clean up Version and Generation string
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

        private static bool IsRomanNumeral(string word)
        {
            // Use a regular expression to check for valid Roman numerals
            return Regex.IsMatch(word, @"^(?i)(?=[MDCLXVI])M*(C[MD]|D?C*)(X[CL]|L?X*)(I[XV]|V?I*)$");
        }
    }
}
