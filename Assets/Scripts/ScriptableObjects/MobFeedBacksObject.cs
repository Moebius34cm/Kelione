using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MobFeedBacksObject : ScriptableObject {

    [Header("Visuels du mob")]
    public Color color;
    public RuntimeAnimatorController animator;
    [Header("Sons du mob")]
    public AudioClip attackClip;
    public AudioClip dashClip;
    public AudioClip hurtClip;
}
