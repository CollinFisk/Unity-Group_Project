using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(CircleCollider2D))]
public class EnemyNav : MonoBehaviour
{
    private PlayerController player;

    [SerializeField] private Transform collisionObject;

    private Rigidbody2D rigidbody;
    private CircleCollider2D DetectionRadiusCollider;

    [SerializeField] private bool canSeePlayer;
    public bool isFollowing;
    //[SerializeField] private bool moving;
    [SerializeField] private bool beingADumbass;
    [SerializeField] private bool patrol;
    [SerializeField] private bool stopping;

    [SerializeField] private float PlayerStopDistance;
    [SerializeField] private float PatrolStopDistance;
    [SerializeField] private float PatrolWaitTime;


    public float speed;
    public float baseSpeed;

    [SerializeField] private float detectionRadius;
    [SerializeField] private float stoppingDistance;
    [SerializeField] private float runAwayDistance;


    [SerializeField] private float PatrolRadiusLowX;
    [SerializeField] private float PatrolRadiusLowY;
    [SerializeField] private float PatrolRadiusHighX;
    [SerializeField] private float PatrolRadiusHighY;

    private Vector3 destination;
    [SerializeField] private LayerMask wallLayer;


    private NavMeshPath navPath;
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        rigidbody = transform.GetComponent<Rigidbody2D>();
        DetectionRadiusCollider = GetComponent<CircleCollider2D>();

        navPath = new NavMeshPath();
    }

    // Update is called once per frame adfafasdfa
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
        else if (patrol && !stopping)
        {
            if (distanceFromStop <= 3)
            {
                destination.x = Random.Range(PatrolRadiusLowX, PatrolRadiusHighX);
                destination.y = Random.Range(PatrolRadiusLowY, PatrolRadiusHighY);
                destination.z = 0;
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

        if (isFollowing)
        {
            Vector2 lookDir = steeringTarget - transform.position;
            rigidbody.MoveRotation(Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f);

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
            if (stopping && !beingADumbass)
            {
                rigidbody.linearVelocity = speed * transform.up;

                Vector2 lookDir = steeringTarget - transform.position;
                rigidbody.MoveRotation(Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f);


                if (distanceFromStop <= stoppingDistance)
                {
                    rigidbody.linearVelocity = Vector2.zero;
                    stopping = false;

                    StartCoroutine(StartPatrol());
                }
            }

            if (stopping && beingADumbass)
            {
                if (player.transform.position.y > transform.position.y)
                {
                    rigidbody.AddForce(speed * transform.up);
                }
                if (player.transform.position.y < transform.position.y)
                {
                    rigidbody.AddForce(speed * -transform.up);
                }
                if (player.transform.position.x > transform.position.x)
                {
                    rigidbody.AddForce(speed * transform.right);
                }
                if (player.transform.position.x < transform.position.x)
                {
                    rigidbody.AddForce(speed * -transform.right);
                }


                rigidbody.linearVelocity = speed * transform.up;

                Vector2 lookDir = steeringTarget - transform.position;
                rigidbody.MoveRotation(Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f);

                if (distanceFromStop <= stoppingDistance)
                {
                    stopping = false;

                    StartCoroutine(StartPatrol());
                }
            }

            if (!patrol && !stopping)
            {
                rigidbody.linearVelocity = Vector2.zero;
            }

            // Note: The patrol generation chunk that was here has safely moved up into our Fix 1 block!
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && canSeePlayer)
        {
            isFollowing = true;
        }
        if (other.gameObject.CompareTag("Projectile"))
        {
            // --- FIX 2: UPDATE THE RAW VECTOR SO PATHFINDING REGISTER IT ---
            destination = other.transform.position;
            destination.z = 0;
            stopping = true;
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
            stopping = true;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        beingADumbass = true;
        collisionObject = collision.gameObject.transform;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        beingADumbass = false;
        collisionObject = null;
    }
    IEnumerator StartPatrol()
    {
        yield return new WaitForSeconds(PatrolWaitTime);
        stopping = true;
    }
}