using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnergyProp : MonoBehaviour {

    public EnergyTriggerComponent trigger;

    public virtual void TriggerEnergy()
    {

    }

    private void OnEnable()
    {
        trigger.RegisterProp(this);
    }

    private void OnDisable()
    {
        trigger.UnRegisterProp(this);
    }

}
