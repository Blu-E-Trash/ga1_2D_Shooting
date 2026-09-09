using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("In-Game UI Settings")]
    public Text TimerText; // 화면 상단 타이머 UI
    public GameObject WarningUI; // 붉은 테두리 또는 WARNING 텍스트 오브젝트

    [Header("Game Over UI Settings")]
    public GameObject GameOverPanel; // 게임 오버 판넬 최상위 객체
    public CanvasGroup GameOverCanvasGroup; // 페이드인을 위한 캔버스 그룹
    public Text FinalSurvivalTimeText; // 최종 생존 시간 텍스트
    public Text KillCountText; // 최종 적 처치 수 텍스트

    [Header("Boss Spawning")]
    public GameObject BossPrefab; // 인스펙터에 할당할 보스 프리팹
    public Transform BossSpawnPoint; // 보스가 처음 등장할 위치

    [Header("Game State")]
    public float SurvivalTime = 0f;
    public int KillCount = 0; // 적 처치 수 변수
    public bool IsBossWave = false;
    private bool _isWarning = false;
    private bool _isGameOver = false; // 게임 오버 상태 체크

    // 보스가 등장할 다음 목표 시간
    private float _nextBossThreshold = 120f;

    // 적 체력 배율
    public float CurrentHealthMultiplier { get; private set; } = 1f;

    private void Awake()
    {
        if (WarningUI != null) WarningUI.SetActive(false);

        // 시작 시 게임 오버 판넬 끄기
        if (GameOverPanel != null) GameOverPanel.SetActive(false);

        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        // 게임 오버 상태가 아닐 때만 작동
        if (!_isGameOver && !IsBossWave && !_isWarning)
        {
            SurvivalTime += Time.deltaTime;
            UpdateTimerUI();
            UpdateHealthMultiplier();

            // 목표 시간에 도달하면 보스 출현 전조(Warning) 시작
            if (SurvivalTime >= _nextBossThreshold)
            {
                StartCoroutine(WarningRoutine());
                _nextBossThreshold += 120;
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

            // CanvasGroup의 alpha를 조절하여 깜빡임 연출
            canvasGroup.alpha = Mathf.PingPong(warningTimer * blinkSpeed, 1f);

            yield return null; // 다음 프레임까지 대기
        }

        canvasGroup.alpha = 1f; // 투명도 원상복구
        if (WarningUI != null) WarningUI.SetActive(false);

        IsBossWave = true;
        _isWarning = false;

        SpawnBoss();
    }

    private void SpawnBoss()
    {
        if (BossPrefab != null && BossSpawnPoint != null)
        {
            // 보스 프리팹을 스폰 위치에 생성
            GameObject boss = Instantiate(BossPrefab, BossSpawnPoint.position, Quaternion.identity);

            // 2. 생성된 보스에게 현재 시간 비례 체력 배율 적용
            BossEnemy bossScript = boss.GetComponent<BossEnemy>();
            if (bossScript != null)
            {
                bossScript.ApplyHealthMultiplier(CurrentHealthMultiplier);
            }
            Debug.Log("보스 등장!");
        }
        else
        {
            Debug.LogWarning("보스 프리팹이나 스폰 위치가 할당되지 않았습니다!");
        }
    }

    // 보스가 죽었을 때 외부(보스 스크립트)에서 호출해 줄 함수
    public void EndBossWave()
    {
        IsBossWave = false;
        TimerText.color = Color.white; // 타이머 색상 원상복구
        _nextBossThreshold += 120f; // 다음 보스 등장 시간을 2분(120초) 뒤로 갱신
        Debug.Log("보스 처치! 생존 시간 재개");
    }

    // 적이 죽을 때마다 호출되어 처치 수를 올림
    public void AddKillCount()
    {
        if (!_isGameOver)
        {
            KillCount++;
        }
    }

    // 플레이어가 죽었을 때 호출될 게임 오버 함수
    public void GameOver()
    {
        _isGameOver = true;
        Time.timeScale = 0f; // 게임 내 모든 시간(적 이동, 총알 등) 정지
        StartCoroutine(GameOverRoutine());
    }

    private IEnumerator GameOverRoutine()
    {
        // 판넬 켜고 투명도 0으로 초기화
        GameOverPanel.SetActive(true);
        GameOverCanvasGroup.alpha = 0f;

        // 텍스트들은 일단 꺼둠
        FinalSurvivalTimeText.gameObject.SetActive(false);
        KillCountText.gameObject.SetActive(false);

        // 투명도 0 -> 1 서서히 증가
        float fadeDuration = 1.5f;
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;
            GameOverCanvasGroup.alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            yield return null;
        }
        GameOverCanvasGroup.alpha = 1f; // 완전히 불투명

        // 1.5초 대기 후 총 생존 시간 출력
        yield return new WaitForSecondsRealtime(1.5f);
        int minutes = Mathf.FloorToInt(SurvivalTime / 60F);
        int seconds = Mathf.FloorToInt(SurvivalTime - minutes * 60);
        FinalSurvivalTimeText.text = $"총 생존 시간\n{minutes:00}:{seconds:00}";
        FinalSurvivalTimeText.gameObject.SetActive(true);

        // 1.5초 대기 후 적 처치 수 출력
        yield return new WaitForSecondsRealtime(1.5f);
        KillCountText.text = $"처치 수\n{KillCount:D6}";
        KillCountText.gameObject.SetActive(true);
    }

    // UI 버튼에 연결할 재시작 함수
    public void RestartGame()
    {
        Time.timeScale = 1f; // 멈췄던 시간을 다시 원상복구
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // 현재 씬 다시 로드
    }

    // UI 버튼에 연결할 종료 함수
    public void QuitGame()
    {
        Debug.Log("게임 종료");
        Application.Quit();
    }
}