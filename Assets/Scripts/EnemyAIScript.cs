using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIScript : MonoBehaviour
{
    public NavMeshAgent navMesh;
    public float startWaitTime = 4;
    public float timeToRotate = 2;
    public float speedwalk = 6;
    public float speedRUn = 9;

    public float viewRadius = 15;
    public float viewAngle = 90;
    public LayerMask playerMask;
    public LayerMask obstacleMask;
    public float meshResolution = 1f;
    public int edgeIterations = 4;
    public float edgeDistance = 0.5f;

    public Transform[] wayPoints;
    int m_CurrentWayPointIndex;

    Vector3 playerLastPosition = Vector3.zero;
    Vector3 m_PlayerPosition;

    float m_waitTime;
    float m_TimeToRotate;
    bool m_PlayerInRange;
    bool m_PlayerNear;
    bool m_isPatrol;
    bool m_CautPlayer;

    private void Start()
    {
        m_PlayerPosition = Vector3.zero;
        m_isPatrol = true;
        m_CautPlayer = false;
        m_PlayerInRange = false;
        m_waitTime = startWaitTime;
        m_TimeToRotate = timeToRotate;

        m_CurrentWayPointIndex = 0;
        navMesh = GetComponent<NavMeshAgent>();

        navMesh.isStopped = false;
        navMesh.speed = speedwalk;
        navMesh.SetDestination(wayPoints[m_CurrentWayPointIndex].position);
    }

    private void Update()
    {
        enviromentView();

        if(!m_isPatrol)
        {
            Chasing();
        }
        else
        {
            Patroling();
        }
    }

    private void Chasing()
    {
        m_PlayerNear = false;
        playerLastPosition = Vector3.zero;

        if(!m_CautPlayer)
        {
            Move(speedRUn);
            navMesh.SetDestination(m_PlayerPosition);
        }
        if(navMesh.remainingDistance <= navMesh.stoppingDistance)
        {
            if (m_waitTime <= 0 && !m_CautPlayer && Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("player").transform.position) >= 6f) {
                m_isPatrol = true;
                m_PlayerNear = false;
                Move(speedwalk);
                m_TimeToRotate = timeToRotate;
                m_waitTime = startWaitTime;
                navMesh.SetDestination(wayPoints[m_CurrentWayPointIndex].position);
            }
            else
            {
                if(Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("player").transform.position) >= 2.5f)
                {
                    Stop();
                    m_waitTime -= Time.deltaTime;
                }
            }
        }
    }
    private void Patroling()
    {
        if(m_PlayerNear)
        {
            if(m_TimeToRotate <=0)
            {
                Move(speedwalk);
                LookingPlayer(playerLastPosition);
            }
            else
            {
                Stop();
                m_TimeToRotate -= Time.deltaTime;
            }
        }
        else
        {
            m_PlayerNear = false;
            playerLastPosition = Vector3.zero;
            navMesh.SetDestination(wayPoints[m_CurrentWayPointIndex].position);
            if(navMesh.remainingDistance <= navMesh.stoppingDistance)
            {
                if(m_waitTime <=0)
                {
                    NextPoint();
                    Move(speedwalk);
                    m_waitTime = startWaitTime;
                }
                else
                {
                    Stop();
                    m_waitTime -= Time.deltaTime;
                }
            }
        }
    }
    public void NextPoint()
    {
        m_CurrentWayPointIndex = (m_CurrentWayPointIndex + 1) % wayPoints.Length;
        navMesh.SetDestination(wayPoints[m_CurrentWayPointIndex].position);
    }

    private void Move(float speed)
    {
        navMesh.isStopped = false;
        navMesh.speed = speed;
    }

    void Stop()
    {
        navMesh.isStopped = true;
        navMesh.speed = 0;
    }

    void CaughtPlayer()
    {
        m_CautPlayer = true;
    }

    void LookingPlayer(Vector3 player)
    {
        navMesh.SetDestination(player);
        if (Vector3.Distance(transform.position, player) <= 0.3)
        {
            if (m_waitTime <= 0)
            {
                m_PlayerNear = false;
                Move(speedwalk);
                navMesh.SetDestination(wayPoints[m_CurrentWayPointIndex].position);
                m_waitTime = startWaitTime;
                m_TimeToRotate = timeToRotate;
            }
            else
            {
                Stop();
                m_waitTime -= Time.deltaTime;
            }
        }
    }

    void enviromentView()
    {
        Collider[] playerInRange = Physics.OverlapSphere(transform.position, viewRadius, playerMask);
        for (int i = 0; i < playerInRange.Length; i++)
        {
            Transform player = playerInRange[i].transform;
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2)
            {
                float dstToPlayer = Vector3.Distance(transform.position, player.position);

                if (!Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleMask))
                {

                    m_PlayerInRange = true;
                    m_isPatrol = false;
                }
                else
                {
                    m_PlayerInRange = false;
                }
            }
            if (Vector3.Distance(transform.position, player.position) > viewRadius)
            {
                m_PlayerInRange = false;
            }

            if (m_PlayerInRange)
            {
                m_PlayerPosition = player.transform.position;
            }
        }
    }
}
