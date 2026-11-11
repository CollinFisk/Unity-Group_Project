using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(CircleCollider2D))]
public class EnemyNav : MonoBehaviour
{
    private PlayerController player;
    private NavMeshPath navPath;
    private Rigidbody2D rigidbody;
    private CircleCollider2D DetectionRadiusCollider;
    private float stoppingDistance;

    [Header("Meta")]
    [SerializeField] private bool canSeePlayer;
    [SerializeField] private bool patrolling;
    [SerializeField] private bool MovingToDestination;
    [SerializeField] private bool choosingPoint = false;
    [SerializeField] private float baseSpeed;
    public bool isFollowing;

    [Space]
    [Header("Changable Stats")]
    public float speed;
    [SerializeField] private float PlayerStopDistance;
    [SerializeField] private float PatrolStopDistance;
    [SerializeField] private float PatrolWaitTime;
    [SerializeField] private float detectionRadius;
    [SerializeField] private float runAwayDistance;

    [Space]
    [Header("Turning")]
    [SerializeField] private float turnSpeed;
    [SerializeField] private float targetAngle;
    [SerializeField] private float smoothAngle;

    [Space]
    [Header("Patrol Bounds")]
    [SerializeField] private float PatrolRadiusLowX;
    [SerializeField] private float PatrolRadiusLowY;
    [SerializeField] private float PatrolRadiusHighX;
    [SerializeField] private float PatrolRadiusHighY;

    [Space]
    [Header("Enemy Avoidance")]
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float avoidanceRadius = 1.5f;
    [SerializeField] private float avoidanceStrength = 2.5f;

