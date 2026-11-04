using System.Collections;
using UnityEngine;

public class EnemyNav : MonoBehaviour
{
    PlayerController Player;
    public Transform player;

    Rigidbody2D rigidbody;
    private CircleCollider2D DetectionRadius;

    public bool isFollowing;
    public bool canMove;    
    public bool moving;
    public bool canAttack;
    public bool isAttacking;
    public bool onGround;
    public bool beingADumbass;
    public bool patrol;

    public float speed;
    public float attackSpeed;
    public float detectionRadius;

    public int PatrolRadiusLowX;
    public int PatrolRadiusLowY;
    public int PatrolRadiusHighX;
    public int PatrolRadiusHighY;
       

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
        if (rigidbody.linearVelocity.magnitude > 1)
        {
            moving = true;
        }

        if (isFollowing)
        {
            rigidbody.linearVelocity = speed * transform.up;


            Vector3 playerPos = player.transform.position;
            playerPos.z = transform.position.z;
            Vector2 lookDir = playerPos - transform.position;
            rigidbody.MoveRotation(Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f);
        }
        else
        {
            if(patrol)
            {
                
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isFollowing = true;
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isFollowing = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isFollowing = false;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player.health--;
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player.health--;
        }
    }

}
