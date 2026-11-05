using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyNav : MonoBehaviour
{    PlayerController Player;
    public Transform player;

    Rigidbody2D rigidbody;
    private CircleCollider2D DetectionRadius;

    public bool canSeePlayer;
    public bool isFollowing;
    public bool moving;
    public bool canAttack;
    public bool isAttacking;
    public bool beingADumbass;
    public bool patrol;
    public bool stopping;

    public float speed;
    public float attackSpeed;
    public float detectionRadius;
    public float stoppingDistance;

    public float PatrolRadiusLowX;
    public float PatrolRadiusLowY;
    public float PatrolRadiusHighX;
    public float PatrolRadiusHighY;

    public Vector3 destination;
    public Transform destinationPoint;
    public LayerMask wallLayer;
    void Start()
    {
        Player = player.GetComponent<PlayerController>();
        player = GameObject.Find("player").transform;
        rigidbody = transform.GetComponent<Rigidbody2D>();
        DetectionRadius = transform.GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 origin = transform.position;
        Vector2 direction = (Vector2)player.position - origin;
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

        if (rigidbody.linearVelocity.magnitude > 1)
        {
            moving = true;
        }

        if (isFollowing)
        {
            rigidbody.linearVelocity = speed * transform.up;

            destination = player.transform.position;
            destination.z = 0;
            Vector2 lookDir = destination - transform.position;
            rigidbody.MoveRotation(Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f);
        }
        else
        {
            if(stopping && !beingADumbass)
            {
                rigidbody.linearVelocity = speed * transform.up;

                Vector2 lookDir = destination - transform.position;
                rigidbody.MoveRotation(Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f);
                if (!patrol)
                {
                    StartCoroutine("Stopping");
                }
                if (distanceFromStop <= stoppingDistance)
                {
                    rigidbody.linearVelocity = Vector2.zero;
                    stopping = false;
                    if (!patrol)
                    {
                        StopCoroutine("Stopping");
                    }
                    else
                    StartCoroutine("StartPatrol");
                }
            }

            if (stopping && beingADumbass)
            {
                rigidbody.linearVelocity = speed * transform.up;

                Vector2 lookDir = destination - transform.position;
                rigidbody.MoveRotation(Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f);
                if (!patrol)
                {
                    StartCoroutine("Stopping");
                }
                if (distanceFromStop <= stoppingDistance)
                {
                    stopping = false;
                    if (!patrol)
                    {
                        StopCoroutine("Stopping");
                    }
                    else
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
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && canSeePlayer)
        {
            isFollowing = true;
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
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        beingADumbass = false;
    }
    IEnumerator Stopping()
    {
        yield return new WaitForSeconds(5);
        stopping = false;
    }
    IEnumerator StartPatrol()
    {

        yield return new WaitForSeconds(5);

        //Time For the Patrol Position to find a good point
        stopping = true;
    }

}
