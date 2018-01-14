using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MobBehaviourObject : ScriptableObject {
    public bool canDash;
    public bool ranged;
    [Range(0, 500)]public float walkSpeed = 200f;
    [Range(550, 1000)]public float dashSpeed = 400f;
    [Range(3, 10)] public float distanceToDash = 6f;
    [Range(5, 12)] public float likeDistanceToPlayer = 8f;
    [Range(0, 6)] public float aggressitvity = 1f;
    [Range(0, 6)] public float patience = 1f;
    [Range(0, 6)] public float fear = 1f;
    public bool canBlockDuringWait = true;
    public bool canBlockDuringAttack = false;
    public bool canBlockDuringDash = false;
    public bool canBlockDuringFlee = false;
    public bool canBlockDuringWalk = true;
}
