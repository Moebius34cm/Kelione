using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteDepth : MonoBehaviour {

    public SpriteRenderer sprite;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        sprite.sortingOrder = -Mathf.RoundToInt(transform.position.y);
	}

    /*private void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.isTrigger && coll.gameObject.GetComponent<SpriteRenderer>() != null)
        {
            if (sprite.sortingOrder <= coll.gameObject.GetComponent<SpriteRenderer>().sortingOrder)
            {
                coll.gameObject.GetComponent<SpriteRenderer>().sortingOrder = sprite.sortingOrder - 1;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.isTrigger && coll.gameObject.GetComponent<SpriteRenderer>() != null)
        {
            if (sprite.sortingOrder >= coll.gameObject.GetComponent<SpriteRenderer>().sortingOrder)
            {
                Debug.Log("Exited");
                coll.gameObject.GetComponent<SpriteRenderer>().sortingOrder = sprite.sortingOrder + 1;
            }
        }
    }*/
}
