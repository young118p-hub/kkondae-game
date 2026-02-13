using UnityEngine;
using UnityEditor;
using TMPro;

public class CreateKoreanFontAsset
{
    [MenuItem("Tools/Create Korean TMP Font Asset")]
    public static void Create()
    {
        string fontPath = "Assets/Resources/Fonts/NotoSansKR-Bold.ttf";
        Font font = AssetDatabase.LoadAssetAtPath<Font>(fontPath);

        if (font == null)
        {
            Debug.LogError($"Font not found at {fontPath}");
            return;
        }

        // Create TMP Font Asset with dynamic mode (renders characters on demand)
        TMP_FontAsset fontAsset = TMP_FontAsset.CreateFontAsset(font, 36, 4,
            UnityEngine.TextCore.LowLevel.GlyphRenderMode.SDFAA,
            4096, 4096,
            AtlasPopulationMode.Dynamic);

        if (fontAsset == null)
        {
            Debug.LogError("Failed to create TMP Font Asset");
            return;
        }

        fontAsset.name = "NotoSansKR-Bold SDF";

        string savePath = "Assets/Resources/Fonts/NotoSansKR-Bold SDF.asset";
        AssetDatabase.CreateAsset(fontAsset, savePath);
        AssetDatabase.SaveAssets();

        Debug.Log($"Korean TMP Font Asset created at {savePath}");
    }
}
