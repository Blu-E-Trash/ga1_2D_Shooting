using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("적 스폰 데이터 (Scriptable Object)")]
    [SerializeField]
    private EnemySpawnDataTableSO _spawnDataTable;

    [Header("적 생성 간격")]
    private float _spawnInterval = 2.0f;
    private float _timer;

    [Header("적 생성 위치")]
    [SerializeField]
    private Transform[] _spawnPoints;

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

    private void SpawnEnemy()
    {
        // 방어 코드: SO가 할당되지 않았거나 데이터가 없을 때, 혹은 스폰 포인트가 없을 때
        if (_spawnDataTable == null || _spawnDataTable.Datas == null || _spawnDataTable.Datas.Length == 0) return;
        if (_spawnPoints == null || _spawnPoints.Length == 0) return;

        // 가중치 기반으로 적 프리팹 선택
        GameObject enemyPrefabToSpawn = GetRandomEnemyPrefab();

        if (enemyPrefabToSpawn == null)
        {
            return;
        }

        // 랜덤 스폰 위치 선택
        int randomSpawnPointIndex = Random.Range(0, _spawnPoints.Length);
        Transform spawnPoint = _spawnPoints[randomSpawnPointIndex];

        // 적 생성
        GameObject spawnedEnemy = Instantiate(enemyPrefabToSpawn, spawnPoint.position, Quaternion.identity);

        // 체력 배율 적용
        EnemyHealth enemyHealth = spawnedEnemy.GetComponent<EnemyHealth>();
        if (enemyHealth != null && GameManager.Instance != null)
        {
            float currentMultiplier = GameManager.Instance.CurrentHealthMultiplier;
            enemyHealth.ApplyHealthMultiplier(currentMultiplier);
        }
    }

    private GameObject GetRandomEnemyPrefab()
    {
        int totalWeight = 0;
        foreach (EnemySpawnData data in _spawnDataTable.Datas)
        {
            totalWeight += data.Weight;
        }

        if (totalWeight <= 0) return null;

        int randomWeight = Random.Range(0, totalWeight);
        int cumulativeWeight = 0;

        foreach (EnemySpawnData data in _spawnDataTable.Datas)
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