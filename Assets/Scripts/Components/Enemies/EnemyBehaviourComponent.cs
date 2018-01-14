using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]
public class EnemyBehaviourComponent : Attackable
{
    public MobFeedBacksObject feedbacks;
    public enum SideNumber { one, two, four };
    public SideNumber sideNumber;
    public MobBehaviourObject frontBehaviour;
    public MobBehaviourObject backBehaviour;
    public MobBehaviourObject sidesBehaviour;
    public Rigidbody2D body;
    public GameObject attaque;
    public Animator anim;

    private Transform target;
    private Vector2 dirToTarget;
    private float distToPlayer;
    private float rot_z;
    private MobBehaviourObject lastBehaviour;
    private bool inAction = false;
    private bool eliptRight = false;
    private bool attacking = false;
    private float numberOfWait = 0;
    private float waitUtility = 0;
    private float attackUtility = 0;
    private float fleeUtility = 0;
    private float moveTowardutility = 0;
    private PlayerActionsObject currentPlayer;
    [HideInInspector] public float scaleOnOne;
    [HideInInspector] public bool canBlock = true;

    private void Start()
    {
        anim.runtimeAnimatorController = feedbacks.animator;
    }

    private void Update()
    {
        if(target != null)
        {
            UpdateTarget();
            DetectOrientation();

            if (inAction == false)
            {
                ChooseAction();
                inAction = true;
            }
        }
        Animations();
    }

    public void Animations()
    {
        anim.SetBool("attacking", attacking);
        anim.SetFloat("speed", body.velocity.magnitude);
    }

    private void DetectOrientation()
    {
        dirToTarget = target.position - transform.position;
        rot_z = Mathf.Atan2(dirToTarget.y, dirToTarget.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot_z);
        distToPlayer = Vector2.Distance(transform.position, target.position);
        float distanceToPlayerDirectionPoint = Vector2.Distance(transform.position, (target.position + target.up));
        scaleOnOne = distanceToPlayerDirectionPoint - distToPlayer;
        if(sideNumber.GetHashCode() == 0)
        {
            lastBehaviour = frontBehaviour;
        }else if(sideNumber.GetHashCode() == 1)
        {
            lastBehaviour = TwoSides(scaleOnOne);
        }
        else
        {
            lastBehaviour = FourSides(scaleOnOne);
        }
    }

    private void ChooseAction()
    {
        //liste des situations pouvant être détectées directement par l'IA augmentant les utilités
        if (distToPlayer > lastBehaviour.likeDistanceToPlayer - 1 && distToPlayer < lastBehaviour.likeDistanceToPlayer + 1) // si dans la marge de distance
        {
            if(lastBehaviour.ranged)
            {
                attackUtility += 3;
            }
            waitUtility += 2;
            moveTowardutility += 1;
        }
        else if (distToPlayer > lastBehaviour.likeDistanceToPlayer - 1) // si trop loin
        {
            moveTowardutility += 3;
        }
        else if (distToPlayer < lastBehaviour.likeDistanceToPlayer + 1) // si prêt de la cible
        {
            if(lastBehaviour.ranged)
            {
                fleeUtility += 4;
            }
            if (!canBlock)
            {
                fleeUtility += 0.5f;
            }
            moveTowardutility += 1;
        }
        if(distToPlayer <= 2f && !lastBehaviour.ranged) // si attaque en melee et assez prêt pour attaquer
        {
            attackUtility += 3;
        }
        

        //Application des multiplicateurs
        attackUtility += numberOfWait;
        moveTowardutility += numberOfWait;
        waitUtility *= lastBehaviour.patience;
        attackUtility *= lastBehaviour.aggressitvity;
        fleeUtility *= lastBehaviour.fear;
        moveTowardutility *= lastBehaviour.aggressitvity;

        //on choisis l'utilité la plus haute et on lance l'action correspondante
        int chosenAction = 0;
        float higestValue = 0;
        float[] actionsUtility = new float[4];
        actionsUtility[0] = waitUtility;
        actionsUtility[1] = attackUtility;
        actionsUtility[2] = fleeUtility;
        actionsUtility[3] = moveTowardutility;
        for(int i = 0; i < actionsUtility.Length; i++)
        {
            if(actionsUtility[i] >= higestValue)
            {
                higestValue = actionsUtility[i];
                chosenAction = i;
            }
        }
        if (chosenAction == 0)
            StartCoroutine(Wait(lastBehaviour.canBlockDuringWait));
        else if (chosenAction == 1)
            StartCoroutine(Attack(lastBehaviour.canBlockDuringAttack));
        else if (chosenAction == 2)
            StartCoroutine(Flee(lastBehaviour.canBlockDuringFlee));
        else if (chosenAction == 3)
            StartCoroutine(MoveToward(lastBehaviour.canBlockDuringWalk));

        //Debug.Log("Wait utility : " + waitUtility + " | Attack utility : " + attackUtility + " | Move toward utility : " + moveTowardutility + " | Flee utility : " + fleeUtility);
        waitUtility = 1;
        attackUtility = 0;
        fleeUtility = 0;
        moveTowardutility = 0;
    }

    private MobBehaviourObject TwoSides(float scale)
    {
        if(scale > 0)
        {
            return backBehaviour;
        }
        else
        {
            return frontBehaviour;
        }
    }

