using System.Reflection;
using PokemonType = DexMasterUI.Assets.Enums.PokemonType;

namespace DexMasterUI.Assets;

/// <summary>
/// Asset Control uses assets embedded into the assembly and returns them as base64 strings
/// To add new assets, add them to the DexMasterUI/Assets/Images folder and set the Build Action to Embedded Resource
/// </summary>
public class AssetControl
{
    /// <summary>
    /// This method returns the embedded image as a base64 string
    /// </summary>
    public static string GetImageForPokemonType(PokemonType type)
        => GetEmbeddedImageAsBase64("PokemonTypes.Pokemon_Type_Icon_" + type.ToString() + ".png");

    /// <summary>
    /// This private method returns the embedded image as a base64 string
    /// </summary>
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