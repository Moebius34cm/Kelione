using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerActionsObject : ScriptableObject
{
    [Header("Vitesses")]
    public float normalSpeed = 300f;
    public float dashSpeed = 600f;

    [Header ("Contrôles")]
    public string LeftJoyHori = "LeftJoyHori";
    public string LeftJoyVerti = "LeftJoyVerti";
    public string AButton = "AButton";
    public string BButton = "BButton";
    public string XButton = "XButton";
    public string YButton = "YButton";

    [Header("Paramètres des actions")]
    public float dashPrecision = 1.5f;
    public float dashingTime = 0.5f;
    public float dashDelay = 0.5f;
    public float actionDelay = 0.6f;
    public float attaqueDelay = 0.15f;
    public float paradeDelay = 0.15f;
    public float cloneLifeDelay = 2f;
    public float knockedDownTime = 2f;

    [Header("Sons")]
    public AudioClip attackSound;
    public AudioClip dashSound;

    [HideInInspector] public Transform pivotTransform;
    [HideInInspector] public Rigidbody2D PlayerBody;
    [HideInInspector] public GameObject playerAttaqueBox;
    [HideInInspector] public GameObject playerParedeBox;
    [HideInInspector] public GameObject playerCloneBox;

}
