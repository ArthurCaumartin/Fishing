using UnityEngine;
using UnityEngine.Rendering;

public class CameraControler : MonoBehaviour
{
    [SerializeField] private float _trackSpeed = 5;
    private Transform _target;
    private Camera _camera;

    private void Start()
    {
        _camera = GetComponent<Camera>();
    }

    private void Update()
    {
        if (!_target) return;
        Vector3 targetPos = _target.position;
        targetPos.z = -10;
        transform.position = Vector3.Lerp(
            transform.position,
            targetPos,
            Time.deltaTime * _trackSpeed
        );
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    public void SetSize(float size)
    {
        _camera.orthographicSize = size;
    }
}