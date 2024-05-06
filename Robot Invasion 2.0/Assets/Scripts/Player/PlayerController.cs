using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Rendering;
using Vector3 = UnityEngine.Vector3;

public class PlayerController : MonoBehaviour
{
    [Header("Movements")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float jumpForce;
    private UnityEngine.Vector3 moveDirection = UnityEngine.Vector3.zero;

    private CharacterController controller;
    [Header("Gravity")]
    [SerializeField] private float gravity;

    [SerializeField] private float groundDistance;
    [SerializeField] private LayerMask GroundMask;
    [SerializeField] private bool isCharacterGrounded = false;
    private Vector3 velocity = Vector3.zero;

    [Header("Animations")]
    private Animator anim;

    private void Start()
    {
        
        GetRefereces();
        InitVariables();
    }

    private void Update()
    {
        HandleIsGrounded();
        HandleJumping();
        HandleGravity();

        HandleRunning();
        HandleMovement();
        HandleAnimations();
    }

    private void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        moveDirection = new Vector3(moveX, 0, moveZ);
        moveDirection = moveDirection.normalized;
        moveDirection = transform.TransformDirection(moveDirection);

        controller.Move(moveDirection * moveSpeed * Time.deltaTime);
    }

    private void HandleRunning()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            moveSpeed = runSpeed;
        }
        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveSpeed = walkSpeed;
        }
    }
    
    private void HandleAnimations()
    {
        if(moveDirection == Vector3.zero)
        {
            anim.SetFloat("Speed", 0, 0.2f, Time.deltaTime);
        }
        else if(moveDirection != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))
        {
            anim.SetFloat("Speed", 0.5f, 0.2f, Time.deltaTime);
        }
        else if (moveDirection != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
        {
            anim.SetFloat("Speed", 1f, 0.2f, Time.deltaTime);
        }
    }
    private void HandleIsGrounded()
    {
        isCharacterGrounded = Physics.CheckSphere(transform.position, groundDistance);
    }

    private void HandleGravity()
    {
        if(isCharacterGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void HandleJumping()
    {
        if(Input.GetKeyDown(KeyCode.Space) && isCharacterGrounded)
        {
            velocity.y += Mathf.Sqrt(jumpForce * -2f * gravity);
        }
    }

    


    private void GetRefereces()
    {
      controller = GetComponent<CharacterController>();
      anim = GetComponentInChildren<Animator>();
    }

    private void InitVariables()
    {
        moveSpeed = walkSpeed;
    }
}
