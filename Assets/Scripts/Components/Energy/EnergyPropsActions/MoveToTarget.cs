using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToTarget : EnergyProp {

    public Transform target;
    public float speed;
    private bool doAction;
    private Vector3 originalPosition;

    private void Start()
    {
        originalPosition = transform.position;
    }

    private void Update()
    {
        if(doAction)
        {
            transform.position = Vector3.Lerp(transform.position, target.position, speed * Time.fixedDeltaTime);
        }
        if(!doAction && transform.position != originalPosition)
        {
            transform.position = Vector3.Lerp(transform.position, originalPosition, speed * Time.fixedDeltaTime);
        }
    }

    public override void TriggerEnergy()
    {
        doAction = true;
    }

    public void UndoAction()
    {
        doAction = false; 
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(3.5f);
        UndoAction();
    }
	
}
