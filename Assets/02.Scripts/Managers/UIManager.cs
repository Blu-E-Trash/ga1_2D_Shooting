using System.Collections;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("In-Game UI")]
    public TextMeshProUGUI TimerText;
    public TextMeshProUGUI MeritText;
    public GameObject WarningUI;

    [Header("Game Over UI")]
    public GameObject GameOverPanel;
    public CanvasGroup GameOverCanvasGroup;
    public TextMeshProUGUI FinalSurvivalTimeText;
    public TextMeshProUGUI KillCountText;
    public TextMeshProUGUI BestSurvivedTime;

    [Header("New Record UI")]
    public TextMeshProUGUI NewRecordText;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (WarningUI != null) WarningUI.SetActive(false);
        if (GameOverPanel != null) GameOverPanel.SetActive(false);

        if (BestSurvivedTime != null) BestSurvivedTime.gameObject.SetActive(false);
        if (NewRecordText != null) NewRecordText.gameObject.SetActive(false);
        if (MeritText != null) UpdateMeritUI(0);
    }

    public void UpdateTimerUI(float survivalTime)
    {
        int minutes = Mathf.FloorToInt(survivalTime / 60F);
        int seconds = Mathf.FloorToInt(survivalTime - minutes * 60);
        TimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void UpdateMeritUI(int currentMerit)
    {
        if (MeritText != null)
        {
            MeritText.text = $"공훈: {currentMerit} Pt";
        }
    }

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

        onWarningComplete?.Invoke();
    }

    public void ResetTimerColor()
    {
        TimerText.color = Color.white;
    }

    public void ShowGameOver(float finalTime, int finalKillCount, float bestTime, bool isNewRecord)
    {
        StartCoroutine(GameOverRoutine(finalTime, finalKillCount, bestTime, isNewRecord));
    }

    private IEnumerator GameOverRoutine(float survivalTime, int killCount, float bestTime, bool isNewRecord)
    {
        GameOverPanel.SetActive(true);
        GameOverCanvasGroup.alpha = 0f;

        FinalSurvivalTimeText.gameObject.SetActive(false);
        KillCountText.gameObject.SetActive(false);
        BestSurvivedTime.gameObject.SetActive(false);
        if (NewRecordText != null) NewRecordText.gameObject.SetActive(false);

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

        yield return new WaitForSecondsRealtime(1.5f);
        int bestMinutes = Mathf.FloorToInt(bestTime / 60F);
        int bestSeconds = Mathf.FloorToInt(bestTime - bestMinutes * 60);
        BestSurvivedTime.text = $"최대 생존 시간\n{bestMinutes:00}:{bestSeconds:00}";
        BestSurvivedTime.gameObject.SetActive(true);

        if (isNewRecord && NewRecordText != null)
        {
            yield return new WaitForSecondsRealtime(1.0f);
            NewRecordText.text = "New Record!";
            NewRecordText.gameObject.SetActive(true);
        }
    }
    public void FlashMeritText()
    {
        if (MeritText != null && gameObject.activeInHierarchy)
        {
            StartCoroutine(FlashMeritRoutine());
        }
    }

    private IEnumerator FlashMeritRoutine()
    {
        MeritText.color = Color.red;

        yield return new WaitForSeconds(0.2f);

        MeritText.color = Color.white;
    }
}