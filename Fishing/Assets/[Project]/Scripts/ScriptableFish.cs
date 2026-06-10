using UnityEngine;

[CreateAssetMenu(fileName = "Fish_", menuName = "MLG/Fish")]
public class ScriptableFish : ScriptableObject
{
    [SerializeField] private Sprite _sprite;
    [SerializeField] private float _strenght = 5;

    public Sprite Sprite => _sprite;
}

