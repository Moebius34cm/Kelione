using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttaqueHitBoxComp : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.gameObject.GetComponent<Attackable>() != null && !coll.isTrigger)
        {
            coll.gameObject.GetComponent<Attackable>().Attacked();
        }
    }
}
