using UnityEngine;
using TMPro;

public class KoreanFontSetup : MonoBehaviour
{
    void Awake()
    {
        // Try loading pre-made TMP font asset first
        TMP_FontAsset tmpFont = Resources.Load<TMP_FontAsset>("Fonts/NotoSansKR-Bold SDF");
        if (tmpFont != null)
        {
            ApplyFont(tmpFont);
            Debug.Log("[KoreanFontSetup] Loaded existing TMP font asset.");
            return;
        }

        // Fallback: create from TTF at runtime
        Font font = Resources.Load<Font>("Fonts/NotoSansKR-Bold");
        if (font == null)
        {
            Debug.LogError("[KoreanFontSetup] Font file not found in Resources/Fonts/");
            return;
        }

        Debug.Log($"[KoreanFontSetup] Font loaded: {font.name}");

        TMP_FontAsset runtimeFont = TMP_FontAsset.CreateFontAsset(font, 36, 4,
            UnityEngine.TextCore.LowLevel.GlyphRenderMode.SDFAA,
            2048, 2048,
            AtlasPopulationMode.Dynamic);

        if (runtimeFont == null)
        {
            Debug.LogError("[KoreanFontSetup] Failed to create TMP font asset from TTF.");
            return;
        }

        runtimeFont.name = "NotoSansKR-Runtime";
        ApplyFont(runtimeFont);
        Debug.Log("[KoreanFontSetup] Runtime Korean font applied.");
    }

    void ApplyFont(TMP_FontAsset font)
    {
        TMP_Text[] allTexts = FindObjectsByType<TMP_Text>(FindObjectsSortMode.None);
        foreach (TMP_Text text in allTexts)
        {
            text.font = font;
        }
    }
}
