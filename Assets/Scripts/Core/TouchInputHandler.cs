using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Handles touch/click input and raycasts to detect BodyPart hits.
/// Works with Unity's new Input System.
/// </summary>
public class TouchInputHandler : MonoBehaviour
{
    Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;
    }

    void Update()
    {
        // Check for mouse click or touch
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 screenPos = Mouse.current.position.ReadValue();
            TryHitAt(screenPos);
        }

        if (Touchscreen.current != null)
        {
            foreach (var touch in Touchscreen.current.touches)
            {
                if (touch.press.wasPressedThisFrame)
                {
                    Vector2 screenPos = touch.position.ReadValue();
                    TryHitAt(screenPos);
                }
            }
        }
    }

    void TryHitAt(Vector2 screenPos)
    {
        if (mainCam == null) return;

        Vector2 worldPos = mainCam.ScreenToWorldPoint(screenPos);
        RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);

        if (hit.collider != null)
        {
            BodyPart bodyPart = hit.collider.GetComponent<BodyPart>();
            if (bodyPart != null)
            {
                Debug.Log($"[Hit] {bodyPart.partType} at {worldPos}");
                bodyPart.Hit();
            }
        }
    }
}
