using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour
{

    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float colliderDistance;
    [SerializeField] private float range;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform guard;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private bool facingLeft;

    [SerializeField] private Transform player;

    [SerializeField] private float patrolDistance;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private LayerMask jumpableGround;

    private GameObject[] waypoints;

    [SerializeField] private bool isPatrol;


    private int currentWaypointIndex = 0;

    private Vector3 initScale;
    private bool movingLeft = false;

    private SpriteRenderer sprite;
    private Animator anim;
    private enum MovementState { idle, running }

    public void Awake()
    {
        initScale = transform.localScale;

    }

    public void Start()
    {
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        if (facingLeft)
        {
            DirectionChange();
            movingLeft = true;
        }

        if (isPatrol)
        {

            GameObject tempObject1 = new GameObject();
            GameObject tempObject2 = new GameObject();
            tempObject1.transform.position = new Vector3(transform.position.x + patrolDistance, transform.position.y, transform.position.z);
            tempObject2.transform.position = new Vector3(transform.position.x - patrolDistance, transform.position.y, transform.position.z);
            waypoints = new GameObject[] {tempObject1, tempObject2};
           
        }
    }

    void Update()
    {
        if (PlayerInSight())
        {
            if(transform.position.x > player.position.x)
            {
                MoveInDirection(-1);
                if (!movingLeft)
                {
                    DirectionChange();
                }
                movingLeft = true;
            }
            else
            {
                MoveInDirection(1);
                if (movingLeft)
                {
                    DirectionChange();
                }
                movingLeft = false;
            }

            /*if (rb.velocity.x == 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
            if(player.position.y > transform.position.y && IsGrounded())
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }*/
            if (CanJump())
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }

        } 
        else if (isPatrol)
        {
            if (Vector2.Distance(waypoints[currentWaypointIndex].transform.position, transform.position) < 0.1f)
            {
                currentWaypointIndex++;
                if (currentWaypointIndex >= waypoints.Length)
                {
                    currentWaypointIndex = 0;
                }
                DirectionChange();
            }
            rb.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypointIndex].transform.position, Time.deltaTime * walkSpeed);
        }


        UpdateAnimation();

    }

    private void DirectionChange()
    {
        initScale.x = -initScale.x;
        transform.localScale = initScale;
    }

    private bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance, 
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);
        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    private void MoveInDirection(int _direction)
    {
        //transform.localScale = new Vector3(Mathf.Abs(initScale.x) * _direction, initScale.y, initScale.z);

       // transform.position = new Vector3(transform.position.x + _direction * Time.deltaTime * runSpeed,
          //  transform.position.y, transform.position.z);

        rb.velocity = new Vector2(runSpeed * _direction, rb.velocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, 0.1f, jumpableGround);

    }

    private bool CanJump()
    {
        bool hit = (Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, new Vector2(1, 0), 0.05f, jumpableGround)
                || Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, new Vector2(-1, 0), 0.05f, jumpableGround))
                && IsGrounded();

        if (hit)
        {
            Debug.Log("hit", this);
        }
        return hit;
        
        
    }

    private void UpdateAnimation()
    {
        MovementState state;

        // Check for movement to update animations
        if (rb.velocity.x > 0f)
        {
            state = MovementState.running;
        }

        else if (rb.velocity.x < 0f)
        {
            state = MovementState.running;
        }

        else
        {
            state = MovementState.idle;
        }

        anim.SetInteger("state", (int)state);
    }
}
