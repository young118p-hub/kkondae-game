using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Boss Setup")]
    public string bossName = "김부장";
    public string bossRank = "부장";

    [Header("Stage")]
    public int currentStage = 1;
    public int maxStage = 5;

    [Header("Events")]
    public UnityEvent onStageStart;
    public UnityEvent onStageClear;
    public UnityEvent onGameOver;

    public bool IsPlaying { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        StartStage();
    }

    public void StartStage()
    {
        IsPlaying = true;
        onStageStart?.Invoke();
    }

    public void OnBossDefeated()
    {
        IsPlaying = false;
        onStageClear?.Invoke();

        if (currentStage < maxStage)
        {
            currentStage++;
            Invoke(nameof(StartStage), 2f);
        }
        else
        {
            onGameOver?.Invoke();
        }
    }

    public void SetBossInfo(string name, string rank)
    {
        bossName = name;
        bossRank = rank;
    }
}
