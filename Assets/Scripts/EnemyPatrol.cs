using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.AI;
using UnityEngine.AI;

public class EnemyPatrol : MonoBehaviour
{

    GameObject player;

    NavMeshAgent agent;

    [SerializeField] LayerMask groundLayer, playerLayer;

    //patroling
    Vector3 destPoint;
    bool walkPointSet;
    [SerializeField] float WalkRange;

    // Start is called before the first frame update
    void Start()
    {
     agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Patrol();
    }

    void Patrol()
    {
        if (!walkPointSet) SearchForDestination() ;
        if(walkPointSet) agent.SetDestination(destPoint);
        if (Vector3.Distance(transform.position, destPoint) < 10) walkPointSet = false;

       
    }

    void SearchForDestination()
    {
        float Z = Random.Range(-WalkRange, WalkRange);
        float X = Random.Range(-WalkRange, WalkRange);

        destPoint = new Vector3(transform.position.x + X, transform.position.y, transform.position.z + Z);

        if(Physics.Raycast(destPoint, Vector3.down, groundLayer))
        {
            walkPointSet = true;
        }
    }
}
