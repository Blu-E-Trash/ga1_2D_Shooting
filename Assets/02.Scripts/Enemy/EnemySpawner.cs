using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("적 프리팹")]
    [SerializeField]
    private GameObject[] _enemyPrefab;

    private int[] _enemyPool = { 0, 0, 1, 1, 1, 2, 2, 2, 2, 2 };

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
        int randomSpawnPointIndex = Random.Range(0, _spawnPoints.Length);
        int randomPoolIndex = Random.Range(0, _enemyPool.Length);
        int enemyIndexToSpawn = _enemyPool[randomPoolIndex];

        GameObject spawnedEnemy = Instantiate(_enemyPrefab[enemyIndexToSpawn], _spawnPoints[randomSpawnPointIndex].position, Quaternion.identity);

        EnemyHealth enemyHealth = spawnedEnemy.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            float currentMultiplier = GameManager.Instance.CurrentHealthMultiplier;
            enemyHealth.ApplyHealthMultiplier(currentMultiplier);
        }
    }
}