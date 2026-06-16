using UnityEngine;

public class FishingSequence : MonoBehaviour
{
    [SerializeField] private FishingRode _fishingRode;
    [Space]
    [SerializeField] private float _startDelay = 3;
    private CameraControler _cameraControler;

    private void Start()
    {
        _cameraControler = Camera.main.GetComponent<CameraControler>();
    }


    public void StartFishSequence()
    {
        //TODO call UI manager
        //use timeline for delay and animation
        _fishingRode.EnableHookFollow(transform.position);
        _cameraControler?.SetTarget(_fishingRode.HookTransform);
    }

}


