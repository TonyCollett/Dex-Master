using System;

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
    }
}
