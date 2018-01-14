using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviour : Attackable {

    public enum Phases { one, two, three}
    public Phases nombreDePhase; 
    public EnemyBehaviourComponent phaseUn;
    public EnemyBehaviourComponent phaseDeux;
    public EnemyBehaviourComponent phaseTrois;
    public GameEvent touched;
    public PointEffector2D waveRange;

    public EnemyAttaqueHitBoxComp attaqueHitBox;
    public EnemyBehaviourComponent usedEnemyBehaviour;
    private void Start()
    {
        usedEnemyBehaviour = phaseUn;
        attaqueHitBox.attachedEnemy = usedEnemyBehaviour;
        phaseUn.enabled = true;
        if (phaseDeux != null)
            phaseDeux.enabled = false;
        if (phaseTrois != null)
            phaseTrois.enabled = false;
    }

    public override void Attacked()
    {
        if (phaseUn.enabled && phaseUn.canBlock)
            return;
        touched.Raise();
        if(phaseDeux != null)
        {
            if(phaseUn.enabled && !phaseDeux.enabled)
            {
                StartCoroutine(Repulse(false, true, false));
                usedEnemyBehaviour = phaseDeux;
                attaqueHitBox.attachedEnemy = usedEnemyBehaviour;

                return;
            }
            if (phaseDeux.enabled && phaseDeux.canBlock)
                return;

            if (phaseTrois != null)
            {
                if (phaseDeux.enabled && !phaseTrois.enabled)
                {
                    StartCoroutine(Repulse(false, false, true));
                    usedEnemyBehaviour = phaseTrois;
                    attaqueHitBox.attachedEnemy = usedEnemyBehaviour;
                    return;
                }
                else if (phaseTrois.enabled)
                {
                    if (phaseTrois.canBlock)
                        return;
                    gameObject.SetActive(false);
                }
            }
            else { gameObject.SetActive(false); }
        }
        else { gameObject.SetActive(false); }
        
    }

    IEnumerator Repulse(bool one, bool two, bool three)
    {
        phaseUn.enabled = false;
        if(phaseDeux != null) { phaseDeux.enabled = false; }
        if(phaseTrois != null) { phaseTrois.enabled = false; }
        waveRange.enabled = true;
        yield return new WaitForSeconds(1.5f);
        waveRange.enabled = false;
        phaseUn.enabled = one;
        if(phaseDeux != null) { phaseDeux.enabled = two; }
        if(phaseTrois != null) { phaseTrois.enabled = three; }
        yield break;
    }

    public void AddMoveTowardUtility(int amount)
    {
        usedEnemyBehaviour.AddMoveTowardUtility(amount);
    }

    public void AddFleeUtility(int amount)
    {
        usedEnemyBehaviour.AddFleeUtility(amount);
    }

    public void AddAttackUtility(int amount)
    {
        usedEnemyBehaviour.AddAttackUtility(amount);
    }

    public void AddWaitUtility(int amount)
    {
        usedEnemyBehaviour.AddWaitUtility(amount);
    }

    public void LaunchResetFight()
    {
        StartCoroutine(usedEnemyBehaviour.ResetFight());
    }
}
