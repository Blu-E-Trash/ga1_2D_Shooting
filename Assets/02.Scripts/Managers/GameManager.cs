using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game State")]
    public float SurvivalTime = 0f;
    public float BestSurvivedTime = 0f;
    public int KillCount = 0;

    [Header("Currency / Merit")]
    public int CurrentMerit = 0;

    public float CurrentHealthMultiplier { get; private set; } = 1f;

    public bool IsGameOver { get; private set; } = false;
    public bool IsBossWave = false;
    public bool IsWarning = false;

    // 보스전 페널티용 타이머 변수 추가
    private float _bossPenaltyTimer = 0f;
    private float _bossPenaltyInterval = 2f;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        BestSurvivedTime = PlayerPrefs.GetFloat("BestSurvivalTime", 0f);

        LoadInGameData();
    }

    private void Start()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateMeritUI(CurrentMerit);
            UIManager.Instance.UpdateTimerUI(SurvivalTime);
        }
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

        ClearInGameData();

        bool isNewRecord = false;
        if (SurvivalTime > BestSurvivedTime)
        {
            BestSurvivedTime = SurvivalTime;
            PlayerPrefs.SetFloat("BestSurvivalTime", BestSurvivedTime);
            PlayerPrefs.Save();
            isNewRecord = true;
        }

        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowGameOver(SurvivalTime, KillCount, BestSurvivedTime, isNewRecord);
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

    public void SaveInGameData()
    {
        PlayerPrefs.SetFloat("Run.SurvivalTime", SurvivalTime);
        PlayerPrefs.SetInt("Run.CurrentMerit", CurrentMerit);
        PlayerPrefs.SetInt("Run.KillCount", KillCount);

        PlayerPrefs.SetInt("Run.HasSave", 1);

        PlayerPrefs.Save();
    }

    public void LoadInGameData()
    {
        if (PlayerPrefs.GetInt("Run.HasSave", 0) == 1)
        {
            SurvivalTime = PlayerPrefs.GetFloat("Run.SurvivalTime", 0f);
            CurrentMerit = PlayerPrefs.GetInt("Run.CurrentMerit", 0);
            KillCount = PlayerPrefs.GetInt("Run.KillCount", 0);
        }
        else
        {
            SurvivalTime = 0f;
            CurrentMerit = 0;
            KillCount = 0;
        }
    }

    public void ClearInGameData()
    {
        PlayerPrefs.DeleteKey("Run.SurvivalTime");
        PlayerPrefs.DeleteKey("Run.CurrentMerit");
        PlayerPrefs.DeleteKey("Run.KillCount");
        PlayerPrefs.SetInt("Run.HasSave", 0); // 저장 데이터 없음 처리

        PlayerPrefs.Save();
    }

    private void OnApplicationQuit()
    {
        // 죽지 않고 살아있을 때만 진행 상황을 저장합니다.
        if (!IsGameOver)
        {
            SaveInGameData();
        }
    }
}