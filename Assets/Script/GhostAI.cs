using UnityEngine;
using UnityEngine.AI;

public class GhostAI : MonoBehaviour
{
    [Header("Player")]
    private Transform player;

    [Header("Movement")]
    private float roamRadius = 15f; // 이동 거리
    private float roamTimer = 5f; // 이동 시간

    [Header("Speed")]
    private float roamSpeed = 2f;
    private float chaseSpeed = 5f;

    [Header("Vision")]
    private float viewDistance = 12f; // 시야거리
    [Range(0, 360)]
    private float viewAngle = 120f; // 시야각

    [Header("Layer Mask")]
    public LayerMask obstacleMask; 

    [Header("Search")]
    private float searchTime = 3f; // 플레이어 마지막 위치 조사 시간

    private NavMeshAgent agent;

    private float roamCounter;
    private float searchCounter;

    private bool isChasing;
    private Vector3 lastKnownPosition;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        player = GameObject.FindGameObjectWithTag("Player").transform;

        agent.speed = roamSpeed;

        roamCounter = roamTimer;

    }

    void Update()
    {
        if (CanSeePlayer())
        {
            ChasePlayer();
        }
        else
        {
            if (isChasing)
            {
                SearchPlayer();
            }
            else
            {
                Roam();
            }
        }
    }

    // ------------------------
    // 배회
    // ------------------------
    void Roam()
    {
        agent.speed = roamSpeed;

        roamCounter += Time.deltaTime;

        if (roamCounter >= roamTimer)
        {
            Vector3 newPos = RandomNavSphere(transform.position, roamRadius, -1);

            agent.SetDestination(newPos);

            roamCounter = 0;
        }
    }

    // ------------------------
    // 추적
    // ------------------------
    void ChasePlayer()
    {
        isChasing = true;

        searchCounter = 0;

        lastKnownPosition = player.position;

        agent.speed = chaseSpeed;

        agent.SetDestination(player.position);
    }

    // ------------------------
    // 플레이어 놓쳤을 때
    // 마지막 위치 조사
    // ------------------------
    void SearchPlayer()
    {
        agent.speed = roamSpeed;

        agent.SetDestination(lastKnownPosition);

        searchCounter += Time.deltaTime;

        if (searchCounter >= searchTime)
        {
            isChasing = false;
            roamCounter = roamTimer;
        }
    }

    // ------------------------
    // 시야 판정
    // ------------------------
    bool CanSeePlayer()
    {
        if (player == null)
            return false;

        Vector3 directionToPlayer =
            (player.position - transform.position).normalized;

        float distanceToPlayer =
            Vector3.Distance(transform.position, player.position);

        // 거리 체크
        if (distanceToPlayer < viewDistance)
        {
            // 각도 체크
            float angle =
                Vector3.Angle(transform.forward, directionToPlayer);

            if (angle < viewAngle / 2)
            {
                // 벽 체크
                if (!Physics.Raycast(
                    transform.position + Vector3.up,
                    directionToPlayer,
                    distanceToPlayer,
                    obstacleMask))
                {
                    return true;
                }
            }
        }

        return false;
    }

    // ------------------------
    // 랜덤 위치 찾기
    // ------------------------
    public static Vector3 RandomNavSphere(
        Vector3 origin,
        float distance,
        int layerMask)
    {
        Vector3 randomDirection =
            Random.insideUnitSphere * distance;

        randomDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(
            randomDirection,
            out navHit,
            distance,
            layerMask);

        return navHit.position;
    }

    // ------------------------
    // 시야 Gizmo
    // ------------------------
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, viewDistance);

        Vector3 leftBoundary =
            Quaternion.Euler(0, -viewAngle / 2, 0)
            * transform.forward;

        Vector3 rightBoundary =
            Quaternion.Euler(0, viewAngle / 2, 0)
            * transform.forward;

        Gizmos.color = Color.yellow;

        Gizmos.DrawLine(
            transform.position,
            transform.position + leftBoundary * viewDistance);

        Gizmos.DrawLine(
            transform.position,
            transform.position + rightBoundary * viewDistance);
    }
}