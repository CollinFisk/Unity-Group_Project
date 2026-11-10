using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class EnemyNav : MonoBehaviour
{    
    PlayerController player;

    public Transform collisionObject;

    Rigidbody2D rigidbody;
    private CircleCollider2D DetectionRadius;

    [SerializeField] private bool canSeePlayer;
    [SerializeField] private bool isFollowing;
    //[SerializeField] private bool moving;
    [SerializeField] private bool beingADumbass;
    [SerializeField] private bool patrol;
    [SerializeField] private bool stopping;

    public float speed;
    public float baseSpeed;

    [SerializeField] private float detectionRadius;
    [SerializeField] private float stoppingDistance;

    [SerializeField] private float PatrolRadiusLowX;
    [SerializeField] private float PatrolRadiusLowY;
    [SerializeField] private float PatrolRadiusHighX;
    [SerializeField] private float PatrolRadiusHighY;

    public Vector3 destination;
    public Transform destinationPoint;
    public LayerMask wallLayer;
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        rigidbody = transform.GetComponent<Rigidbody2D>();
        DetectionRadius = transform.GetComponent<CircleCollider2D>();
        destinationPoint = transform.GetChild(2);
    }

    // Update is called once per frame adfafasdfa
    void Update()
    {
        Vector2 origin = transform.position;
        Vector2 direction = (Vector2)player.transform.position - origin;
        float distance = direction.magnitude;
        direction.Normalize();
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance, wallLayer);

        float distanceFromStop = Vector2.Distance(transform.position, destinationPoint.position);
        destinationPoint.position = destination;
        if (hit.collider != null)
        {
            canSeePlayer = false;
            isFollowing = false;
        }
        else
        {
            canSeePlayer = true;
        }
        /*
        if (rigidbody.linearVelocity.magnitude > 1)
        {
            moving = true;
        }
        else
            moving = false;
        */
        if (isFollowing)
        {
            rigidbody.linearVelocity = speed * transform.up;

            destination = player.transform.position;
            destination.z = 0;
            Vector2 lookDir = destination - transform.position;
            rigidbody.MoveRotation(Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f);

            if (distanceFromStop <= stoppingDistance)
            {
                rigidbody.linearVelocity = Vector2.zero;
            }
        }
        else
        {
            if (stopping && !beingADumbass)
            {
                rigidbody.linearVelocity = speed * transform.up;

                Vector2 lookDir = destination - transform.position;
                rigidbody.MoveRotation(Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f);


                if (distanceFromStop <= stoppingDistance)
                {
                    rigidbody.linearVelocity = Vector2.zero;
                    stopping = false;

                    StartCoroutine("StartPatrol");
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

                Vector2 lookDir = destination - transform.position;
                rigidbody.MoveRotation(Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f);

                if (distanceFromStop <= stoppingDistance)
                {
                    stopping = false;

                    StartCoroutine("StartPatrol");
                }
            }

            if (!patrol && !stopping)
            {
                rigidbody.linearVelocity = Vector2.zero;
            }
            if (patrol && !stopping)
            {
                if (distanceFromStop <= 3)
                {
                    destination.x = Random.Range(PatrolRadiusLowX, PatrolRadiusHighX);
                    destination.y = Random.Range(PatrolRadiusLowY, PatrolRadiusHighY);
                    destination.z = 0;
                }
            }
        }
        if (!isFollowing)
        {
            stoppingDistance = 0.1f;
        }
        else
        {
            stoppingDistance = 1.5f;
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
            destinationPoint.position = other.transform.position;
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
        yield return new WaitForSeconds(5);
        stopping = true;
    }
}
