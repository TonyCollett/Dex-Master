using System.Reflection;
using PokemonType = DexMasterUI.Assets.Enums.PokemonType;

namespace DexMasterUI.Assets;

public class AssetControl
{
    public static string GetImageForPokemonType(PokemonType type)
        => GetEmbeddedImageAsBase64("PokemonTypes.Pokemon_Type_Icon_" + type.ToString() + ".png");

    private static string GetEmbeddedImageAsBase64(string resourceName)
    {
        var assembly = Assembly.GetExecutingAssembly();

        using (Stream stream = assembly.GetManifestResourceStream("DexMasterUI.Assets.Images." + resourceName))
        using (MemoryStream memoryStream = new())
        {
            if (stream != null) stream.CopyTo(memoryStream);
            byte[] imageBytes = memoryStream.ToArray();
            return $"data:image/png;base64,{Convert.ToBase64String(imageBytes)}";
        }
    }
}