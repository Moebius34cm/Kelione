using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyTriggerComponent : Attackable {

    private List<EnergyProp> props = new List<EnergyProp>();

    public override void Attacked()
    {
        attackedSound.Play();
        for(int i = props.Count - 1; i >= 0; i--)
        {
            props[i].TriggerEnergy();
        }
    }

    public void RegisterProp(EnergyProp prop)
    {
        props.Add(prop);
    }

    public void UnRegisterProp(EnergyProp prop)
    {
        props.Remove(prop);
    }
}
