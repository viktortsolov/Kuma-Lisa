using UnityEngine;
using UnityEngine.SceneManagement;

public class Fox : MonoBehaviour
{
    Rigidbody2D rb;
    Animator animator;
    [SerializeField] Collider2D standingCollider, crouchingCollider;
    [SerializeField] Transform groundCheckCollider;
    [SerializeField] Transform overheadCheckCollider;
    [SerializeField] LayerMask groundLayer;

    const float groundCheckRadius = 0.2f;
    const float overheadCheckRadius = 0.2f;
    [SerializeField] float speed = 1;
    [SerializeField] float jumpPower = 500;
    float horizontalValue;
    float runSpeedModifier = 2;
    float crouchSpeedModifier = 0.5f;

    bool isGrounded;
    bool isRunning;
    bool facingRight = true;
    bool jump;
    bool crouchPressed;
    bool isDead = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!CanMoveOrInteract())
        {
            return;
        }

        //Set the yVelocity in the animator
        animator.SetFloat("yVelocity", rb.velocity.y);

        //Store the horizontal value
        horizontalValue = Input.GetAxisRaw("Horizontal");

        //Go to menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }

        //If LShift is clicked enable isRunning, otherwise disable it
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isRunning = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRunning = false;
        }

        //If we press Jump button enable jump, otherwise disable it
        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            animator.SetBool("Jump", true);
        }
        else if (Input.GetButtonUp("Jump"))
        {
            jump = false;
        }

        //If we press Crouch button enable crouch, otherwise disable it
        if (Input.GetButtonDown("Crouch"))
        {
            crouchPressed = true;
        }
        else if (Input.GetButtonUp("Crouch"))
        {
            crouchPressed = false;
        }
    }

    private void FixedUpdate()
    {
        GroundCheck();
        Move(horizontalValue, jump, crouchPressed);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(groundCheckCollider.position, groundCheckRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(overheadCheckCollider.position, overheadCheckRadius);
    }

    void GroundCheck()
    {
        isGrounded = false;
        //Check if the GroundCheckObject is colliding with other 2D Colliders that are paprt in the "Ground" layer
        //If yes (isGrounded true) else (isGrounded false)
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckCollider.position, groundCheckRadius, groundLayer);
        if (colliders.Length > 0)
        {
            isGrounded = true;
        }

        //As long as we are grounded the "Jump" bool in the animator is disabled
        animator.SetBool("Jump", !isGrounded);
    }

    private void Move(float dir, bool jumpFlag, bool crouchFlag)
    {
        #region Move & Run
        //Set value of x using dir & speed
        float xValue = dir * speed * 100 * Time.fixedDeltaTime;
        //If we are running multiply with the running modifier
        if (isRunning)
        {
            xValue *= runSpeedModifier;
        }

        //If we are crouching reduce the speed
        if (crouchFlag)
        {
            xValue *= crouchSpeedModifier;
        }

        //Create Vec2 for the velocity and set the player's velocity
        Vector2 targetVelocity = new Vector2(xValue, rb.velocity.y);
        rb.velocity = targetVelocity;

        //If looking right and clicked left flip to the left
        if (facingRight && dir < 0)
        {
            transform.localScale = new Vector3(-10, 10, 10);
            facingRight = false;
        }
        //If looking left and clicked right flip to the right
        else if (!facingRight && dir > 0)
        {
            transform.localScale = new Vector3(10, 10, 10);
            facingRight = true;
        }

        //0 for idle
        //4 for walking
        //8 for running

        //Set the float xVelocity according to the value of the RigidBody2D velocity
        animator.SetFloat("Blend", Mathf.Abs(rb.velocity.x));
        #endregion

        #region Jump & Crouch
        //If we are crouching and disabled crouching
        //Check overhead for collision with Ground items
        //If there are any, remain crouched, otherwise un-crouch
        if (!crouchFlag)
        {
            if (Physics2D.OverlapCircle(overheadCheckCollider.position, overheadCheckRadius, groundLayer))
            {
                crouchFlag = true;
            }
        }

        if (isGrounded)
        {
            //If we press Crouch we disable the standing collider and we animate crouching
            //Reduce the speed
            //If released resume the original speed + enable the standing collider + disable crouch animation
            standingCollider.enabled = !crouchFlag;

            //If the player is grounded and press Space - Jump
            if (jumpFlag)
            {
                jumpFlag = false;

                //Add jump force
                rb.AddForce(new Vector2(0f, jumpPower));
            }
        }

        animator.SetBool("Crouch", crouchFlag);
        crouchingCollider.enabled = crouchFlag;
        #endregion
    }

    bool CanMoveOrInteract()
    {
        bool can = true;

        if (FindObjectOfType<InteractionSystem>().isExamining)
        {
            can = false;
        }
        if (FindObjectOfType<InventorySystem>().isOpen)
        {
            can = false;
        }
        if (isDead)
        {
            can = false;
        }

        return can;
    }

    public void Die()
    {
        isDead = true;
        FindObjectOfType<LevelManager>().Restart();
    }
}
