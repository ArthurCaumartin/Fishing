using System;
using Alchemy.Inspector;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    [SerializeField] private GameEvent _gameEvent;
    /* [SerializeField] */ private UnityEvent _unityEvent = new UnityEvent();

    public void InvokeEvent()
    {
        _unityEvent.Invoke();
    }

    public void Sub(UnityAction action)
    {
        _unityEvent.AddListener(action);
    }

    private void OnEnable()
    {
        _gameEvent.AddListener(this);
    }

    private void OnDisable()
    {
        _gameEvent.RemoveListener(this);
    }
}

