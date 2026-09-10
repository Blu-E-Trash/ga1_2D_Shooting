using UnityEngine;

public class EnemyLoot : MonoBehaviour
{
    [SerializeField] private GameObject[] _buffItem;
    [SerializeField] private int _dropRate;

    public void TryDropItem()
    {
        if (_buffItem == null || _buffItem.Length == 0) return;

        int randomChance = Random.Range(0, 100);
        if (randomChance < _dropRate)
        {
            int index = Random.Range(0, _buffItem.Length);
            Instantiate(_buffItem[index], transform.position, Quaternion.identity);
        }
    }
}