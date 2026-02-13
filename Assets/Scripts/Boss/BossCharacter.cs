using UnityEngine;
using UnityEngine.Events;

public class BossCharacter : MonoBehaviour
{
    [Header("Stats")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("Stage Scaling")]
    public float healthPerStage = 30f;

    [Header("Reaction Dialogues")]
    public string[] hitDialogues = new string[]
    {
        "으악!",
        "아야!",
        "이, 이게 뭐하는 짓이야!",
        "그, 그만해!!",
        "내가 누군지 알아?!",
        "사, 살려줘...",
        "다시는 안 그럴게!",
        "으으...",
        "꺄악!",
        "히익!"
    };

    public string[] lowHealthDialogues = new string[]
    {
        "제발 그만...",
        "잘못했어요...",
        "다시는 꼰대짓 안 할게...",
        "살려주세요...",
        "내가 잘못했다..."
    };

    [Header("Events")]
    public UnityEvent<float> onHealthChanged;
    public UnityEvent<string> onDialogue;
    public UnityEvent onDefeated;

    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        ResetBoss();
    }

    public void ResetBoss()
    {
        int stage = GameManager.Instance != null ? GameManager.Instance.currentStage : 1;
        maxHealth = 100f + (stage - 1) * healthPerStage;
        currentHealth = maxHealth;
        onHealthChanged?.Invoke(currentHealth / maxHealth);
    }

    public void TakeDamage(float damage, string bodyPartName)
    {
        if (currentHealth <= 0f) return;

        currentHealth -= damage;
        currentHealth = Mathf.Max(0f, currentHealth);

        onHealthChanged?.Invoke(currentHealth / maxHealth);

        string dialogue = GetRandomDialogue();
        onDialogue?.Invoke(dialogue);

        if (animator != null)
        {
            animator.SetTrigger("Hit");
        }

        if (currentHealth <= 0f)
        {
            OnDefeated();
        }
    }

    string GetRandomDialogue()
    {
        float healthRatio = currentHealth / maxHealth;

        if (healthRatio < 0.3f && lowHealthDialogues.Length > 0)
        {
            return lowHealthDialogues[Random.Range(0, lowHealthDialogues.Length)];
        }

        if (hitDialogues.Length > 0)
        {
            return hitDialogues[Random.Range(0, hitDialogues.Length)];
        }

        return "으악!";
    }

    void OnDefeated()
    {
        if (animator != null)
        {
            animator.SetTrigger("Defeated");
        }

        onDefeated?.Invoke();

        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnBossDefeated();
        }
    }
}
