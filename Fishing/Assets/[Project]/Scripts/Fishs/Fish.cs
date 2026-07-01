using UnityEngine;

public class Fish : MonoBehaviour
{
    [SerializeField] private ScriptableFishData _fishData;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    

    public void Init(ScriptableFishData fishData)
    {
        if(!fishData) return;
        _fishData = fishData;

        name = "Fish_" + _fishData.Name;
        _spriteRenderer.sprite = _fishData.Sprite;
    }


    public void Hook(FishingRode rode)
    {
        transform.parent = rode.HookTransform;
    }
}



