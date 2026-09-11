using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("적 스폰 데이터 (가중치 기반)")]
    [SerializeField]
    private EnemySpawnData[] _spawnDatas;

    [Header("적 생성 간격")]
    private float _spawnInterval = 2.0f;
    private float _timer;

    [Header("적 생성 위치")]
    [SerializeField]
    private Transform[] _spawnPoints;

    private int _totalWeight = 0;

    private void Start()
    {
        CalculateTotalWeight();
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= _spawnInterval)
        {
            SpawnEnemy();

            _spawnInterval = Random.Range(1.0f, 3.0f);
            _timer = 0f;
        }
    }

    private void CalculateTotalWeight()
    {
        _totalWeight = 0;
        if (_spawnDatas != null)
        {
            foreach (EnemySpawnData data in _spawnDatas)
            {
                _totalWeight += data.Weight;
            }
        }
    }

    private void SpawnEnemy()
    {
        // 1. 가중치 기반으로 적 프리팹 선택
        GameObject enemyPrefabToSpawn = GetRandomEnemyPrefab();

        // 2. 랜덤 스폰 위치 선택
        int randomSpawnPointIndex = Random.Range(0, _spawnPoints.Length);
        Transform spawnPoint = _spawnPoints[randomSpawnPointIndex];

        // 3. 적 생성
        GameObject spawnedEnemy = Instantiate(enemyPrefabToSpawn, spawnPoint.position, Quaternion.identity);

        // 4. 체력 배율 적용
        EnemyHealth enemyHealth = spawnedEnemy.GetComponent<EnemyHealth>();
        if (enemyHealth != null && GameManager.Instance != null)
        {
            float currentMultiplier = GameManager.Instance.CurrentHealthMultiplier;
            enemyHealth.ApplyHealthMultiplier(currentMultiplier);
        }
    }

    private GameObject GetRandomEnemyPrefab()
    {
        if (_totalWeight <= 0) return null;

        int randomWeight = Random.Range(0, _totalWeight);
        int cumulativeWeight = 0;

        foreach (EnemySpawnData data in _spawnDatas)
        {
            cumulativeWeight += data.Weight;
            if (randomWeight < cumulativeWeight)
            {
                return data.EnemyPrefab;
            }
        }

        return null;
    }
}