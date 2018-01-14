using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnComp : MonoBehaviour {

    public Transform respawnPoint;
    
    public void Respawn()
    {
        gameObject.transform.position = respawnPoint.position; 
    }
}
