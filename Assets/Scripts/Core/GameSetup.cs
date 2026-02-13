using UnityEngine;

/// <summary>
/// Automatically wires up all game components at runtime.
/// Attach to GameManager object - handles all event connections.
/// </summary>
public class GameSetup : MonoBehaviour
{
    [Header("References")]
    public BossCharacter boss;
    public HealthBarUI healthBarUI;
    public DialogueUI dialogueUI;
    public HitEffectManager hitEffectManager;
    public StageClearUI stageClearUI;

    void Start()
    {
        AutoFindReferences();
        WireEvents();
        Debug.Log($"[GameSetup] boss={boss != null} healthBar={healthBarUI != null} dialogue={dialogueUI != null} hitMgr={hitEffectManager != null} stageClear={stageClearUI != null}");
    }

    void AutoFindReferences()
    {
        if (boss == null) boss = FindAnyObjectByType<BossCharacter>();
        if (healthBarUI == null) healthBarUI = FindAnyObjectByType<HealthBarUI>();
        if (dialogueUI == null) dialogueUI = FindAnyObjectByType<DialogueUI>();
        if (hitEffectManager == null) hitEffectManager = FindAnyObjectByType<HitEffectManager>();
        if (stageClearUI == null) stageClearUI = FindAnyObjectByType<StageClearUI>();
    }

    void WireEvents()
    {
        if (boss != null)
        {
            if (healthBarUI != null)
            {
                boss.onHealthChanged.AddListener(healthBarUI.UpdateHealth);
            }

            if (dialogueUI != null)
            {
                boss.onDialogue.AddListener(dialogueUI.ShowDialogue);
            }
        }

        // Stage clear events
        GameManager gm = GameManager.Instance;
        if (gm != null && stageClearUI != null)
        {
            gm.onStageClear.AddListener(stageClearUI.ShowStageClear);
            gm.onGameOver.AddListener(stageClearUI.ShowGameClear);

            // Reset boss and health bar on new stage
            gm.onStageStart.AddListener(() =>
            {
                if (boss != null) boss.ResetBoss();
                if (healthBarUI != null)
                {
                    healthBarUI.UpdateHealth(1f);
                    healthBarUI.UpdateBossInfo();
                }
            });
        }

        // Body parts -> HitEffectManager
        BodyPart[] bodyParts = FindObjectsByType<BodyPart>(FindObjectsSortMode.None);
        foreach (BodyPart bp in bodyParts)
        {
            if (hitEffectManager != null)
            {
                bp.onHit.AddListener((damage, pos) =>
                {
                    hitEffectManager.SpawnDamagePopup(damage, pos);
                });
            }
        }
    }
}
