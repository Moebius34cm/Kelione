using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttaqueHitBoxComp : Blockable {

    public GameEvent hitPlayer;
    public EnemyBehaviourComponent attachedEnemy;

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.GetComponent<PlayerComponent>() != null)
        {
            if (attachedEnemy.scaleOnOne <= -0.5f)
            {
                Debug.Log("player should be dead because the baby got rekt");
            }
            else
            {
                Debug.Log(attachedEnemy.gameObject.name + " touched");
                hitPlayer.Raise();
            }
        }
    }

    public override void Blocked()
    {
        attachedEnemy.LaunchStunDelay();
    }
}