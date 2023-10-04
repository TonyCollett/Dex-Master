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

            return string.Concat(input.First().ToString().ToUpper(), input.AsSpan(1));
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
    }
}
