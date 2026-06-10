using UnityEngine;

public class LevelCase : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    
    public void Start()
    {
        if(!_spriteRenderer) _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.color = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
    }
}