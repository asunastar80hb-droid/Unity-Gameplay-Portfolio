using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody) , typeof(CapsuleCollider))]
public class MobilePlayerController_RB  : MonoBehaviour
{
     public static Vector3 PlayerPosition {private set; get;}
     
     [SerializeField] private FixedJoystick joystick;        
     [SerializeField] private Animator animator;        
     [SerializeField] private float moveSpeed = 6f;
     public float gravity = -9.81f;
     public float jumpHeight = 1.5f;
     private CharacterController controller;
     private float velocity = 0;
     
     private string isMoving;
     private void Awake()
     {
          controller = GetComponent<CharacterController>();
          animator = GetComponent<Animator>();
          isMoving = "isMoving";
     }

     private void Update()
     {
          if (joystick.Horizontal != 0 || joystick.Vertical != 0)
          {
               animator.SetBool(isMoving , true);
               Vector3 move = new Vector3(joystick.Horizontal, velocity, joystick.Vertical);
               controller.Move(moveSpeed * Time.deltaTime * move);
               move.y = 0;
               transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(move),
                    Time.fixedDeltaTime * 4f);
               PlayerPosition = transform.position;
          }
          else
          {
               animator.SetBool(isMoving, false);
          }
          
     }
}