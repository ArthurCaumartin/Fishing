using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public enum DiveState
{
    WaitForStart,
    Diving,
    FishSelection,
    Rewind
}

public class DiveControler : MonoBehaviour
{
    [SerializeField] private FishingRode _rode;
    [SerializeField] private Transform _cameraTarget;
    [Space]
    [SerializeField] private float _diveSpeed = 2;
    [SerializeField] private Transform _startPoint;
    [SerializeField] private Transform _endPoint;
    private float _diveTravelTime;
    private float _diveDepth;

    private CameraControler _cameraControler;
    private DiveState _diveState;
    private Vector2 _worldPointerPosition;

    public void InitDive(CameraControler controler)
    {
        _cameraControler = controler;
        _cameraControler.SetTarget(_cameraTarget);
        _diveDepth = Vector3.Distance(_startPoint.position, _endPoint.position);

        _diveState = DiveState.WaitForStart;
        _rode.OnDropEnd.AddListener(StartFishSelection);
    }

    private void Update()
    {
        if (_diveState == DiveState.Diving)
        {
            if (_diveTravelTime >= 1)
            {
                _diveState = DiveState.FishSelection;
            }

            print("hook dist travel time = " + _rode.DistanceTravelTime);

            _diveTravelTime += Time.deltaTime * _diveSpeed / _diveDepth;
            Vector3 divePosition = Vector3.Lerp(_startPoint.position, _endPoint.position, _diveTravelTime);
            _cameraTarget.position = divePosition;
        }
    }

    public void StartFishSelection()
    {
        _cameraControler.SetTarget(_rode.HookTransform);
        _diveState = DiveState.FishSelection;


    }

    private void OnMovePointer(InputValue value)
    {
        Vector2 screenPos = value.Get<Vector2>();
        _worldPointerPosition = Camera.main.ScreenToWorldPoint(screenPos);
    }

    private void OnClicPointer(InputValue value)
    {
        if (_diveState == DiveState.WaitForStart)
        {
            if (_worldPointerPosition.y < transform.position.y)
            {
                _diveState = DiveState.Diving;
                _rode.EnableHookFollow(_worldPointerPosition);
            }
        }
    }
}