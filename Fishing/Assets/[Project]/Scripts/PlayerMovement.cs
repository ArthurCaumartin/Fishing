using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _movementSpeed = 5;
    private Vector2 _velocity;
    private Vector2 _currentVel;

    private void Start()
    {
        if(!CanvasManager.Instance) return;

        GameEvents.onDiveStart += () => enabled = false;
        GameEvents.onDiveEnd += () => enabled = true;

        CanvasManager.Instance.SetPanelInputAction(
            () => _velocity.x = -1,
            () => _velocity.x = 0,
            () => _velocity.x = 1,
            () => _velocity.x = 0
        );
    }

    private void Update()
    {
        _currentVel = Vector2.Lerp(_currentVel, _velocity, Time.deltaTime * 3);
        transform.Translate(_currentVel * _movementSpeed * Time.deltaTime);
    }

    public void OnMove(InputValue value)
    {
        _velocity = value.Get<Vector2>();
    }
}