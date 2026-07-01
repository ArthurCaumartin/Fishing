using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

// TODO ajouter des "Sequencable" ? pour gerer l'enchainement ?

public class FishingSequence : MonoBehaviour
{
    [SerializeField] private Transform _cameraTarget;
    [SerializeField] private FishingRode _fishingRode;
    [SerializeField] private FishGenerator _fishGenerator;
    [Space]
    [SerializeField] private GameEvent _eventDiveStart;
    [SerializeField] private GameEvent _eventDiveEnd;
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

        _cameraControler.SetTarget(_fishingRode.HookTransform, _cameraControler.DiveOffSet);

        bool isHookDiveEnd = false;
        _fishingRode.StartHookDive(transform.position, () => isHookDiveEnd = true);
        while (!isHookDiveEnd) yield return null;

        _cameraControler.SetOrtoSize(2.5f);
        _cameraControler.SetTarget(_fishingRode.HookTransform);
        yield return new WaitForSeconds(1);

        Fish newFish =  _fishGenerator.GenerateNewFish(_fishingRode.HookTransform.position, 10);  
        newFish.Hook(_fishingRode);

        yield return new WaitForSeconds(1);


        float rewindTime = 1;
        while (rewindTime > 0)
        {
            // print("Rewind : " + rewindTime);
            Vector2 newPos = _fishingRode.GetPositionOnPath(rewindTime);
            _fishingRode.SetHookPosition(newPos);
            // Debug.Log("Time : " + rewindTime + " / Pos : " + newPops);
            _cameraTarget.position = newPos;
            rewindTime -= Time.deltaTime * 5 / _fishingRode.CurrentTravelDistance;
            yield return null;
        }

        _fishingRode.ResetRode();
        _cameraControler.SetOrtoSize();

        _eventDiveEnd.Raise();
        _diveCoroutine = null;
    }
}