using UnityEngine;

// 데이터 클래스: 순수하게 데이터(값)를 보관하고 전달하는 목적으로 만든 특별한 클래스
[System.Serializable]
public class EnemySpawnData
{
    public GameObject EnemyPrefab;
    public int Weight;
}
