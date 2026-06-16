using System.Collections;
using UnityEngine;

public class FishingSequence : MonoBehaviour
{
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
        GameEvents.onDiveStart.Invoke();

        _cameraControler?.SetTarget(_fishingRode.HookTransform);

        bool isHookDiveEnd = false;
        _fishingRode.StartHookDive(transform.position, () => isHookDiveEnd = true);
        while (!isHookDiveEnd) yield return null;

        _cameraControler.SetOrtoSize(5);
        yield return new WaitForSeconds(1);
        _cameraControler?.SetTarget(_cameraTarget);

        float rewindTime = 1;
        while (rewindTime > 0)
        {
            Vector2 newPops = _fishingRode.GetPositionOnPath(rewindTime);
            // Debug.Log("Time : " + rewindTime + " / Pos : " + newPops);
            _cameraTarget.position = newPops;
            rewindTime -= Time.deltaTime * 5 / _fishingRode.MaxTravelDistance;
            yield return null;
        }

        _cameraControler.SetOrtoSize();
        GameEvents.onDiveEnd.Invoke();
    }
}


