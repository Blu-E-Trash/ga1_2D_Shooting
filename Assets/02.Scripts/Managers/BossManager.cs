using UnityEngine;

public class BossManager : MonoBehaviour
{
    public static BossManager Instance { get; private set; }

    [Header("Boss Spawning")]
    public GameObject BossPrefab;
    public Transform BossSpawnPoint;

    private float _nextBossThreshold = 120f;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        if (GameManager.Instance == null || GameManager.Instance.IsGameOver) return;

        // 보스전이나 경고 상태가 아닐 때 시간 체크
        if (!GameManager.Instance.IsBossWave && !GameManager.Instance.IsWarning)
        {
            if (GameManager.Instance.SurvivalTime >= _nextBossThreshold)
            {
                GameManager.Instance.IsWarning = true;
                _nextBossThreshold += 120f;

                UIManager.Instance.StartWarningRoutine(SpawnBoss);
            }
        }
    }

    private void SpawnBoss()
    {
        GameManager.Instance.IsWarning = false;
        GameManager.Instance.IsBossWave = true;

        if (BossPrefab != null && BossSpawnPoint != null)
        {
            GameObject boss = Instantiate(BossPrefab, BossSpawnPoint.position, Quaternion.identity);

            BossEnemy bossScript = boss.GetComponent<BossEnemy>();
            if (bossScript != null)
            {
                bossScript.ApplyHealthMultiplier(GameManager.Instance.CurrentHealthMultiplier);
            }
            Debug.Log("보스 등장!");
        }
    }

    // 보스가 죽었을 때 외부에서 호출해 줄 함수
    public void EndBossWave()
    {
        GameManager.Instance.IsBossWave = false;
        UIManager.Instance.ResetTimerColor();
        Debug.Log("보스 처치! 생존 시간 재개");
    }
}