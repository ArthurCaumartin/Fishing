using UnityEditor;
using UnityEngine;


public class FishGenerator : MonoBehaviour
{
    [SerializeField] private ScriptableLootTable _lootTable;


    public Fish GenerateNewFish(Vector3 position, float depth)
    {
        ScriptableFishData selectData = _lootTable.GetLoot();
        Fish newFish = Instantiate(selectData.Prefab, position, Quaternion.identity);
        newFish.Init(selectData);

        return newFish;
    }
}



