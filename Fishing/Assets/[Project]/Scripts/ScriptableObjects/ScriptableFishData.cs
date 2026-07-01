using UnityEngine;

[CreateAssetMenu(fileName = "FishData_", menuName = "MLG/FishData")]
public class ScriptableFishData : ScriptableObject
{
    [SerializeField] private Fish _prefab;
    [Header("Stats : ")]
    [SerializeField] private string _name = "name_not_set";
    [SerializeField, TextArea] private string _description = "description_not_set";
    [SerializeField] private FishSize _size = FishSize.NotDefine;
    [SerializeField] private Rarity _rarity = Rarity.NotDefine;

    [Header("Visual : ")]
    [SerializeField] private Sprite _sprite;

    public Fish Prefab => _prefab;
    public string Name => _name;
    public string Description => _description;

    public FishSize Size => _size;
    public Rarity Rarity => _rarity;

    public Sprite Sprite => _sprite;
}






