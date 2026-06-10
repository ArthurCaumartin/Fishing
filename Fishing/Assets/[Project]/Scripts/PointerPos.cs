using UnityEngine;
using UnityEngine.InputSystem;

public class PointerPos : MonoBehaviour
{
    public Vector3 _pointerPos;

    void Update()
    {
        transform.position = _pointerPos;
    }

    public void OnMovePointer(InputValue value)
    {
        _pointerPos =
        Camera.main.ScreenToWorldPoint(value.Get<Vector2>());
        _pointerPos.z = 0;
    }
}