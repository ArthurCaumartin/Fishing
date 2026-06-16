using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _movementSpeed = 5;
    private Vector2 _velocity;

    private void Start()
    {
        if(!CanvasManager.Instance) return;
        CanvasManager.Instance.SetPanelInputAction(
            () => _velocity.x = -1,
            () => _velocity.x = 0,
            () => _velocity.x = 1,
            () => _velocity.x = 0
        );
    }

    private void Update()
    {
        transform.Translate(_velocity * _movementSpeed * Time.deltaTime);
    }

    public void OnMove(InputValue value)
    {
        _velocity = value.Get<Vector2>();
    }
}