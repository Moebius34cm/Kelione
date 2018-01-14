using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent (typeof(Rigidbody2D))]
public class PlayerComponent : MonoBehaviour {

    public PlayerActionsObject playerActionsObject;
    public Rigidbody2D body;
    public Animator anim;
    public LayerMask layerMask;
    public GameObject pivot;
    public GameObject attaque;
    public GameObject parade;
    public bool hasCloneCapacity = false;
    public GameObject cloneFeinte;
    public Transform cloneFeinteSpawn;
    public GameEvent dashEvent;
    public GameEvent attackEvent;
    public GameEvent blockEvent;
    public GameEvent cloneEvent;
    public GameEvent playerKnockedDown;
    public GameEvent playerDead;

    [Header("Sons")]
    public AudioSource attackSound;
    public AudioSource dashSound;

    private Vector3 dashAim;
    private float usedSpeed;
    private bool canDash = true;
    private bool dashing = false;
    private bool canAction = true;
    private bool isKnockedDown = false;
    private Vector2 lastDir;
    private Coroutine feinte;
    private Coroutine knockDownCoroutine;

    // Use this for initialization
    void Start () {
        playerActionsObject.pivotTransform = pivot.transform;
        usedSpeed = playerActionsObject.normalSpeed;
        attackSound.clip = playerActionsObject.attackSound;
        dashSound.clip = playerActionsObject.dashSound;
	}
	
	// Update is called once per frame
	void Update () {
        Movement();
        Actions();
        EnableAnimations();
        Debug.DrawLine(pivot.transform.position, new Vector2(transform.position.x + lastDir.x,transform.position.y + lastDir.y), Color.magenta);
    }

    void Movement()
    {
        float walkX = Input.GetAxisRaw(playerActionsObject.LeftJoyHori);
        float walkY = Input.GetAxisRaw(playerActionsObject.LeftJoyVerti);
        float rot_z = Mathf.Atan2(walkY, walkX) * Mathf.Rad2Deg;
        rot_z -= 90;
        if (Mathf.Abs(walkX + walkY) > 0.3f && canAction)
        {
            pivot.transform.rotation = Quaternion.Euler(0f, 0f, rot_z);
            lastDir = new Vector2(walkX, walkY);
        }
        if (dashing)
        {
            dashAim = new Vector2(walkX, walkY).normalized * usedSpeed * Time.fixedDeltaTime;
            body.velocity = Vector2.Lerp(body.velocity, dashAim, playerActionsObject.dashPrecision);
            return;
        }
        else
        {
            body.velocity = new Vector2(walkX, walkY).normalized * usedSpeed * Time.fixedDeltaTime;
        }
    }

    void Actions()
    {
        if(Input.GetButtonDown(playerActionsObject.AButton) && canDash && !isKnockedDown)
        {
            StartCoroutine(Dashing());
        }

        if (Input.GetButtonDown(playerActionsObject.XButton) && canAction)
        {
            StartCoroutine(Attacking());
        }

        if (Input.GetButtonDown(playerActionsObject.BButton) && canAction)
        {
            StartCoroutine(Blocking());
        }

        if (Input.GetButtonDown(playerActionsObject.YButton) && canAction && hasCloneCapacity)
        {
            if(feinte == null)
                feinte = StartCoroutine(Cloning());
            else
            {
                StopCoroutine(feinte);
                feinte = StartCoroutine(Cloning());
            }   
        }
    }

    void EnableAnimations()
    {
        anim.SetBool("knockdown", isKnockedDown);
    }

    IEnumerator Dashing()
    {
        canDash = false;
        dashing = true;
        usedSpeed = playerActionsObject.dashSpeed;
        dashSound.Play();
        dashEvent.Raise();
        yield return new WaitForSeconds(playerActionsObject.dashingTime);
        usedSpeed = playerActionsObject.normalSpeed;
        dashing = false;
        yield return new WaitForSeconds(playerActionsObject.dashDelay);
        canDash = true;
    }

