using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // 일정 시간마다 적을 생성하는 스크립트
    [Header("적 프리팹")]
    [SerializeField]
    private GameObject[] _enemyPrefab;

    [Header("적 생성 간격")]
    private float _spawnInterval = 2.0f; // 적 생성 간격 (초 단위)
    private float _timer;

    [Header("적 생성 위치")]
    [SerializeField]
    private Transform[] _spawnPoints; // 적 생성 위치

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= _spawnInterval)
        {
            SpawnEnemy();

            _spawnInterval = Random.Range(1.0f, 3.0f); // 적 생성 간격을 랜덤하게 설정

            _timer = 0f;
        }
    }

    private void SpawnEnemy()
    {
        int randomSpawnPointIndex = Random.Range(0, _spawnPoints.Length);
        int randomEnemyIndex = Random.Range(0, _enemyPrefab.Length);
        Instantiate(_enemyPrefab[randomEnemyIndex], _spawnPoints[randomSpawnPointIndex].position, Quaternion.identity);
    }
}
