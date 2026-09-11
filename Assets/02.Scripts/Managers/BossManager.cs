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
            string poolTag = BossPrefab.name;
            GameObject boss = ObjectManager.Instance.SpawnFromPool(poolTag, BossSpawnPoint.position, Quaternion.identity);

            if (boss != null)
            {
                EnemyHealth enemyHealth = boss.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.ApplyHealthMultiplier(GameManager.Instance.CurrentHealthMultiplier);
                }
            }
        }
    }

    public void EndBossWave()
    {
        GameManager.Instance.IsBossWave = false;
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ResetTimerColor();
        }
    }
}