using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement_Platformer : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    bool isGrounded = false;
    public Transform isGroundedChecker;
    public float checkGroundRadius;
    public LayerMask groundLayer;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public float rememberGroundedFor;
    float lastTimeGrounded;
    public int defaultAdditionalJumps = 1;
    int additionalJumps;
    private SpriteRenderer _renderer;
    bool isCrouching = false;
    bool isJumping = false;
    bool jumped = false;
    bool isUnderPlatform = false;
    

    Rigidbody2D rb;
    Animator animator;
    BoxCollider2D boxCollider2D;
    CircleCollider2D circleCollider2D;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        _renderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        circleCollider2D = GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        Move();
        Jump();
        BetterJump();
        checkIfFalling();
        CheckIfGrounded();
        Crouch();
    }
    private void FixedUpdate()
    {
        // Crouching...
        boxCollider2D.isTrigger = (isCrouching || isUnderPlatform) ? true : false;
    }

    void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float moveBy = x * speed;
        rb.velocity = new Vector2(moveBy, rb.velocity.y);
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            _renderer.flipX = false;
        }
        else if (Input.GetAxisRaw("Horizontal") < 0)
        {
            _renderer.flipX = true;
        }
        animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
    }
    void checkIfFalling()
    {
        if (rb.velocity.y < -0.1)
        {
            //if (!jumped)
            //    GetComponent<Animator>().Play("Player_Fall");
            isJumping = false;
            animator.SetBool("IsJumping", isJumping);
        }
        else
        {
            
        }
    }
    void Crouch()
    {
        if (Input.GetKeyDown(KeyCode.S) && isGrounded)
        {
            if(!isCrouching)
                GetComponent<Animator>().Play("Player_Crouch_Transition");
            isCrouching = true;
            animator.SetBool("IsCrouching", isCrouching);
        }
        else if(Input.GetKeyUp(KeyCode.S))
        {
            if (isUnderPlatform)
                return;
            else
            {
                isCrouching = false;
                animator.SetBool("IsCrouching", isCrouching);
            }
        }
    }
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && (isGrounded || Time.time - lastTimeGrounded <= rememberGroundedFor || additionalJumps > 0) && !isUnderPlatform)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            additionalJumps--;
            jumped = true;
            isJumping = true;
            animator.SetBool("IsJumping", isJumping);
            GetComponent<Animator>().Play("Player_Jump");
        }
    }
    void CheckIfGrounded()
    {
        Collider2D colliders = Physics2D.OverlapCircle(isGroundedChecker.position, checkGroundRadius, groundLayer);
        if (colliders != null && !(rb.velocity.y < -0.01))
        {
            isGrounded = true;
            additionalJumps = defaultAdditionalJumps;
        }
        else
        {
            if (isGrounded)
            {
                lastTimeGrounded = Time.time;
            }
            isGrounded = false;
        }
    }
    void BetterJump()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            rb.velocity += Vector2.up * Physics2D.gravity * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" && !(rb.velocity.y < -0.1))
        {
            animator.SetBool("IsGrounded", true);
            jumped = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            animator.SetBool("IsGrounded", false);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" && !(rb.velocity.y < -0.1))
        {
            animator.SetBool("IsGrounded", true);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if ((groundLayer.value & (1 << collision.gameObject.layer)) > 0)
            isUnderPlatform = true;
        Debug.Log("Under:" + isUnderPlatform);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((groundLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            isCrouching = false;
            animator.SetBool("IsCrouching", isCrouching);
            isUnderPlatform = false;
        }
        Debug.Log("Under:" + isUnderPlatform);
    }
}
