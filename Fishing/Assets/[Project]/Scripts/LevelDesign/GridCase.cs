using UnityEngine;

public class GridCase : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    
    public void Start()
    {
        if(!_spriteRenderer) _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _spriteRenderer.color = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
    }
}