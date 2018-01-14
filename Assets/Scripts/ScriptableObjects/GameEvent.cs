using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameEvent : ScriptableObject
{

    private List<GameEventListener> listeners = new List<GameEventListener>();

    public void Raise()
    {
        for(int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEvenRaised();
        }
    }

    public void RegisterListener(GameEventListener registListener)
    {
        listeners.Add(registListener);
    }

    public void UnRegisterListener(GameEventListener unRegistListener)
    {
        listeners.Remove(unRegistListener);
    }

}
