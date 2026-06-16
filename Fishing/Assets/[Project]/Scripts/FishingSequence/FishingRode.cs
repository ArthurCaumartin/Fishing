using System;
using System.Collections;
using System.Collections.Generic;
using Alchemy.Inspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[Serializable]
public struct LinePathPoint
{
    public Vector2 startPos;
    public Vector2 endPos;
    public float startTime;
    public float endTime;
    float distance;

    public LinePathPoint(Vector2 startPos, Vector2 endPos, float startTime, float endTime, float distance)
    {
        this.startPos = startPos;
        this.endPos = endPos;

        this.startTime = startTime;
        this.endTime = endTime;
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
    [Header("Visual : ")]
    [SerializeField] private LineRenderer _lineRendere;

    private Vector3 _pointerPosition;
    private Vector2 _movementDir = Vector2.down;
    private List<LinePathPoint> _linePathList = new List<LinePathPoint>();
    private Vector2 _lastPathPosRecord;
    private Coroutine _diveCoroutine = null;

    public Transform HookTransform => _hookTransorm;
    public float DistanceTravelTime => _currentTravelDistance / _maxTravelDistance;
    public float MaxTravelDistance => _maxTravelDistance;

    private Action _onDiveEnd = null;

    public void StartHookDive(Vector3 startPointerPos, Action onDiveEnd)
    {
        if (_diveCoroutine != null) return;
        enabled = true;
        _hookTransorm.localPosition = Vector3.zero;
        _pointerPosition = startPointerPos;
        _lastPathPosRecord = _hookTransorm.position;

        _onDiveEnd = onDiveEnd;

        _diveCoroutine = StartCoroutine(HookDiveCoroutine());
    }

    private IEnumerator HookDiveCoroutine()
    {
        while (_currentTravelDistance < _maxTravelDistance)
        {
            MoveHook();
            RecordPath();
            DetectObstacle();
            yield return null;
        }
        EndDive();
        // _lineRendere.enabled = false;
    }

    private void MoveHook()
    {
        Vector2 pointerDir = _pointerPosition - _hookTransorm.position;
        _movementDir = Vector2.Lerp(_movementDir, pointerDir, Time.deltaTime * _trackSpeed);
        transform.Translate(_movementDir.normalized * _movementSpeed * Time.deltaTime, Space.World);
        _currentTravelDistance += Time.deltaTime * _movementSpeed;
    }

    private void EndDive()
    {
        RecordPath(isLastPathPoint: true);
        _onDiveEnd.Invoke();
        _diveCoroutine = null;
    }

    private void RecordPath(bool isLastPathPoint = false)
    {
        if (isLastPathPoint)
        {
            LinePathPoint newPathPoint = new LinePathPoint(
                _lastPathPosRecord,
                _hookTransorm.position,
                _linePathList[_linePathList.Count - 1].endTime,
                1,
                Vector2.Distance(_lastPathPosRecord, _hookTransorm.position)
            );
            _linePathList.Add(newPathPoint);
            return;
        }

        float distance = Vector2.Distance(_lastPathPosRecord, _hookTransorm.position);
        if (distance >= _pathResolution)
        {
            float startTime = _linePathList.Count > 0 ? _linePathList[_linePathList.Count - 1].endTime : 0;
            float endTime = _currentTravelDistance / _maxTravelDistance;

            LinePathPoint newPathPoint = new LinePathPoint(
                _lastPathPosRecord,
                _hookTransorm.position,
                startTime,
                endTime,
                distance
            );
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
            EndDive();
        }
    }

    public Vector2 GetPositionOnPath(float time)
    {
        if (time >= 1) return _linePathList[_linePathList.Count - 1].endPos;
        if (time <= 0) return _linePathList[0].startPos;

        for (int i = 0; i < _linePathList.Count; i++)
        {
            if (time > _linePathList[i].startTime && time < _linePathList[i].endTime)
            {
                float pointTime = Mathf.InverseLerp(_linePathList[i].startTime, _linePathList[i].endTime, time);
                return Vector2.Lerp(_linePathList[i].startPos, _linePathList[i].endPos, pointTime);
            }
        }
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
            Gizmos.DrawSphere(_linePathList[i].startPos, _pathResolution / 2);
            Gizmos.DrawLine(_linePathList[i].startPos, _linePathList[i].endPos);
        }
    }
}
