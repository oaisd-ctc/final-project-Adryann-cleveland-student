using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent ai;
    public List<Transform> destinations;
    public Animator aiAnim;
    public float walkSpeed, chaseSpeed, minIdleTime, maxIdleTime, idleTime, sightDistance, catchDistance, chaseTime, minChaseTime, maxChaseTime, jumpscareTime;
    public bool walking, chasing;
    public Transform player;
    Transform currentDest;
    Vector3 dest;
    int randNum;
    public int destinationAmount;
    public Vector3 rayCastOffset;
    public string deathScene;

    public float Damage;

    public HealthSystem playerHealth;

    SphereCollider cicle;
    void Start()
    {
       // playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthSystem>();
       // if(playerHealth == null)
       // {
         //   Debug.LogError("Player doesnt have Health");
//
       // }
        //else
       // {
         //   Debug.LogError("Player Object not foind");
       // }
        walking = true;
        randNum = Random.Range(0, destinations.Count);
        currentDest = destinations[randNum];
        aiAnim = GetComponent<Animator>();
        cicle = GetComponentInChildren<SphereCollider>();
    }
    void Update()
    {

        
        Vector3 direction = (player.position - transform.position).normalized;
        RaycastHit hit;
        if (Physics.Raycast(transform.position + rayCastOffset, direction, out hit, sightDistance))
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                walking = false;
                StopCoroutine("stayIdle");
                StopCoroutine("chaseRoutine");
                StartCoroutine("chaseRoutine");
                chasing = true;
            }
        }
        if (chasing == true)
        {
            dest = player.position;
            ai.destination = dest;
            ai.speed = chaseSpeed;
            aiAnim.ResetTrigger("walk");
            aiAnim.ResetTrigger("Idle");
           aiAnim.SetTrigger("walk");
            float distance = Vector3.Distance(player.position, ai.transform.position);
            if (distance <= catchDistance)
            {
                //player.gameObject.SetActive(false);
                Attack();
                
               aiAnim.ResetTrigger("walk");
               aiAnim.ResetTrigger("Idle");
                aiAnim.ResetTrigger("walk");
            
                
                
                walking = false;
            }
        }
        if (walking == true)
        {
            dest = currentDest.position;
            ai.destination = dest;
            ai.speed = walkSpeed;
           // aiAnim.ResetTrigger("sprint");
           aiAnim.ResetTrigger("Idle");
            aiAnim.SetTrigger("walk");
            if (ai.remainingDistance <= ai.stoppingDistance)
            {
                aiAnim.ResetTrigger("walk");
                aiAnim.ResetTrigger("walk");
                aiAnim.SetTrigger("Idle");
                ai.speed = 0;
                StopCoroutine("stayIdle");
                StartCoroutine("stayIdle");
                
                walking = false;
            }
        }
    }
    IEnumerator stayIdle()
    {
        idleTime = Random.Range(minIdleTime, maxIdleTime);
        yield return new WaitForSeconds(idleTime);
        walking = true;
        randNum = Random.Range(0, destinations.Count);
        currentDest = destinations[randNum];
    }
    IEnumerator chaseRoutine()
    {
        chaseTime = Random.Range(minChaseTime, maxChaseTime);
        yield return new WaitForSeconds(chaseTime);
        walking = true;
        chasing = false;
        randNum = Random.Range(0, destinations.Count);
        currentDest = destinations[randNum];
    }
   void Attack()
    {
        if (!aiAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack1"))
         {
            aiAnim.SetTrigger("Attack1");
            ai.SetDestination(transform.position);
            HealthSystem.Instance.TakeDamage(Damage);
        }
      
    }
  void enableAttack()
    {
        cicle.enabled = true;
    }

    void disableAttack()
    {
        cicle.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<PlayerMovement>();
        if(player != null)
        {
          
            print("HIT");
        }
    }
}