    [Space]
    [Header("Pathing")]
    [SerializeField] private Vector3 destination;
    [SerializeField] private LayerMask wallLayer;


    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        rigidbody = transform.GetComponent<Rigidbody2D>();
        DetectionRadiusCollider = GetComponent<CircleCollider2D>();
        destination = transform.position;
        navPath = new NavMeshPath();
        baseSpeed = speed;
        StartPatrol();
    }
    void Update()
    {
        Vector2 origin = transform.position;
        Vector2 direction = (Vector2)player.transform.position - origin;
        float distance = direction.magnitude;
        direction.Normalize();
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance, wallLayer);

        float distanceFromStop = Vector2.Distance(transform.position, destination);

        DetectionRadiusCollider.radius = detectionRadius;


        if (hit.collider != null)
        {
            canSeePlayer = false;
            isFollowing = false;
        }
        else
        {
            canSeePlayer = true;
        }

        if (isFollowing)
        {
            destination = player.transform.position;
            destination.z = 0;
        }
        else if (patrolling && !MovingToDestination)
        {
            if (distanceFromStop <= stoppingDistance && choosingPoint)
            {
                int maxAttempts = 20;
                int attempts = 0;
                bool validPointFound = false;
                Vector3 proposedDestination = Vector3.zero;

                while (!validPointFound && attempts < maxAttempts)
                {
                    attempts++;
                    proposedDestination.x = Random.Range(PatrolRadiusLowX, PatrolRadiusHighX);
                    proposedDestination.y = Random.Range(PatrolRadiusLowY, PatrolRadiusHighY);
                    proposedDestination.z = 0;

                    float checkRadius = 0.5f;
                    Collider2D hitTile = Physics2D.OverlapCircle(proposedDestination, checkRadius, wallLayer);

                    if (hitTile == null)
                    {
                        validPointFound = true;
                    }
                }
                if (validPointFound)
                {
                    destination = proposedDestination;
                    Debug.Log($"Successfully found clear patrol point at {destination} after {attempts} attempts.");
                    choosingPoint = false;
                }
                else
                {
                    Debug.LogWarning("Could not find a valid patrol point after max attempts. Trying again next frame.");
                }
            }
        }

        Vector3 steeringTarget = destination;
        if (NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, navPath))
        {
            if (navPath.corners.Length > 1)
            {
                steeringTarget = navPath.corners[1];
            }
        }

        Vector2 pathDir = ((Vector2)steeringTarget - (Vector2)transform.position).normalized;
        Vector2 avoidDir = GetAvoidanceVector() * avoidanceStrength;
        Vector2 finalMoveDir = (pathDir + avoidDir).normalized;

        if (isFollowing)
        {
            if (IsInvoking(nameof(StartPatrol)))
            {
                CancelStartPatrol();
            }
            patrolling = false;

            rigidbody.MoveRotation(Mathf.Atan2(finalMoveDir.y, finalMoveDir.x) * Mathf.Rad2Deg - 90f);

            if (distanceFromStop > stoppingDistance)
            {
                rigidbody.linearVelocity = speed * transform.up;
            }
            else if (distanceFromStop <= stoppingDistance && distanceFromStop > runAwayDistance)
            {
                rigidbody.linearVelocity = Vector2.zero;
            }
            else if (distanceFromStop <= runAwayDistance)
            {
                rigidbody.linearVelocity = speed / 2 * -transform.up;
            }
        }
        else
        {
            if (MovingToDestination)
            {
                rigidbody.MoveRotation(Mathf.Atan2(finalMoveDir.y, finalMoveDir.x) * Mathf.Rad2Deg - 90f);
                rigidbody.linearVelocity = speed * transform.up;

                if (distanceFromStop <= stoppingDistance)
                {
                    rigidbody.linearVelocity = Vector2.zero;
                    StartPatrol();

                    MovingToDestination = false;
                }
            }

            if (!patrolling && !MovingToDestination)
            {
                rigidbody.linearVelocity = Vector2.zero;
            }
        }

        if (!isFollowing)
        {
            stoppingDistance = PatrolStopDistance;
        }
        else
        {
            stoppingDistance = PlayerStopDistance;
        }

    }
    private Vector2 GetAvoidanceVector()
    {
        Collider2D[] nearbyEnemies = Physics2D.OverlapCircleAll(transform.position, avoidanceRadius, enemyLayer);
        Vector2 avoidanceForce = Vector2.zero;

        foreach (Collider2D other in nearbyEnemies)
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
        if (other.gameObject.CompareTag("Projectile"))
        {
            destination = other.transform.position;
            destination.z = 0;
            MovingToDestination = true;
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && canSeePlayer)
        {
            isFollowing = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isFollowing = false;
            MovingToDestination = true;
        }
    }
    public void StartPatrol()
    {
        patrolling = true;
        choosingPoint = true;
        Invoke(nameof(Patrol), PatrolWaitTime);
    }
    private void Patrol()
    {
        if (!choosingPoint)
        {
            MovingToDestination = true;
        }
        else
        {
            choosingPoint = true;
            Invoke(nameof(Patrol), 0.1f);
        }
    }
    public void CancelStartPatrol()
    {
        choosingPoint = false;
        CancelInvoke(nameof(Patrol));
    }

    ///////////////////////////////////*////////////////////////////////////////
    private void OnDrawGizmos()
    {
        // Only run this while the game is actively playing and a path exists
        if (!Application.isPlaying || navPath == null || navPath.corners == null || navPath.corners.Length == 0)
            return;

        // 1. Draw the FINAL DESTINATION as a Red Wire Sphere so you know where they are trying to go
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(destination, 0.4f);

        // 2. Draw a line directly from the enemy to their current immediate steering target
        Gizmos.color = Color.cyan;
        if (navPath.corners.Length > 1)
        {
            Gizmos.DrawLine(transform.position, navPath.corners[1]);
        }

        // 3. Draw EVERY CORNER of the path as a yellow dot, connected by green lines
        for (int i = 0; i < navPath.corners.Length; i++)
        {
            // Draw a small solid yellow sphere at the corner so you can see where turns happen
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(navPath.corners[i], 0.15f);

            // Connect this corner to the next corner in the array
            if (i < navPath.corners.Length - 1)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawLine(navPath.corners[i], navPath.corners[i + 1]);
            }
        }
    }
    ////////////////////////////////////////////*///////////////////////////////////////////
}