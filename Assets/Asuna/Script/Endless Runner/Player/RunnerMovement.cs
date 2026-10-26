using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class RunnerMovement : MonoBehaviour
{
    private Animator animator;

    [Header("Forward Movement")]
    [SerializeField] private float forwardSpeed = 8f;

    [Header("Lane Movement")]
    [SerializeField] private float laneDistance = 2.5f;
    [SerializeField] private float laneChangeSpeed = 10f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float gravity = -20f;
    [SerializeField] private float groundedVelocity = -2f;

    [Header("Ground Check")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckRadius = 0.25f;
    [SerializeField] private float groundOffset = 0.1f;

    [Header("Animation Parameters")]
    [SerializeField] private string isMoving = "isRunning";
    [SerializeField] private string isIdle = "isIdle";
    [SerializeField] private string isJumping = "IsJump";
    [SerializeField] private string deathTrigger = "Die";
    
    private Rigidbody rb;
    private CapsuleCollider capsule;

    private int currentLane = 1;

    private bool isDead;
    private bool isJump;
    private bool isGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        capsule = GetComponent<CapsuleCollider>();

        rb.freezeRotation = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        rb.useGravity = false;

        if (animator == null)
            animator = GetComponent<Animator>();

        SetAnimationState(false, true, false);
    }

    private void Update()
    {
        if (isDead)
            return;
        
        HandleInput();
        CheckGround();
        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        if (isDead)
            return;
        
        MovePlayer();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveLeft();
        }

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveRight();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }
    private void MoveLeft()
    {
        if (currentLane <= 0)
            return;

        currentLane--;
    }
    private void MoveRight()
    {
        if (currentLane >= 2)
            return;

        currentLane++;
    }

    private void MovePlayer()
    {
        Vector3 velocity = rb.linearVelocity;

        velocity.z = forwardSpeed;

        float targetX = (currentLane - 1) * laneDistance;

        float newX = Mathf.Lerp(transform.position.x, targetX, laneChangeSpeed * Time.fixedDeltaTime
        );

        float horizontalVelocity = (newX - transform.position.x) / Time.fixedDeltaTime;

        velocity.x = horizontalVelocity;

        if (isGrounded && !isJump)
        {
            velocity.y = groundedVelocity;
        }
        else
        {
            velocity.y += gravity * Time.fixedDeltaTime;
        }

        rb.linearVelocity = velocity;
    }

    private void Jump()
    {
        if (isJump)
            return;

        if (!isGrounded)
            return;

        isJump = true;
        isGrounded = false;

        Vector3 velocity = rb.linearVelocity;

        velocity.y = jumpForce;

        rb.linearVelocity = velocity;

        UpdateAnimation();
    }
    private void CheckGround()
    {
        Bounds bounds = capsule.bounds;

        Vector3 checkPosition = new Vector3(bounds.center.x,
            bounds.min.y - groundOffset, bounds.center.z);

        bool detected = Physics.CheckSphere(checkPosition, groundCheckRadius,
            groundLayer, QueryTriggerInteraction.Ignore);

        if (isJump)
        {
            if (rb.linearVelocity.y <= 0f && detected)
            {
                isGrounded = true;
                isJump = false;
            }
            else
            {
                isGrounded = false;
            }
        }
        else
        {
            isGrounded = detected;
        }
    }

    private void UpdateAnimation()
    {
        if (animator == null)
            return;

        if (isJump)
        {
            SetAnimationState(moving: false, idle: false, jumping: true);
            return;
        }

        SetAnimationState(moving: true, idle: false, jumping: false);
    }
    private void SetAnimationState(bool moving, bool idle, bool jumping)
    {
        animator.SetBool(isMoving, moving);
        animator.SetBool(isIdle, idle);
        animator.SetBool(isJumping, jumping);
    }
    public void PlayDeathAnimation()
    {
        if (isDead)
            return;

        isDead = true;

        rb.linearVelocity = Vector3.zero;
        rb.useGravity = true;

        if (animator != null)
        {
            animator.ResetTrigger(deathTrigger);
            animator.SetTrigger(deathTrigger);
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        CapsuleCollider col = GetComponent<CapsuleCollider>();

        if (col == null)
            return;

        Bounds bounds = col.bounds;
        Vector3 checkPosition = new Vector3(bounds.center.x, bounds.min.y - groundOffset, bounds.center.z);

        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(checkPosition, groundCheckRadius);
    }
    
}