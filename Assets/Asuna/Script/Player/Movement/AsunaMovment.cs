using UnityEngine;

public class AsunaMovment : MonoBehaviour
{
     public static Vector3 PlayerPosition {private set; get;}
     
     [SerializeField] private FixedJoystick joystick;        
     [SerializeField] Animator animator;        
     [SerializeField] private float moveSpeed = 6f;
     public float gravity = -9.81f;
     public float jumpHeight = 1.5f;
     [SerializeField] CharacterController controller;
     private float velocity = -2;
     
     private string isMoving;
     private string isIdle;
     private string isJumping;
     private void Awake()
     {
          controller = GetComponent<CharacterController>();
          animator = GetComponent<Animator>();
          isMoving = "isRuning";
          isIdle = "isIdle";
          isJumping = "IsJump";
          animator.SetBool(isMoving , false);
          animator.SetBool(isIdle , true);
          animator.SetBool(isJumping , false);
     }

     // private void Update()
     // {
     //      if (joystick.Horizontal != 0 || joystick.Vertical != 0)
     //      {
     //           animator.SetBool(isMoving , true);
     //           animator.SetBool(isIdle , false);
     //           animator.SetBool(isJumping , false);
     //           Vector3 move = new Vector3(joystick.Horizontal, velocity, joystick.Vertical);
     //           controller.Move(moveSpeed * Time.deltaTime * move);
     //           move.y = 0;
     //           transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(move),
     //                Time.fixedDeltaTime * 4f);
     //           PlayerPosition = transform.position;
     //      }
     //      else
     //      {
     //           animator.SetBool(isMoving, false);
     //           animator.SetBool(isIdle , true);
     //           animator.SetBool(isJumping , false);
     //      }
     // }

        
     [SerializeField] private Transform groundCheck;
     [SerializeField] private float groundDistance = 0.25f;
     [SerializeField] private LayerMask groundMask;
     
     private bool isGrounded;
     private float verticalVelocity;
     
     private void CheckGround()
     {
          isGrounded = Physics.CheckSphere(
               groundCheck.position,
               groundDistance,
               groundMask,
               QueryTriggerInteraction.Ignore);
     
          if (isGrounded && verticalVelocity < 0)
          {
               verticalVelocity = -2f;
          }
     }
     
     private void ApplyGravity()
     {
          verticalVelocity += gravity * Time.deltaTime;
     }
     
     private void MovePlayer()
     {
          Vector3 move = new Vector3(
               joystick.Horizontal,
               0,
               joystick.Vertical);
     
          move = Vector3.ClampMagnitude(move, 1f);
     
          move.y = verticalVelocity;
     
          controller.Move(move * moveSpeed * Time.deltaTime);
     
          if (move.x != 0 || move.z != 0)
          {
               Quaternion targetRotation =
                    Quaternion.LookRotation(new Vector3(move.x, 0, move.z));
     
               transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    10f * Time.deltaTime);
          }
     }
     private void Update()
     {
          CheckGround();
     
          ApplyGravity();
     
          MovePlayer();
     
          animator.SetBool(isMoving, joystick.Direction.magnitude > 0.1f);
          animator.SetBool(isIdle, joystick.Direction.magnitude <= 0.1f);
     }
     private void OnDrawGizmosSelected()
     {
          if (groundCheck == null)
               return;
     
          Gizmos.color = isGrounded ? Color.green : Color.red;
     
          Gizmos.DrawWireSphere(
               groundCheck.position,
               groundDistance);
     }
}