    private MobBehaviourObject FourSides(float scale)
    {
        if (scale >= -1 && scale < -0.25f)
        {
            return frontBehaviour;
        }
        else if (scale <= 1 && scale > 0.75f)
        {
            return backBehaviour;
        }
        else
        {
            return sidesBehaviour;
        }
    }

    IEnumerator Wait(bool blocking)
    {
        canBlock = blocking;
        numberOfWait += 0.15f;
        body.velocity = (eliptRight) ? transform.up.normalized * lastBehaviour.walkSpeed * Time.fixedDeltaTime : transform.up.normalized * -lastBehaviour.walkSpeed * Time.fixedDeltaTime;
        yield return new WaitForSeconds(0.1f);
        inAction = false;
        yield break;
    }

    IEnumerator Attack(bool blocking)
    {
        canBlock = blocking;
        numberOfWait = 0;
        attacking = true;
        yield return new WaitForSeconds(0.1f);
        body.velocity = Vector2.zero;
        attaque.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        attaque.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        attacking = false;
        inAction = false;
        fleeUtility += 3;
        yield break;
    }

    IEnumerator MoveToward(bool blocking)
    {
        canBlock = blocking;
        numberOfWait = 0;
        if(lastBehaviour.canDash && distToPlayer < lastBehaviour.distanceToDash)
        {
            attacking = true;
            yield return new WaitForSeconds(0.2f);
            body.velocity = dirToTarget.normalized * lastBehaviour.dashSpeed * Time.fixedDeltaTime;
            yield return new WaitForSeconds(0.2f);
            inAction = false;
            yield break;
        }
        body.velocity = dirToTarget.normalized * lastBehaviour.walkSpeed * Time.fixedDeltaTime;
        yield return new WaitForSeconds(0.15f);
        inAction = false;
        yield break;
    }

    IEnumerator Flee(bool blocking)
    {
        canBlock = blocking;
        if (lastBehaviour.canDash)
        {
            body.velocity = dirToTarget.normalized * -lastBehaviour.dashSpeed * Time.fixedDeltaTime;
        }
        else
        {
            body.velocity = dirToTarget.normalized * (-lastBehaviour.walkSpeed * 1.5f) * Time.fixedDeltaTime;
        }
        yield return new WaitForSeconds(0.1f);
        body.velocity = Vector2.zero;
        inAction = false;
        yield break;
    }

    public void AddAttackUtility(int amount)
    {
        attackUtility += amount;
    }

    public void AddWaitUtility(int amount)
    {
        waitUtility += amount;
    }

    public void AddFleeUtility(int amount)
    {
        fleeUtility += amount;
    }

    public void AddMoveTowardUtility(int amount)
    {
        moveTowardutility += amount;
    }

    private void UpdateTarget()
    {
        target = currentPlayer.pivotTransform;
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.gameObject.GetComponent<PlayerComponent>() != null && target == null)
        {
            currentPlayer = coll.GetComponent<PlayerComponent>().playerActionsObject;
            target = currentPlayer.pivotTransform;
            StartCoroutine(ChangeEliptDirection());
        }
    }

    IEnumerator ChangeEliptDirection()
    {
        yield return new WaitForSeconds(Random.Range(1, 5));
        eliptRight = !eliptRight;
        StartCoroutine(ChangeEliptDirection());
    }

    public void LaunchStunDelay()
    {
        StopAllCoroutines();
        attaque.SetActive(false);
        StartCoroutine(StunDelay());
    }

    IEnumerator StunDelay()
    {
        inAction = true;
        attacking = false;
        body.velocity = dirToTarget.normalized * -400f * Time.fixedDeltaTime;
        yield return new WaitForSeconds(0.3f);
        body.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.7f);
        inAction = false;
        yield break;
    }

    public override void Attacked()
    {
        attackedSound.Play();
        if (canBlock)
            return;
        gameObject.SetActive(false);
    }

    public void LaunchResetFight()
    {
        StopAllCoroutines();
        attaque.SetActive(false);
        StartCoroutine(ResetFight());
        inAction = true;
    }

    public IEnumerator ResetFight()
    {
        Debug.Log(gameObject.name + " is reseting");
        if(distToPlayer > lastBehaviour.likeDistanceToPlayer - 1) // si trop loin
        {
            while(distToPlayer > lastBehaviour.likeDistanceToPlayer - 1)
            {
                body.velocity = dirToTarget.normalized * lastBehaviour.walkSpeed * Time.fixedDeltaTime;
                yield return new WaitForSeconds(0.15f);
            }
            inAction = false;
            Debug.Log(gameObject.name + " finished reseting and was too far");
            yield break;
        }else if(distToPlayer < lastBehaviour.likeDistanceToPlayer + 1) // si trop prêt
        {
            while (distToPlayer < lastBehaviour.likeDistanceToPlayer + 1)
            {
                body.velocity = dirToTarget.normalized * -lastBehaviour.walkSpeed * Time.fixedDeltaTime;
                yield return new WaitForSeconds(0.15f);
            }
            inAction = false;
            Debug.Log(gameObject.name + " finished reseting and was too close");
            yield break;
        }
        else if  (distToPlayer < lastBehaviour.likeDistanceToPlayer - 1 && distToPlayer > lastBehaviour.likeDistanceToPlayer + 1) // si dans la marge
        {
            yield return new WaitForSeconds(lastBehaviour.patience / 2);
            inAction = false;
            Debug.Log(gameObject.name + " finished reseting and was at good distance");
            yield break;
        }
    }

}
