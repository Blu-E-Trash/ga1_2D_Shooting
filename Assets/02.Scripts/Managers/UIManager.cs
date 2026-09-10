using System.Collections;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("In-Game UI")]
    public TextMeshProUGUI TimerText;
    public GameObject WarningUI;

    [Header("Game Over UI")]
    public GameObject GameOverPanel;
    public CanvasGroup GameOverCanvasGroup;
    public TextMeshProUGUI FinalSurvivalTimeText;
    public TextMeshProUGUI KillCountText;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (WarningUI != null) WarningUI.SetActive(false);
        if (GameOverPanel != null) GameOverPanel.SetActive(false);
    }

    public void UpdateTimerUI(float survivalTime)
    {
        int minutes = Mathf.FloorToInt(survivalTime / 60F);
        int seconds = Mathf.FloorToInt(survivalTime - minutes * 60);
        TimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // BossManager가 호출할 경고 연출 시작 함수
    public void StartWarningRoutine(System.Action onWarningComplete)
    {
        StartCoroutine(WarningRoutine(onWarningComplete));
    }

    private IEnumerator WarningRoutine(System.Action onWarningComplete)
    {
        TimerText.color = Color.red;
        if (WarningUI != null) WarningUI.SetActive(true);

        CanvasGroup canvasGroup = WarningUI.GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = WarningUI.AddComponent<CanvasGroup>();

        float warningTimer = 0f;
        float blinkSpeed = 4f;

        while (warningTimer < 3f)
        {
            warningTimer += Time.deltaTime;
            canvasGroup.alpha = Mathf.PingPong(warningTimer * blinkSpeed, 1f);
            yield return null;
        }

        canvasGroup.alpha = 1f;
        if (WarningUI != null) WarningUI.SetActive(false);

        // 3초 연출이 끝나면 BossManager에게 완료되었다고 알려줌
        onWarningComplete?.Invoke();
    }

    public void ResetTimerColor()
    {
        TimerText.color = Color.white;
    }

    // GameManager가 호출할 게임오버 연출 시작 함수
    public void ShowGameOver(float finalTime, int finalKillCount)
    {
        StartCoroutine(GameOverRoutine(finalTime, finalKillCount));
    }

    private IEnumerator GameOverRoutine(float survivalTime, int killCount)
    {
        GameOverPanel.SetActive(true);
        GameOverCanvasGroup.alpha = 0f;
        FinalSurvivalTimeText.gameObject.SetActive(false);
        KillCountText.gameObject.SetActive(false);

        float fadeDuration = 1.5f;
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;
            GameOverCanvasGroup.alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            yield return null;
        }
        GameOverCanvasGroup.alpha = 1f;

        yield return new WaitForSecondsRealtime(1.5f);
        int minutes = Mathf.FloorToInt(survivalTime / 60F);
        int seconds = Mathf.FloorToInt(survivalTime - minutes * 60);
        FinalSurvivalTimeText.text = $"총 생존 시간\n{minutes:00}:{seconds:00}";
        FinalSurvivalTimeText.gameObject.SetActive(true);

        yield return new WaitForSecondsRealtime(1.5f);
        KillCountText.text = $"처치 수\n{killCount:D6}";
        KillCountText.gameObject.SetActive(true);
    }
}