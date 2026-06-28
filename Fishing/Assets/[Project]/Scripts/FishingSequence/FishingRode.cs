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
    [SerializeField] private TrailRenderer _trailRenderer;
    [SerializeField] private LineRenderer _lineRenderer;

    private Vector3 _pointerPosition;
    private Vector2 _movementDir = Vector2.down;
    [SerializeField, ReadOnly] private List<LinePathPoint> _linePathList = new List<LinePathPoint>();
    private Vector2 _lastPathPosRecord;
    private Coroutine _diveCoroutine = null;

    public Transform HookTransform => _hookTransorm;
    public float DistanceTravelTime => _currentTravelDistance / _maxTravelDistance;
    public float CurrentTravelDistance => _currentTravelDistance;

    private Action _onDiveEnd = null;

    public void StartHookDive(Vector3 startPointerPos, Action onDiveEnd)
    {
        if (_diveCoroutine != null) return;
        enabled = true;
        _hookTransorm.localPosition = Vector3.zero;
        _pointerPosition = startPointerPos;
        _lastPathPosRecord = _hookTransorm.position;
        _currentTravelDistance = 0;

        _onDiveEnd = onDiveEnd;

        _diveCoroutine = StartCoroutine(HookDiveCoroutine());

        _trailRenderer.enabled = true;
        _lineRenderer.enabled = false;
    }

    public void ResetRode()
    {
        _currentTravelDistance = 0;
        _hookTransorm.localPosition = Vector3.zero;
        _lastPathPosRecord = _hookTransorm.position;

        _linePathList.Clear();
        _lineRenderer.positionCount = 0;
        _trailRenderer.Clear();
    }

    private IEnumerator HookDiveCoroutine()
    {
        while (_currentTravelDistance < _maxTravelDistance)
        {
            MoveHook();
            RecordPath();
            if (DetectObstacle())
                break;

            yield return null;
        }
        EndHookDive();
        _diveCoroutine = null;
        // _lineRendere.enabled = false;
    }

    private void MoveHook()
    {
        Vector2 pointerDir = _pointerPosition - _hookTransorm.position;
        _movementDir = Vector2.Lerp(_movementDir, pointerDir, Time.deltaTime * _trackSpeed);
        _hookTransorm.Translate(_movementDir.normalized * _movementSpeed * Time.deltaTime, Space.World);
        _currentTravelDistance += Time.deltaTime * _movementSpeed;
    }

    private void EndHookDive()
    {
        print("End rode Dive");
        LinePathPoint lastPoint = new LinePathPoint(
             _lastPathPosRecord,
            _hookTransorm.position,
            _linePathList[_linePathList.Count - 1].endTime,
            _currentTravelDistance / _maxTravelDistance,
            Vector2.Distance(_lastPathPosRecord, _hookTransorm.position)
        );
        _linePathList.Add(lastPoint);

        _trailRenderer.enabled = false;
        _lineRenderer.enabled = true;
        UpdateLineRendere(_lineRenderer, _linePathList);
        // _lineRenderer.positionCount++;
        // _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, lastPoint.endPos);

        _onDiveEnd.Invoke();
    }

    private void RecordPath()
    {
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

    private bool DetectObstacle()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(_hookTransorm.position, _hookDetectionRadius, _hookDetectionLayer);
        // print("Detecte Obstacle : " + (cols.Length > 0));
        return cols.Length > 0;
    }

    public Vector2 GetPositionOnPath(float time)
    {
        float relativeTime = Mathf.Lerp(0, _linePathList[_linePathList.Count - 1].endTime, time);
        // print("relativeTime = " + relativeTime);
        if (relativeTime >= _linePathList[_linePathList.Count - 1].endTime) return _linePathList[_linePathList.Count - 1].endPos;
        if (relativeTime <= 0) return _linePathList[0].startPos;

        for (int i = 0; i < _linePathList.Count; i++)
        {
            if (relativeTime > _linePathList[i].startTime && relativeTime < _linePathList[i].endTime)
            {
                float pointTime = Mathf.InverseLerp(_linePathList[i].startTime, _linePathList[i].endTime, relativeTime);
                return Vector2.Lerp(_linePathList[i].startPos, _linePathList[i].endPos, pointTime);
            }
        }
        return Vector2.zero;
    }

    private void UpdateLineRendere(LineRenderer lineRenderer, List<LinePathPoint> path)
    {
        lineRenderer.positionCount = path.Count;
        // lineRenderer.SetPosition(0, transform.position);
        for (int i = 0; i < path.Count; i++)
        {
            lineRenderer.SetPosition(i, path[i].endPos);
        }
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

        Gizmos.color = new Color(1, 0, 0, .5f);
        for (int i = 0; i < _linePathList.Count; i++)
        {
            Gizmos.DrawSphere(_linePathList[i].startPos, _pathResolution / 2);
            Gizmos.DrawLine(_linePathList[i].startPos, _linePathList[i].endPos);
        }
    }
}
