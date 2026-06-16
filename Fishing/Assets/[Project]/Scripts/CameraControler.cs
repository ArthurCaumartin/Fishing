using UnityEngine;
using UnityEngine.Rendering;

public class CameraControler : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [Space]
    [SerializeField] private Transform _target;
    [SerializeField] private float _trackSpeed = 5;
    private Camera _camera;
    private float _orthoSizeStart;
    private float _orthoSizeTarget;

    private void Start()
    {
        _camera = GetComponent<Camera>();
        _orthoSizeStart = _camera.orthographicSize;
        _orthoSizeTarget = _orthoSizeStart;

        GameEvents.onDiveEnd += () => SetTarget(_player);
    }

    private void Update()
    {
        _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, _orthoSizeTarget, Time.deltaTime * 10);

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
        print("Cam Target set to : " + target.name);
        _target = target;
    }

    public void SetSize(float size)
    {
        _camera.orthographicSize = size;
    }

    public void SetOrtoSize(float size = 0)
    {
        if(size != 0)
        {
            _orthoSizeTarget = size;
            return;   
        }
        _orthoSizeTarget = _orthoSizeStart;
    }
}