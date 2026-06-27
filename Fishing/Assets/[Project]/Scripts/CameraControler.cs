using UnityEngine;

// [ExecuteInEditMode]
public class CameraControler : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [Space]
    [SerializeField] private Transform _target;
    [SerializeField] private float _trackSpeed = 5;
    [Space]
    [SerializeField] private Vector2 _playerOffSet;
    [SerializeField] private Vector2 _diveOffSet;
    private Camera _camera;
    private float _orthoSizeStart;
    private float _orthoSizeTarget;
    private Vector2 _offSet;

    public Vector2 PlayerOffSet => _playerOffSet;
    public Vector2 DiveOffSet => _diveOffSet;


    private void Start()
    {
        _camera = GetComponent<Camera>();
        _orthoSizeStart = _camera.orthographicSize;
        _orthoSizeTarget = _orthoSizeStart;

        GameEvents.onDiveEnd += () => SetTarget(_player, PlayerOffSet);
    }

    private void Update()
    {
        if(!_target || !_camera) return;
        _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, _orthoSizeTarget, Time.deltaTime * 10);

        Vector3 targetPos = _target.position + (Vector3)_offSet;
        targetPos.z = -10;
        transform.position = Vector3.Lerp(
            transform.position,
            targetPos,
            Time.deltaTime * _trackSpeed
        );
    }

    public void SetTarget(Transform target, Vector2 offSet = new Vector2())
    {
        print("Cam Target set to : " + target.name);
        _target = target;
        _offSet = offSet;
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