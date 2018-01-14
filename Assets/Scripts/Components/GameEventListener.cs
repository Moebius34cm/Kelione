using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class GameEventListener : MonoBehaviour {

    public GameEvent gameEvent;
    public UnityEvent response;

    public void OnEvenRaised()
    {
        response.Invoke();
    }

    private void OnEnable()
    {
        gameEvent.RegisterListener(this);
    }
    private void OnDisable()
    {
        gameEvent.UnRegisterListener(this);
    }

}
