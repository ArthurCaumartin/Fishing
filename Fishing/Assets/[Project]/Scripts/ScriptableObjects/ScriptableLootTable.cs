using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LootTable_", menuName = "MLG/LootTable")]
public class ScriptableLootTable : ScriptableObject
{
    [SerializeField] private List<ScriptableFishData> _lootableList;

    public ScriptableFishData GetLoot()
    {
        return _lootableList.GetRandom();
    }
}






