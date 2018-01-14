using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlockHitBoxComp : MonoBehaviour {

    private void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.gameObject.GetComponent<Blockable>() != null)
        {
            coll.gameObject.GetComponent<Blockable>().Blocked();
        }
    }
}
