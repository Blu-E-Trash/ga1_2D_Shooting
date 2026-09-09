using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI Settings")]
    public Text timerText; // 화면 상단 타이머 UI
    public GameObject warningUI; // 붉은 테두리 또는 WARNING 텍스트 오브젝트

    [Header("Game State")]
    public float survivalTime = 0f;
    public bool isBossWave = false;
    private bool isWarning = false;

    // 보스가 등장할 다음 목표 시간
    private float nextBossThreshold = 60f;

    // 적 체력 배율
    public float CurrentHealthMultiplier { get; private set; } = 1f;

    private void Awake()
    {
        warningUI.SetActive(false);
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        // 보스 웨이브나 경고 상태가 아닐 때만 생존 시간이 흐름
        if (!isBossWave && !isWarning)
        {
            survivalTime += Time.deltaTime;
            UpdateTimerUI();
            UpdateHealthMultiplier();

            // 목표 시간에 도달하면 보스 출현 전조(Warning) 시작
            if (survivalTime >= nextBossThreshold)
            {
                StartCoroutine(WarningRoutine());
            }
        }
    }

    private void UpdateTimerUI()
    {
        // MM:SS 포맷으로 변환
        int minutes = Mathf.FloorToInt(survivalTime / 60F);
        int seconds = Mathf.FloorToInt(survivalTime - minutes * 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void UpdateHealthMultiplier()
    {
        // 1분(60초)마다 체력이 10%(0.1)씩 증가
        int minutesPassed = Mathf.FloorToInt(survivalTime / 60f);
        CurrentHealthMultiplier = 1f + (minutesPassed * 0.1f);
    }

    private IEnumerator WarningRoutine()
    {
        isWarning = true;
        timerText.color = Color.red; // 타이머를 붉은색으로

        if (warningUI != null) warningUI.SetActive(true);

        // CanvasGroup 컴포넌트 가져오기
        CanvasGroup canvasGroup = warningUI.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = warningUI.AddComponent<CanvasGroup>();
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
        if (warningUI != null) warningUI.SetActive(false);

        isBossWave = true;
        isWarning = false;

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
        isBossWave = false;
        timerText.color = Color.white; // 타이머 색상 원상복구
        nextBossThreshold += 60f; // 다음 보스 등장 시간 갱신 (임시)
        Debug.Log("보스 처치! 생존 시간 재개");
    }
}