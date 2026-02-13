using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class StageClearUI : MonoBehaviour
{
    [Header("References (auto-found if null)")]
    public CanvasGroup canvasGroup;
    public TextMeshProUGUI clearText;
    public TextMeshProUGUI subText;

    void Awake()
    {
        if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();
        if (clearText == null)
        {
            Transform t = transform.Find("ClearText");
            if (t != null) clearText = t.GetComponent<TextMeshProUGUI>();
        }
        if (subText == null)
        {
            Transform t = transform.Find("SubText");
            if (t != null) subText = t.GetComponent<TextMeshProUGUI>();
        }

        if (canvasGroup != null) canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
    }

    public void ShowStageClear()
    {
        StartCoroutine(StageClearRoutine());
    }

    public void ShowGameClear()
    {
        if (clearText != null) clearText.text = "GAME CLEAR!";
        if (subText != null) subText.text = "You defeated all bosses!";
        StartCoroutine(StageClearRoutine());
    }

    IEnumerator StageClearRoutine()
    {
        int stage = GameManager.Instance != null ? GameManager.Instance.currentStage : 1;

        if (clearText != null && clearText.text != "GAME CLEAR!")
        {
            clearText.text = $"Stage {stage} Clear!";
        }

        if (subText != null && subText.text != "You defeated all bosses!")
        {
            string bossName = GameManager.Instance != null ? GameManager.Instance.bossName : "Boss";
            subText.text = $"{bossName} defeated!";
        }

        // Fade in
        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = true;
            float t = 0f;
            while (t < 0.5f)
            {
                t += Time.deltaTime;
                canvasGroup.alpha = t / 0.5f;
                yield return null;
            }
            canvasGroup.alpha = 1f;
        }

        yield return new WaitForSeconds(2f);

        // Fade out
        if (canvasGroup != null)
        {
            float t = 0.5f;
            while (t > 0f)
            {
                t -= Time.deltaTime;
                canvasGroup.alpha = t / 0.5f;
                yield return null;
            }
            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = false;
        }
    }
}
