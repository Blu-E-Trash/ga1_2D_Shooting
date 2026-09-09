using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game State")]
    public float SurvivalTime = 0f;
    public int KillCount = 0;
    public float CurrentHealthMultiplier { get; private set; } = 1f;

    public bool IsGameOver { get; private set; } = false;
    public bool IsBossWave = false;
    public bool IsWarning = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        if (!IsGameOver && !IsBossWave && !IsWarning)
        {
            SurvivalTime += Time.deltaTime;
            UpdateHealthMultiplier();

            if (UIManager.Instance != null)
            {
                UIManager.Instance.UpdateTimerUI(SurvivalTime);
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

    public void GameOver()
    {
        IsGameOver = true;
        Time.timeScale = 0f;

        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowGameOver(SurvivalTime, KillCount);
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