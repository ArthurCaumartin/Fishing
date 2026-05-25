using UnityEngine;

public class CameraControler : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Transform _hubTransform;
    [SerializeField] private float _trackSpeed = 5;
    private Transform _target;

    private void Awake()
    {
        TargetHub();
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

    public void TargetPlayer() => _target = _playerTransform;
    public void TargetHub() => _target = _hubTransform;
}