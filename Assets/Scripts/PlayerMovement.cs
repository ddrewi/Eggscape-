using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    // controls
    [SerializeField] public Joystick joystick;

    // Declare and initialize once for efficiency
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;
    private BoxCollider2D coll;

    // movement factors
    [SerializeField] private float regGrav = 2f;
    [SerializeField] private float lowGrav = 1f;
    [SerializeField] private float flutterVelDebuff = 0.7f;
    
    [SerializeField] private LayerMask jumpableGround;

    // SerializeField lets us change variable values from the Inspector
    private float dirX;
    private float moveSpeed;
    [SerializeField] private float baseSpeed = 5f;
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float jumpForce = 7f;

    [SerializeField] private TextMeshProUGUI timeText;

    private bool canMove = true;
    private bool floating = false;

    private float currentTime;
    private float otherTime;

    // enum is a set of defined constants that we can choose to assign to the variable
    // instead of having many booleans controlling states, we can have this one variable with one state
    private enum MovementState { idle, running, hit }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();
        ItemCollector.setCherries(0);
        currentTime = Time.time;
        otherTime = Time.time - 1f;
        dirX = 1;
        moveSpeed = baseSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        // This returns a decimal between -1 and 1 (?)
        // Think of a joystick--the farther in a direction you push, the faster you move (good for mobile joystick)
        // On a keyboard, this will always be -1 or 1 (?)

        currentTime = Time.time;

        timeText.text = "" + (int) (currentTime * 1000)/1000.0;
        if (currentTime > otherTime)
        {
            canMove = true;
        }
        else
        {
            canMove = false;
            rb.velocity = new Vector2(-1 * dirX * 10f, 5f);
            Debug.Log("Player can't move");
           
        }

        if (canMove)
        {
            if (joystick.Horizontal >= .25f)
            {
                dirX = 1;
            }
            else if (joystick.Horizontal <= -.25f)
            {
                dirX = -1;
            }
            else
            {
                dirX = 0;
            }
        }
        

        if (canMove)
        {
            rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
        }

        
        // low grav
        if (floating && rb.velocity.y < 0f){
            rb.gravityScale = lowGrav;
        }
        else{
             rb.gravityScale = regGrav;
        }

        /*
        float verticalMove = joystick.Vertical;

        if (verticalMove >= .6f && rb.velocity.y < 0)
        {
            rb.gravityScale = lowGrav;
            if (canMove)
            {
                rb.velocity = new Vector2(dirX * moveSpeed * flutterVelDebuff, rb.velocity.y);
            }

        }
        else
        {
            rb.gravityScale = regGrav;
        }
        */

        

        // When space is pressed, add a new velocity vector 7 units upward without changing x vector
        if (Input.GetKeyDown("space") && IsGrounded())
        {
            // 2D Game
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);

        }

        // moveSpeed = ( 5f* Mathf.Pow(2.7f, -6f / ItemCollector.getCherries())) + baseSpeed;

        moveSpeed = Mathf.Min(maxSpeed, baseSpeed + .3f * ItemCollector.getCherries()) ;
        // Debug.Log("moveSpeed: " + moveSpeed);
        UpdateAnimation();
    }

    // Method for updating Animations
    // ordered in terms of precedence
    private void UpdateAnimation()
    {
        MovementState state;

        // otherTime = currentTime + .35f;

        // Check for movement to update animations
        if (!canMove)
        {
            state = MovementState.hit;
        }

        else if (dirX > 0f)
        {
            state = MovementState.running;
            sprite.flipX = true;
        }

        else if (dirX < 0f)
        {
            state = MovementState.running;
            sprite.flipX = false;
        }

        else {
            state = MovementState.idle;
        }

        anim.SetInteger("state", (int)state);
    }


    public void jump()
    {
        if (IsGrounded())
        { 
            rb.velocity = new Vector2(dirX * -10f, jumpForce);
            
        }
    }

    public void yesFloating()
    {
        floating = true;
    }

    public void noFloating()
    {
        floating = false;
    }
    /*
    while (!IsGrounded())
            {
                if (rb.velocity.y < 0)
                {
                    rb.gravityScale = lowGrav;
                    if (canMove)
                    {
                        rb.velocity = new Vector2(dirX * moveSpeed * flutterVelDebuff, rb.velocity.y);
                    }
                }
            }
            rb.gravityScale = regGrav;
    */




    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, 0.1f, jumpableGround);
        
    }

    public void resetJumpForce()
    {
        jumpForce = 7f;
        Debug.Log("Reset Jump Force: " + jumpForce);
    }

    public void BounceBack()
    {
        currentTime = Time.time;
        otherTime = currentTime + .35f;
        Debug.Log("Message received.\ncurrent time: " + 
            currentTime + "\notherTime: " + otherTime, this);
        canMove = false;
        Debug.Log("Velocity changed" + rb.velocity);
    }

    public bool getMoveState()
    {
        return canMove;
    }

}
