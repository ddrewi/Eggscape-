using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chicken : MonoBehaviour
{

    [SerializeField] public Joystick joystick;

    // Declare and initialize once for efficiency
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;
    private BoxCollider2D coll;

    [SerializeField] private float regGrav = 2f;
    [SerializeField] private float lowGrav = 0.35f;
    [SerializeField] private float flutterVelDebuff = 0.5f;

    [SerializeField] private LayerMask jumpableGround;

    // SerializeField lets us change variable values from the Inspector
    private float dirX;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 8.5f;

    // enum is a set of defined constants that we can choose to assign to the variable
    // instead of having many booleans controlling states, we can have this one variable with one state
    private enum MovementState { idle, running }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // This returns a decimal between -1 and 1 (?)
        // Think of a joystick--the farther in a direction you push, the faster you move (good for mobile joystick)
        // On a keyboard, this will always be -1 or 1 (?)
        dirX = joystick.Horizontal;

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


        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

        float verticalMove = joystick.Vertical;

        if (verticalMove >= .6f && rb.velocity.y < 0)
        {
            rb.gravityScale = lowGrav;
            rb.velocity = new Vector2(dirX * moveSpeed * flutterVelDebuff, rb.velocity.y);

        }
        else
        {
            rb.gravityScale = regGrav;
        }


        // When space is pressed, add a new velocity vector 7 units upward without changing x vector
        if (Input.GetKeyDown("space") && IsGrounded())
        {
            // 2D Game
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);

        }

        UpdateAnimation();
    }

    // Method for updating Animations
    // ordered in terms of precedence
    private void UpdateAnimation()
    {
        MovementState state;

        // Check for movement to update animations
        if (dirX > 0f)
        {
            state = MovementState.running;
            sprite.flipX = true;
        }

        else if (dirX < 0f)
        {
            state = MovementState.running;
            sprite.flipX = false;
        }

        else
        {
            state = MovementState.idle;
        }

        anim.SetInteger("state", (int)state);
    }

    public void jump()
    {
        if (IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, 0.1f, jumpableGround);

    }
}
