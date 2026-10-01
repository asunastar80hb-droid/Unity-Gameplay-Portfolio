using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class AsunaMovmentControler : MonoBehaviour
{
    public static Vector3 PlayerPosition;

    [Header("References")]
    public Joystick joystick;
    public Animator animator;

    [Header("Movement")]
    public float moveSpeed = 6f;
    public float rotationSpeed = 10f;

    [Header("Jump")]
    public float jumpForce = 7f;

    [Header("Ground Check")]
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.25f;
    public float groundOffset = 0.1f;

    [Header("Gravity")]
    public float gravity = -20f;
    public float groundedVelocity = -2f;
    
    [Header("Animation Parameters")]
    [SerializeField] private string isMoving = "isRuning";
    [SerializeField] private string isIdle = "isIdle";
    [SerializeField] private string isJumping = "IsJump";

    private bool isJump;
    private Rigidbody rb;
    private CapsuleCollider capsule;
    public bool isGrounded;
    private Vector3 movement;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        capsule = GetComponent<CapsuleCollider>();

        rb.freezeRotation = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        rb.useGravity = false;
        rb.mass = 1f;
    }

    private void Update()
    {
        GetMovementInput();

        CheckGround();

        UpdateAnimation();

        PlayerPosition = transform.position;
    }

    private void FixedUpdate()
    {
        MovePlayer();
        RotatePlayer();
    }

    private void GetMovementInput()
    {
        float horizontal = joystick != null ? joystick.Horizontal : Input.GetAxisRaw("Horizontal");

        float vertical = joystick != null ? joystick.Vertical : Input.GetAxisRaw("Vertical");

        Vector3 input = new Vector3(horizontal, 0f, vertical);

        movement = Vector3.ClampMagnitude(input, 1f);
    }

    private void MovePlayer()
    {
        Vector3 velocity = rb.linearVelocity;

        velocity.x = movement.x * moveSpeed;
        velocity.z = movement.z * moveSpeed;

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

    private void RotatePlayer()
    {
        if (movement.sqrMagnitude < 0.01f) return;

        Quaternion targetRotation = Quaternion.LookRotation(movement);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.fixedDeltaTime
        );
    }
    public void Jump()
    {
        if (isJump) return;
        
        CheckGround();

        if (!isGrounded) return;
        
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

        Vector3 checkPosition = new Vector3(
            bounds.center.x,
            bounds.min.y - groundOffset,
            bounds.center.z
        );

        bool detected = Physics.CheckSphere(
            checkPosition,
            groundCheckRadius,
            groundLayer,
            QueryTriggerInteraction.Ignore
        );

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
        if (animator == null) return;

        bool moving = movement.sqrMagnitude > 0.01f;

        animator.SetBool(isMoving, moving && !isJump);
        animator.SetBool(isIdle, !moving && !isJump);
        animator.SetBool(isJumping, isJump);
    }

    private void OnDrawGizmosSelected()
    {
        CapsuleCollider col = GetComponent<CapsuleCollider>();

        if (col == null) return;

        Bounds bounds = col.bounds;

        Vector3 checkPosition = new Vector3(
            bounds.center.x,
            bounds.min.y - groundOffset,
            bounds.center.z
        );

        Gizmos.color = isGrounded ? Color.green : Color.red;

        Gizmos.DrawWireSphere(checkPosition, groundCheckRadius);
    }
}