using UnityEngine;

[CreateAssetMenu(fileName = "ItemSpawnDataTable", menuName = "Scriptable Objects/ItemSpawnDataTableSO")]
public class ItemSpawnDataTable : ScriptableObject
{
    public ItemSpawnData[] Datas;
}