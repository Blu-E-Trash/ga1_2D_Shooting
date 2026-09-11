using UnityEngine;

[CreateAssetMenu(fileName = "ItemSpawnDataTable", menuName = "Scriptable Objects/ItemSpawnDataTableSO")]
public class ItemSpawnDataTableSO : ScriptableObject
{
    public ItemSpawnData[] Datas;
}