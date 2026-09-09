using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI Settings")]
    public Text TimerText; // 화면 상단 타이머 UI
    public GameObject WarningUI; // 붉은 테두리 또는 WARNING 텍스트 오브젝트

    [Header("Game State")]
    public float SurvivalTime = 0f;
    public bool IsBossWave = false;
    private bool _isWarning = false;

    // 보스가 등장할 다음 목표 시간
    private float _nextBossThreshold = 60f;

    // 적 체력 배율
    public float CurrentHealthMultiplier { get; private set; } = 1f;

    private void Awake()
    {
        WarningUI.SetActive(false);
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        // 보스 웨이브나 경고 상태가 아닐 때만 생존 시간이 흐름
        if (!IsBossWave && !_isWarning)
        {
            SurvivalTime += Time.deltaTime;
            UpdateTimerUI();
            UpdateHealthMultiplier();

            // 목표 시간에 도달하면 보스 출현 전조(Warning) 시작
            if (SurvivalTime >= _nextBossThreshold)
            {
                StartCoroutine(WarningRoutine());
            }
        }
    }

    private void UpdateTimerUI()
    {
        // MM:SS 포맷으로 변환
        int minutes = Mathf.FloorToInt(SurvivalTime / 60F);
        int seconds = Mathf.FloorToInt(SurvivalTime - minutes * 60);
        TimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void UpdateHealthMultiplier()
    {
        // 1분(60초)마다 체력이 10%(0.1)씩 증가
        int minutesPassed = Mathf.FloorToInt(SurvivalTime / 60f);
        CurrentHealthMultiplier = 1f + (minutesPassed * 0.1f);
    }

    private IEnumerator WarningRoutine()
    {
        _isWarning = true;
        TimerText.color = Color.red; // 타이머를 붉은색으로

        if (WarningUI != null) WarningUI.SetActive(true);

        // CanvasGroup 컴포넌트 가져오기
        CanvasGroup canvasGroup = WarningUI.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = WarningUI.AddComponent<CanvasGroup>();
        }

        float warningTimer = 0f;
        float blinkSpeed = 4f; // 깜빡이는 속도 (수치를 높이면 더 빨리 깜빡거림)

        // 3초 동안 매 프레임 실행되는 반복문
        while (warningTimer < 3f)
        {
            warningTimer += Time.deltaTime;

            // CanvasGroup의 alpha를 조절하면
            canvasGroup.alpha = Mathf.PingPong(warningTimer * blinkSpeed, 1f);

            yield return null; // 다음 프레임까지 대기
        }

        canvasGroup.alpha = 1f;
        if (WarningUI != null) WarningUI.SetActive(false);

        IsBossWave = true;
        _isWarning = false;

        SpawnBoss();
    }

    private void SpawnBoss()
    {
        // TODO: 보스 생성 로직
        Debug.Log("보스 등장!");
    }

    // 보스가 죽었을 때 외부(보스 스크립트)에서 호출해 줄 함수
    public void EndBossWave()
    {
        IsBossWave = false;
        TimerText.color = Color.white; // 타이머 색상 원상복구
        _nextBossThreshold += 60f; // 다음 보스 등장 시간 갱신 (임시)
        Debug.Log("보스 처치! 생존 시간 재개");
    }
}