    IEnumerator Attacking()
    {
        parade.SetActive(false);
        attaque.SetActive(true);
        attackSound.Play();
        canAction = false;
        body.velocity = Vector2.zero;
        attackEvent.Raise();
        yield return new WaitForSeconds(playerActionsObject.attaqueDelay);
        attaque.SetActive(false);
        yield return new WaitForSeconds(playerActionsObject.actionDelay);
        canAction = true;
    }

    IEnumerator Blocking()
    {
        RotateToNearestEnemy();
        parade.SetActive(true);
        attaque.SetActive(false);
        canAction = false;
        body.velocity = Vector2.zero;
        blockEvent.Raise();
        yield return new WaitForSeconds(playerActionsObject.paradeDelay);
        parade.SetActive(false);
        yield return new WaitForSeconds(playerActionsObject.actionDelay);
        canAction = true;
    }

    IEnumerator Cloning()
    {
        canAction = false;
        cloneFeinte.transform.position = cloneFeinteSpawn.position;
        cloneFeinte.SetActive(true);
        playerActionsObject.pivotTransform = cloneFeinte.transform;
        body.velocity = Vector2.zero;
        cloneFeinte.GetComponent<Rigidbody2D>().velocity = lastDir.normalized * playerActionsObject.dashSpeed * Time.fixedDeltaTime;
        cloneEvent.Raise();
        yield return new WaitForSeconds(playerActionsObject.dashingTime * 0.7f);
        cloneFeinte.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        yield return new WaitForSeconds(playerActionsObject.actionDelay / 2);
        canAction = true;
        yield return new WaitForSeconds(playerActionsObject.cloneLifeDelay);
        cloneFeinte.SetActive(false);
        playerActionsObject.pivotTransform = transform;
    }

    public void KnowckDown()
    {
        if (parade.activeSelf)
            return;
        if (!isKnockedDown && knockDownCoroutine == null)
        {
            Debug.Log("knockeddown");
            StopAllCoroutines();
            isKnockedDown = true;
            usedSpeed = playerActionsObject.normalSpeed / 2;
            cloneFeinte.SetActive(false);
            attaque.SetActive(false);
            parade.SetActive(false);
            knockDownCoroutine = StartCoroutine(KnockedDownDelay());
        }
        else
        {
            Died();
        }
            
    }

    IEnumerator KnockedDownDelay()
    {
        yield return new WaitForSeconds(playerActionsObject.knockedDownTime);
        canDash = true;
        canAction = true;
        isKnockedDown = false;
        usedSpeed = playerActionsObject.normalSpeed;
        knockDownCoroutine = null;
    }

    public void GetUp()
    {
        canDash = true;
        canAction = true;
        isKnockedDown = false;
        usedSpeed = playerActionsObject.normalSpeed;
    }

    public void Died()
    {
        Debug.Log("died() called");
        playerDead.Raise();
        canDash = true;
        canAction = true;
        usedSpeed = playerActionsObject.normalSpeed;
    }


    public void RotateToNearestEnemy()
    {
        Vector3 nearestEnemy = new Vector3(0, 0, 0);
        Collider2D[] results = Physics2D.OverlapCircleAll(transform.position, 1f, layerMask);
        if (results.Length == 0)
            return;

        for (int i = results.Length - 1; i >= 0; i--)
        {
            if (nearestEnemy == Vector3.zero)
            {
                nearestEnemy = results[i].transform.position;
            }

            if (Vector3.Distance(transform.position, nearestEnemy) > Vector3.Distance(transform.position, results[i].transform.position))
            {
                nearestEnemy = results[i].transform.position;
            }
        }

        Vector3 dirToNearest = nearestEnemy - transform.position;
        float rot_z = Mathf.Atan2(dirToNearest.y, dirToNearest.x) * Mathf.Rad2Deg;
        rot_z -= 90;
        pivot.transform.rotation = Quaternion.Euler(0, 0, rot_z);
    }
}
