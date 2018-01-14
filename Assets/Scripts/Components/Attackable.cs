using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attackable : MonoBehaviour {

    public AudioSource attackedSound;

    public virtual void Attacked()
    {
        attackedSound.Play();
    }

}
