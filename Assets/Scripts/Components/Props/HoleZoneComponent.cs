using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleZoneComponent : MonoBehaviour {

    public Collider2D boundCollider;
    public Collider2D deadZoneCollider;
    private Coroutine hollow;

    public void Hollow()
    {
        boundCollider.isTrigger = true;
        deadZoneCollider.enabled = false;
        if (hollow == null)
            hollow = StartCoroutine(Delay());
    }

	IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.4f);
        boundCollider.isTrigger = false;
        deadZoneCollider.enabled = true;
        hollow = null;
        yield break;
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.gameObject.GetComponent<PlayerComponent>() != null)
        {
            coll.gameObject.GetComponent<PlayerComponent>().Died();
        }
    }
}
