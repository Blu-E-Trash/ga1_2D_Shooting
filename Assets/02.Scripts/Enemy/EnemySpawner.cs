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

        // Instantiate 대신 ObjectManager의 풀링 시스템을 사용
        // 프리팹의 이름 자체를 태그(Key)로 사용하여 풀에서 꺼내옵니다.
        string poolTag = enemyPrefabToSpawn.name;
        GameObject spawnedEnemy = ObjectManager.Instance.SpawnFromPool(poolTag, spawnPoint.position, Quaternion.identity);

        // 풀 매니저에 해당 태그가 없어서 null이 반환될 경우를 대비한 방어 코드
        if (spawnedEnemy == null) return;

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