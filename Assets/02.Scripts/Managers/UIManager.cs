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
    public TextMeshProUGUI BestSurvuvedTime; // 오타(Survuved)는 기존 연결 유지를 위해 그대로 두었습니다.

    [Header("New Record UI")]
    public TextMeshProUGUI NewRecordText; // 신기록 축하 텍스트 추가

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (WarningUI != null) WarningUI.SetActive(false);
        if (GameOverPanel != null) GameOverPanel.SetActive(false);

        // 시작할 때 최고 기록 및 축하 텍스트 끄기
        if (BestSurvuvedTime != null) BestSurvuvedTime.gameObject.SetActive(false);
        if (NewRecordText != null) NewRecordText.gameObject.SetActive(false);
    }

    public void UpdateTimerUI(float survivalTime)
    {
        int minutes = Mathf.FloorToInt(survivalTime / 60F);
        int seconds = Mathf.FloorToInt(survivalTime - minutes * 60);
        TimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
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

    // 매개변수에 isNewRecord 추가
    public void ShowGameOver(float finalTime, int finalKillCount, float bestTime, bool isNewRecord)
    {
        StartCoroutine(GameOverRoutine(finalTime, finalKillCount, bestTime, isNewRecord));
    }

    private IEnumerator GameOverRoutine(float survivalTime, int killCount, float bestTime, bool isNewRecord)
    {
        GameOverPanel.SetActive(true);
        GameOverCanvasGroup.alpha = 0f;

        // 텍스트들 초기화 (숨김)
        FinalSurvivalTimeText.gameObject.SetActive(false);
        KillCountText.gameObject.SetActive(false);
        BestSurvuvedTime.gameObject.SetActive(false);
        if (NewRecordText != null) NewRecordText.gameObject.SetActive(false);

        // 1. 패널 페이드 인
        float fadeDuration = 1.5f;
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;
            GameOverCanvasGroup.alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            yield return null;
        }
        GameOverCanvasGroup.alpha = 1f;

        // 2. 총 생존 시간 출력
        yield return new WaitForSecondsRealtime(1.5f);
        int minutes = Mathf.FloorToInt(survivalTime / 60F);
        int seconds = Mathf.FloorToInt(survivalTime - minutes * 60);
        FinalSurvivalTimeText.text = $"총 생존 시간\n{minutes:00}:{seconds:00}";
        FinalSurvivalTimeText.gameObject.SetActive(true);

        // 3. 처치 수 출력
        yield return new WaitForSecondsRealtime(1.5f);
        KillCountText.text = $"처치 수\n{killCount:D6}";
        KillCountText.gameObject.SetActive(true);

        // 4. 최대 생존 시간 출력
        yield return new WaitForSecondsRealtime(1.5f);
        int bestMinutes = Mathf.FloorToInt(bestTime / 60F);
        int bestSeconds = Mathf.FloorToInt(bestTime - bestMinutes * 60);
        BestSurvuvedTime.text = $"최대 생존 시간\n{bestMinutes:00}:{bestSeconds:00}";
        BestSurvuvedTime.gameObject.SetActive(true);

        // 5. 신기록 달성 시 축하 텍스트 출력
        if (isNewRecord && NewRecordText != null)
        {
            yield return new WaitForSecondsRealtime(1.0f); // 약간의 텀을 주고 등장
            NewRecordText.text = "New Record!"; // 유니티 에디터에서 설정해도 되지만 코드로도 설정 가능
            NewRecordText.gameObject.SetActive(true);
        }
    }
}