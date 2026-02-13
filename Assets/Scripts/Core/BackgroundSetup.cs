using UnityEngine;

/// <summary>
/// Creates a simple office-style background at runtime.
/// </summary>
public class BackgroundSetup : MonoBehaviour
{
    void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null) sr = gameObject.AddComponent<SpriteRenderer>();

        // Create office-like gradient background
        int w = 128;
        int h = 192;
        Texture2D tex = new Texture2D(w, h);
        tex.filterMode = FilterMode.Bilinear;

        Color wallTop = new Color(0.85f, 0.82f, 0.75f);    // Beige wall top
        Color wallBot = new Color(0.75f, 0.72f, 0.65f);     // Beige wall bottom
        Color floorColor = new Color(0.45f, 0.42f, 0.38f);  // Dark floor

        int floorLine = h / 3;

        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                if (y < floorLine)
                {
                    // Floor
                    float shade = 0.95f + 0.05f * ((x + y) % 2);
                    Color c = floorColor * shade;
                    c.a = 1f;
                    tex.SetPixel(x, y, c);
                }
                else
                {
                    // Wall with gradient
                    float t = (float)(y - floorLine) / (h - floorLine);
                    Color c = Color.Lerp(wallBot, wallTop, t);

                    // Subtle vertical lines (wall panels)
                    if (x % 32 < 2)
                    {
                        c *= 0.92f;
                        c.a = 1f;
                    }

                    tex.SetPixel(x, y, c);
                }
            }
        }

        // Baseboard line
        for (int x = 0; x < w; x++)
        {
            for (int y = floorLine - 2; y < floorLine + 2; y++)
            {
                if (y >= 0 && y < h)
                {
                    tex.SetPixel(x, y, new Color(0.35f, 0.32f, 0.28f));
                }
            }
        }

        tex.Apply();

        sr.sprite = Sprite.Create(tex, new Rect(0, 0, w, h), new Vector2(0.5f, 0.5f), 16);
        sr.sortingOrder = -10;

        // Scale to fill camera view
        transform.localScale = new Vector3(1.5f, 1.5f, 1f);
    }
}
