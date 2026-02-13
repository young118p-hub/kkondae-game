using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBarUI : MonoBehaviour
{
    [Header("References (auto-found if null)")]
    public RectTransform healthFillRect;
    public RectTransform healthDelayRect;
    public Image healthFillImage;
    public Image healthDelayImage;
    public TextMeshProUGUI bossNameText;
    public TextMeshProUGUI stageText;

    [Header("Settings")]
    public float delaySpeed = 2f;
    public Color healthyColor = new Color(0.2f, 0.8f, 0.2f);
    public Color damagedColor = new Color(0.8f, 0.8f, 0.2f);
    public Color criticalColor = new Color(0.8f, 0.2f, 0.2f);

    float targetRatio = 1f;
    float delayRatio = 1f;

    void Awake()
    {
        AutoFindReferences();
    }

    void Start()
    {
        UpdateBossInfo();
    }

    void AutoFindReferences()
    {
        Transform fillT = transform.Find("HealthBarFill");
        if (fillT != null)
        {
            healthFillRect = fillT.GetComponent<RectTransform>();
            healthFillImage = fillT.GetComponent<Image>();
        }

        Transform delayT = transform.Find("HealthBarDelay");
        if (delayT != null)
        {
            healthDelayRect = delayT.GetComponent<RectTransform>();
            healthDelayImage = delayT.GetComponent<Image>();
        }

        Transform nameT = transform.Find("BossNameText");
        if (nameT != null) bossNameText = nameT.GetComponent<TextMeshProUGUI>();

        Transform canvas = transform.parent;
        if (canvas != null)
        {
            Transform stageT = canvas.Find("StageText");
            if (stageT != null) stageText = stageT.GetComponent<TextMeshProUGUI>();
        }
    }

    void Update()
    {
        // Delayed bar smoothly follows
        delayRatio = Mathf.MoveTowards(delayRatio, targetRatio, delaySpeed * Time.deltaTime);
        SetBarWidth(healthDelayRect, delayRatio);
    }

    public void UpdateHealth(float ratio)
    {
        targetRatio = ratio;
        SetBarWidth(healthFillRect, ratio);

        if (healthFillImage != null)
        {
            healthFillImage.color = GetHealthColor(ratio);
        }
    }

    void SetBarWidth(RectTransform bar, float ratio)
    {
        if (bar == null) return;
        // Scale anchorMax.x to represent fill (bar stretches from left)
        bar.anchorMin = new Vector2(0f, 0f);
        bar.anchorMax = new Vector2(ratio, 1f);
        bar.offsetMin = new Vector2(4, 4);
        bar.offsetMax = new Vector2(-4, -4);
    }

    public void UpdateBossInfo()
    {
        if (GameManager.Instance == null) return;

        if (bossNameText != null)
        {
            bossNameText.text = $"{GameManager.Instance.bossRank} {GameManager.Instance.bossName}";
        }

        if (stageText != null)
        {
            stageText.text = $"Stage {GameManager.Instance.currentStage}";
        }
    }

    Color GetHealthColor(float ratio)
    {
        if (ratio > 0.6f) return healthyColor;
        if (ratio > 0.3f) return damagedColor;
        return criticalColor;
    }
}
