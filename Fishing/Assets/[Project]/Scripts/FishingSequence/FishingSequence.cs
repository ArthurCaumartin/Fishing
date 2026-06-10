using UnityEngine;

public class FishingSequence : MonoBehaviour
{
    [SerializeField] private FishingRode _fishingRode;
    [Space]
    [SerializeField] private float _startDelay = 3;



    public void StartFishSequence()
    {
        //TODO call UI manager
        _fishingRode.EnableHookFollow(transform.position);
    }

}


