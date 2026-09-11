using UnityEngine;

public class EnemyLoot : MonoBehaviour
{
    [Header("Loot Settings")]
    [SerializeField] private ItemSpawnDataTable _lootTable;
    [SerializeField] private int _dropRate;

    public void TryDropItem()
    {
        // SO가 할당되지 않았거나 데이터가 없을 때
        if (_lootTable == null || _lootTable.Datas == null || _lootTable.Datas.Length == 0) return;

        int randomChance = Random.Range(0, 100);
        if (randomChance >= _dropRate) return;

        // 가중치 기반으로 아이템 프리팹 선택
        GameObject itemToSpawn = GetRandomItemPrefab();

        if (itemToSpawn != null)
        {
            Instantiate(itemToSpawn, transform.position, Quaternion.identity);
        }
    }

    private GameObject GetRandomItemPrefab()
    {
        int totalWeight = 0;
        foreach (ItemSpawnData data in _lootTable.Datas)
        {
            totalWeight += data.Weight;
        }

        if (totalWeight <= 0) return null;

        int randomWeight = Random.Range(0, totalWeight);
        int cumulativeWeight = 0;

        foreach (ItemSpawnData data in _lootTable.Datas)
        {
            cumulativeWeight += data.Weight;
            if (randomWeight < cumulativeWeight)
            {
                return data.ItemPrefab;
            }
        }

        return null;
    }
}