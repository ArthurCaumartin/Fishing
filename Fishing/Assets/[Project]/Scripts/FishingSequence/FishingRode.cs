using System.Collections.Generic;
using Alchemy.Inspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public struct LinePathPoint
{
    public Vector2 start;
    public Vector2 end;
    public float distance;

    public LinePathPoint(Vector2 start, Vector2 end, float distance)
    {
        this.start = start;
        this.end = end;
        this.distance = distance;
    }
}

public class FishingRode : MonoBehaviour
{
    [SerializeField] private Transform _hookTransorm;
    [Header("Stats : ")]
    [SerializeField] private float _maxTravelDistance = 10;
    [SerializeField] private float _trackSpeed = 5;
    [SerializeField] private float _movementSpeed = 5;
    [SerializeField, ReadOnly] private float _currentTravelDistance = 0;
    [Header("Hook Detection : ")]
    [SerializeField] private float _hookDetectionRadius = .5f;
    [SerializeField] private LayerMask _hookDetectionLayer;
    [Header("Line Path : ")]
    [SerializeField, Range(0.01f, 1)] private float _pathResolution;
    private Vector3 _pointerPosition;
    private Vector2 _movementDir = Vector2.down;

    public float DistanceTravelTime => _currentTravelDistance / _maxTravelDistance;
    private List<LinePathPoint> _linePathList = new List<LinePathPoint>();
    private Vector2 _lastPathPosRecord;

    [HideInInspector] public UnityEvent OnDropEnd = new UnityEvent();
    public Transform HookTransform => _hookTransorm;

    public void EnableHookFollow(Vector3 startPointerPos)
    {
        enabled = true;
        _pointerPosition = startPointerPos;

        _lastPathPosRecord = _hookTransorm.position;
    }

    void Update()
    {
        if (_currentTravelDistance >= _maxTravelDistance) return;
        HookDrop();
        RecordPath();
        DetectObstacle();
    }

    private void HookDrop()
    {
        Vector2 pointerDir = _pointerPosition - _hookTransorm.position;
        _movementDir = Vector2.Lerp(_movementDir, pointerDir, Time.deltaTime * _trackSpeed);
        transform.Translate(_movementDir.normalized * _movementSpeed * Time.deltaTime, Space.World);
        _currentTravelDistance += Time.deltaTime * _movementSpeed;
    }

    private void RecordPath()
    {
        float distance = Vector2.Distance(_lastPathPosRecord, _hookTransorm.position);
        if (distance >= _pathResolution)
        {
            LinePathPoint newPathPoint = new LinePathPoint(_lastPathPosRecord, _hookTransorm.position, distance);
            _linePathList.Add(newPathPoint);

            _lastPathPosRecord = _hookTransorm.position;
        }
    }

    private void DetectObstacle()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(_hookTransorm.position, _hookDetectionRadius, _hookDetectionLayer);
        if (cols.Length > 0)
        {
            _currentTravelDistance = _maxTravelDistance;
            OnDropEnd.Invoke();
        }
    }


    public Vector2 GetPositionOnPath(float time)
    {
        //TODO hehehe


        return Vector2.zero;
    }

    public void OnMovePointer(InputValue value)
    {
        Vector3 screenPos = value.Get<Vector2>();
        _pointerPosition = Camera.main.ScreenToWorldPoint(screenPos);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_hookTransorm.position, _hookDetectionRadius);

        if (_linePathList.Count == 0 || _linePathList == null) return;

        Gizmos.color = Color.red;
        for (int i = 0; i < _linePathList.Count; i++)
        {
            Gizmos.DrawSphere(_linePathList[i].start, _pathResolution / 2);
            Gizmos.DrawLine(_linePathList[i].start, _linePathList[i].end);
        }

    }
}