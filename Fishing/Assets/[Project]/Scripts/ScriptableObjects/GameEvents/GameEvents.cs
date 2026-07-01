
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameEvent_", menuName = "MLG/GameEvent")]
public class GameEvent : ScriptableObject
{
    [SerializeField] private List<GameEventListener> _eventListener;

    public void Raise()
    {
        foreach (var item in _eventListener)
            item?.InvokeEvent();
    }

    public void AddListener(GameEventListener listener)
    {
        if (!_eventListener.Contains(listener)) _eventListener.Add(listener);
    }

    public void RemoveListener(GameEventListener listener)
    {
        if (_eventListener.Contains(listener)) _eventListener.Remove(listener);
    }
}

