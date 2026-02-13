using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueUI : MonoBehaviour
{
    [Header("References (auto-found if null)")]
    public TextMeshProUGUI dialogueText;
    public CanvasGroup canvasGroup;

    [Header("Settings")]
    public float displayDuration = 1.5f;
    public float fadeSpeed = 3f;

    Coroutine currentDialogue;

    void Awake()
    {
        if (dialogueText == null)
        {
            Transform t = transform.Find("DialogueText");
            if (t != null) dialogueText = t.GetComponent<TextMeshProUGUI>();
        }
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }
    }

    public void ShowDialogue(string text)
    {
        if (currentDialogue != null)
        {
            StopCoroutine(currentDialogue);
        }
        currentDialogue = StartCoroutine(DialogueRoutine(text));
    }

    IEnumerator DialogueRoutine(string text)
    {
        if (dialogueText != null)
        {
            dialogueText.text = text;
        }

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
        }

        // Scale punch effect
        transform.localScale = Vector3.one * 1.2f;
        float scaleTimer = 0.1f;
        while (scaleTimer > 0f)
        {
            scaleTimer -= Time.deltaTime;
            transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 1.2f, scaleTimer / 0.1f);
            yield return null;
        }
        transform.localScale = Vector3.one;

        yield return new WaitForSeconds(displayDuration);

        // Fade out
        if (canvasGroup != null)
        {
            while (canvasGroup.alpha > 0f)
            {
                canvasGroup.alpha -= fadeSpeed * Time.deltaTime;
                yield return null;
            }
        }
    }
}
