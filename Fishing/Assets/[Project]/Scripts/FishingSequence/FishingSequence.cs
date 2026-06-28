using System.Collections;
using UnityEngine;

public class FishingSequence : MonoBehaviour
{
    [SerializeField] private GameEvent _eventDiveStart;
    [SerializeField] private GameEvent _eventDiveEnd;
    [SerializeField] private FishingRode _fishingRode;
    [SerializeField] private Transform _cameraTarget;
    [Space]
    private CameraControler _cameraControler;
    private Coroutine _diveCoroutine = null;

    private void Start()
    {
        _cameraControler = Camera.main.GetComponent<CameraControler>();
    }

    public void StartFishSequence()
    {
        if (_diveCoroutine != null) return;
        _diveCoroutine = StartCoroutine(DiveSequence());
    }

    private IEnumerator DiveSequence()
    {
        _eventDiveStart.Raise();


        _cameraControler?.SetTarget(_fishingRode.HookTransform, _cameraControler.DiveOffSet);

        bool isHookDiveEnd = false;
        _fishingRode.StartHookDive(transform.position, () => isHookDiveEnd = true);
        while (!isHookDiveEnd) yield return null;

        _cameraControler.SetOrtoSize(5);
        yield return new WaitForSeconds(1);
        // _cameraControler?.SetTarget(_cameraTarget); //TODO change "camera target" simplement avec le hook

        float rewindTime = 1;
        while (rewindTime > 0)
        {
            // print("Rewind : " + rewindTime);
            Vector2 newPops = _fishingRode.GetPositionOnPath(rewindTime);
            // Debug.Log("Time : " + rewindTime + " / Pos : " + newPops);
            _cameraTarget.position = newPops;
            rewindTime -= Time.deltaTime * 5 / _fishingRode.CurrentTravelDistance;
            yield return null;
        }

        _fishingRode.ResetRode();
        _cameraControler.SetOrtoSize();

        _eventDiveEnd.Raise();
        _diveCoroutine = null;
    }
}


