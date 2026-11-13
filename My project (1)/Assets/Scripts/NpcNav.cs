using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class NpcNav : MonoBehaviour
{
    #region All Controls/References + OnStart
    private PlayerController player;
    private NavMeshPath navPath;
    private Rigidbody2D rigidbody;
    private CircleCollider2D DetectionRadiusCollider;

    [Header("Meta")]
    [SerializeField] private bool canSeePlayer;
    [SerializeField] private float stoppingDistance;
    [SerializeField] private float distanceFromStop;
    [SerializeField] private Transform followingEnemy;


    Vector3 steeringTarget;
    Vector3 runAwayPoint;
    Vector2 pathDir;
    Vector2 avoidDir;
    Vector2 finalMoveDir;
    public bool isFollowing;
    [SerializeField] private bool RunAwayFromEnemy = false;

    [Space]
    [Header("Changable Stats")]
    public float baseSpeed;
    public float speed;
    [SerializeField] private float PlayerStopDistance;
    [SerializeField] private float NormalStopDistance;
    [SerializeField] private float detectionRadius;

    [Space]
    [Header("NPC Avoidance and Pathing")]
    [SerializeField] private float avoidanceRadius = 1.5f;
    [SerializeField] private float avoidanceStrength = 2.5f;
    [SerializeField] private Vector3 destination;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private LayerMask NpcLayer;
    [SerializeField] private LayerMask enemyLayer;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        rigidbody = GetComponent<Rigidbody2D>();
        DetectionRadiusCollider = GetComponent<CircleCollider2D>();
        destination = transform.localToWorldMatrix * transform.localScale;
        navPath = new NavMeshPath();
        baseSpeed = speed;
    }
    #endregion

    //DO NOT CHANGE THE ORDER OF ANYTHING IN THE VOID UPDATE!!! IT WILL BREAK EVERYTHING AND TAKE FOREVER TO FIX!!!
    private void Update()
    {
        //Settings
        DetectionRadiusCollider.radius = detectionRadius;
        if (!isFollowing)
        {
            stoppingDistance = NormalStopDistance;
        }
        else
        {
            stoppingDistance = PlayerStopDistance;
        }
        Vector2 origin = transform.position;
        Vector2 direction = (Vector2)player.transform.position - origin;
        float distance = direction.magnitude;
        direction.Normalize();
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance, wallLayer);
        if (hit.collider != null)
        {
            canSeePlayer = false;
        }
        else
        {
            canSeePlayer = true;
        }

        //Picking Destination/Measuring Distance
        if (isFollowing)
        {
            destination = player.transform.position;
            destination.z = 0;
        }
        else
        {
            if (RunAwayFromEnemy)
            {
                GetRunAwayPoint();
            }
            else
            {
                rigidbody.linearVelocity = Vector3.zero;
            }
        }

        //Setting Up NavMesh and Steering
        distanceFromStop = Vector2.Distance(transform.position, destination);
        steeringTarget = destination;
        if (NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, navPath))
        {
            if (navPath.corners.Length > 1)
            {
                steeringTarget = navPath.corners[1];
            }
        }
        pathDir = ((Vector2)steeringTarget - (Vector2)transform.position).normalized;
        avoidDir = GetAvoidanceVector() * avoidanceStrength;
        finalMoveDir = (pathDir + avoidDir).normalized;

        //Movement and Rotation
        RotateTowardsDestination();
        MoveToDestination();
    }
    #region Rotation and Movement
  
    private void RotateTowardsDestination()
    {
        rigidbody.MoveRotation(Mathf.Atan2(finalMoveDir.y, finalMoveDir.x) * Mathf.Rad2Deg - 90f);
    }
    private void MoveToDestination()
    {
        if (distanceFromStop > stoppingDistance)
        {
            rigidbody.linearVelocity = speed * transform.up;
        }
        else if (distanceFromStop <= stoppingDistance)
        {
            rigidbody.linearVelocity = Vector2.zero;
        }

    }

    private void GetRunAwayPoint()
    {
            runAwayPoint = -(followingEnemy.transform.position - transform.position).normalized;
    }
    #endregion  
    
    #region AvoidanceVector and Triggers

    private Vector2 GetAvoidanceVector()
    {
        Collider2D[] nearbyNPCs = Physics2D.OverlapCircleAll(transform.position, avoidanceRadius, NpcLayer);
        Vector2 avoidanceForce = Vector2.zero;

        foreach (Collider2D other in nearbyNPCs)
        {
            // Skip ourselves
            if (other.gameObject == this.gameObject || other.isTrigger)
                continue;

            Vector2 awayFromOther = (Vector2)transform.position - (Vector2)other.transform.position;
            float distance = awayFromOther.magnitude;

            if (distance > 0.01f)
            {
                float strength = 1f - (distance / avoidanceRadius);
                Vector2 pushDir = awayFromOther.normalized * strength;

                Vector2 sideStep = new Vector2(-awayFromOther.y, awayFromOther.x).normalized * (strength * 0.35f);

                avoidanceForce += (pushDir + sideStep);
            }
        }

        return Vector2.ClampMagnitude(avoidanceForce, 1.2f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && canSeePlayer)
        {
            isFollowing = true;
        }
        if ((other.gameObject.CompareTag("MeleeEnemy") || other.gameObject.CompareTag("RangedEnemy")) && !isFollowing)
        {
            RunAwayFromEnemy = true;
            followingEnemy = other.transform;
        }
        if (other.gameObject.CompareTag("Projectile") && !isFollowing)
        {
            destination = other.transform.position;
            destination.z = 0;
            RunAwayFromEnemy = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if ((other.gameObject.CompareTag("MeleeEnemy") || other.gameObject.CompareTag("RangedEnemy")) && !isFollowing)
        {
            RunAwayFromEnemy = false;
            followingEnemy = null;
            RotateTowardsDestination();
            MoveToDestination();
        }
    }

    #endregion
    private void OnDrawGizmos()
    {
        if (navPath == null || navPath.corners == null || navPath.corners.Length == 0)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(destination, 0.4f);

        Gizmos.color = Color.cyan;
        if (navPath.corners.Length > 1)
        {
            Gizmos.DrawLine(transform.position, navPath.corners[1]);
        }

        for (int i = 0; i < navPath.corners.Length; i++)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(navPath.corners[i], 0.15f);

            if (i < navPath.corners.Length - 1)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawLine(navPath.corners[i], navPath.corners[i + 1]);
            }
        }
    }
}