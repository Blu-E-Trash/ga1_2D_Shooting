using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game State")]
    public float SurvivalTime = 0f;
    public float BestSurvuvedTime = 0f; // 기존 오타 유지
    public int KillCount = 0;

    [Header("Currency / Merit")]
    public int CurrentMerit = 0;

    public float CurrentHealthMultiplier { get; private set; } = 1f;

    public bool IsGameOver { get; private set; } = false;
    public bool IsBossWave = false;
    public bool IsWarning = false;

    // 💡 보스전 페널티용 타이머 변수 추가
    private float _bossPenaltyTimer = 0f;
    private float _bossPenaltyInterval = 2f;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        BestSurvuvedTime = PlayerPrefs.GetFloat("BestSurvivalTime", 0f);
    }

    private void Update()
    {
        if (IsGameOver) return;

        if (!IsBossWave && !IsWarning)
        {
            SurvivalTime += Time.deltaTime;
            UpdateHealthMultiplier();

            if (UIManager.Instance != null)
            {
                UIManager.Instance.UpdateTimerUI(SurvivalTime);
            }

            _bossPenaltyTimer = 0f;
        }
        else if (IsBossWave)
        {
            _bossPenaltyTimer += Time.deltaTime;

            if (_bossPenaltyTimer >= _bossPenaltyInterval)
            {
                _bossPenaltyTimer -= _bossPenaltyInterval;

                if (CurrentMerit > 0)
                {
                    CurrentMerit--;

                    if (UIManager.Instance != null)
                    {
                        UIManager.Instance.UpdateMeritUI(CurrentMerit);
                        UIManager.Instance.FlashMeritText();
                    }
                }
            }
        }
    }

    private void UpdateHealthMultiplier()
    {
        int minutesPassed = Mathf.FloorToInt(SurvivalTime / 60f);
        CurrentHealthMultiplier = 1f + (minutesPassed * 0.1f);
    }

    public void AddKillCount()
    {
        if (!IsGameOver) KillCount++;
    }

    public void AddMerit(int amount)
    {
        if (!IsGameOver)
        {
            CurrentMerit += amount;
            if (UIManager.Instance != null) UIManager.Instance.UpdateMeritUI(CurrentMerit);
        }
    }

    public bool TrySpendMerit(int amount)
    {
        if (CurrentMerit >= amount)
        {
            CurrentMerit -= amount;
            if (UIManager.Instance != null) UIManager.Instance.UpdateMeritUI(CurrentMerit);
            return true;
        }
        return false;
    }

    public void GameOver()
    {
        IsGameOver = true;
        Time.timeScale = 0f;

        bool isNewRecord = false;
        if (SurvivalTime > BestSurvuvedTime)
        {
            BestSurvuvedTime = SurvivalTime;
            PlayerPrefs.SetFloat("BestSurvivalTime", BestSurvuvedTime);
            PlayerPrefs.Save();
            isNewRecord = true;
        }

        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowGameOver(SurvivalTime, KillCount, BestSurvuvedTime, isNewRecord);
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Debug.Log("게임 종료");
        Application.Quit();
    }
}