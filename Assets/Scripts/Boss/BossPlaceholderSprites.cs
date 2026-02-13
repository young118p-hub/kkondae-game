using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Creates human-shaped placeholder sprites for the boss character.
/// Each body part gets an appropriately shaped and colored sprite.
/// </summary>
public class BossPlaceholderSprites : MonoBehaviour
{
    void Awake()
    {
        Dictionary<string, (int w, int h, Color color)> partSpecs = new()
        {
            { "Head",    (48, 48, new Color(0.96f, 0.80f, 0.65f)) },  // Round head - skin
            { "Cheek",   (20, 16, new Color(1f, 0.70f, 0.65f)) },     // Small cheek - pinkish
            { "Hand_L",  (24, 24, new Color(0.96f, 0.82f, 0.68f)) },  // Left hand - skin
            { "Hand_R",  (24, 24, new Color(0.96f, 0.82f, 0.68f)) },  // Right hand - skin
            { "Torso",   (56, 72, new Color(0.25f, 0.30f, 0.42f)) },  // Suit body - dark blue
            { "Calf",    (40, 56, new Color(0.22f, 0.22f, 0.28f)) },  // Pants legs - dark
            { "Butt",    (50, 32, new Color(0.25f, 0.28f, 0.38f)) },  // Lower body - dark
        };

        SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sr in renderers)
        {
            string partName = sr.gameObject.name;
            if (partSpecs.TryGetValue(partName, out var spec))
            {
                sr.sprite = CreateRoundedSprite(spec.w, spec.h, spec.color, partName == "Head");
                sr.color = Color.white; // color baked into sprite
            }
            else if (sr.sprite == null)
            {
                sr.sprite = CreateRoundedSprite(32, 32, sr.color, false);
                sr.color = Color.white;
            }
        }
    }

    Sprite CreateRoundedSprite(int w, int h, Color baseColor, bool isCircle)
    {
        Texture2D tex = new Texture2D(w, h);
        tex.filterMode = FilterMode.Point;

        float cx = w / 2f;
        float cy = h / 2f;
        float rx = w / 2f;
        float ry = h / 2f;

        Color darkEdge = baseColor * 0.7f;
        darkEdge.a = 1f;

        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                float dx = (x - cx) / rx;
                float dy = (y - cy) / ry;
                float dist = dx * dx + dy * dy;

                if (isCircle && dist > 1f)
                {
                    tex.SetPixel(x, y, Color.clear);
                }
                else if (!isCircle && (x < 2 || x >= w - 2 || y < 2 || y >= h - 2))
                {
                    // Rounded rectangle border
                    bool corner = (x < 4 && y < 4) || (x < 4 && y >= h - 4) ||
                                  (x >= w - 4 && y < 4) || (x >= w - 4 && y >= h - 4);
                    tex.SetPixel(x, y, corner ? Color.clear : darkEdge);
                }
                else
                {
                    // Simple shading - lighter at top
                    float shade = Mathf.Lerp(1.1f, 0.85f, (float)y / h);
                    Color c = baseColor * shade;
                    c.a = 1f;
                    tex.SetPixel(x, y, c);
                }
            }
        }

        tex.Apply();
        return Sprite.Create(tex, new Rect(0, 0, w, h), new Vector2(0.5f, 0.5f), 32);
    }
}
