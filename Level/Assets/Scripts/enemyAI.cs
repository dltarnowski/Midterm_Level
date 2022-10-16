using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyAI : MonoBehaviour, IDamage
{
    [Header("----- Componenets -----")]
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Renderer model;
    [SerializeField] CannonController cannonCtrl;
    [SerializeField] Collider col;
    [SerializeField] Animator anim;

    [Header("----- Enemy Stats -----")]
    [SerializeField] int HP;
    [SerializeField] int speedChase;
    [SerializeField] int facePlayerSpeed;
    [SerializeField] int animLerpSpeed;
    [SerializeField] int sightDist;
    [SerializeField] int viewAngle;
    [SerializeField] float damagedDuration;
    [SerializeField] GameObject headPos;
    [SerializeField] int roamDist;

    [Header("----- Weapon Stats -----")]
    [SerializeField] internal float attackRate;
    [SerializeField] internal GameObject attackPos;
    [SerializeField] GameObject weapon;
    [SerializeField] GameObject bullet;


    public bool stationary;
    public bool noRotation;
    bool isShooting;
    bool playerInRange;
    Color modelColor;
    Vector3 playerDir;
    float stoppingDistanceOrig;
    Vector3 startingPos;
    float angle;
    float speedPatrol;


    void Start()
    {
        modelColor = model.material.color;
        stoppingDistanceOrig = agent.stoppingDistance;
        startingPos = transform.position;
        speedPatrol = agent.speed;
        roam();
    }

    // Update is called once per frame
    void Update()
    {
        if(!anim.GetBool("Dead"))
        {
            anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), agent.velocity.normalized.magnitude, Time.deltaTime * animLerpSpeed));

            if (agent.enabled)
            {
                if (playerInRange)
                {
                    playerDir = gameManager.instance.player.transform.position - headPos.transform.position;
                    angle = Vector3.Angle(playerDir, transform.forward);
                    if(CompareTag("Ranged"))
                        canSeePlayer(shoot(), isShooting);
                }
                if (agent.remainingDistance < 0.1f && agent.destination != gameManager.instance.player.transform.position)
                    roam();
            }
        }
    }

    void roam()
    {
        agent.stoppingDistance = 0;
        agent.speed = speedPatrol;
        Vector3 randomDirection = Random.insideUnitSphere * roamDist;
        randomDirection += startingPos;

        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, 1, 1);
        NavMeshPath path = new NavMeshPath();

        agent.CalculatePath(hit.position, path);
        agent.SetPath(path);
    }

    void canSeePlayer(IEnumerator attack, bool isAttacking)
    {
        RaycastHit hit;
        if (Physics.Raycast(headPos.transform.position, playerDir, out hit, sightDist))
        {
            Debug.DrawRay(headPos.transform.position, playerDir);
            if (hit.collider.CompareTag("Player") && angle <= viewAngle)
            {
                agent.speed = speedChase;
                agent.stoppingDistance = stoppingDistanceOrig;
                agent.SetDestination(gameManager.instance.player.transform.position);
                if (agent.remainingDistance < agent.stoppingDistance)
                    facePlayer();

                if (!isAttacking)
                    StartCoroutine(attack);
            }
        }
    }

    void facePlayer()
    {
        playerDir.y = 0;
        Quaternion rotation = Quaternion.LookRotation(playerDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * facePlayerSpeed);
    }
    public void takeDamage(int dmg)
    {
        HP -= dmg;
        if (HP <= 0)
        {
            gameManager.instance.checkEnemyTotal();
            anim.SetBool("Dead", true);
            if (cannonCtrl != null)
            {
                cannonCtrl.enabled = true;
                gameObject.transform.DetachChildren();
            }
            col.enabled = false;
            agent.enabled = false;
            Destroy(gameObject, 5);
        }
        else if (HP > 0)
            StartCoroutine(flashDamage());

    }

    IEnumerator shoot()
    {
        isShooting = true;
        anim.SetTrigger("Shoot");
        Instantiate(bullet, attackPos.transform.position, transform.rotation);
        yield return new WaitForSeconds(attackRate);
        isShooting = false;
    }



    IEnumerator flashDamage()
    {
        model.material.color = Color.red;
        agent.enabled = false;
        yield return new WaitForSeconds(damagedDuration);
        model.material.color = modelColor;
        agent.enabled = true;
        agent.SetDestination(gameManager.instance.player.transform.position);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            agent.stoppingDistance = 0;
        }

    }
}
