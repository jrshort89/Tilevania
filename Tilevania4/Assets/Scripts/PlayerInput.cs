using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] float defaultGravity = 8f;


    Vector2 moveInput;
    Rigidbody2D myRigidBody;
    Animator myAnimator;
    int ground;
    int climbableObject;
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        ground = LayerMask.GetMask("Ground");
        climbableObject = LayerMask.GetMask("Climb");
    }

    // Update is called once per frame
    void Update()
    {        
        Run();
        ChangeSpriteDirection();
        StartClimbing();
    }

    void Run()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;        
        myAnimator.SetBool("isRunning", playerHasHorizontalSpeed);        
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();        
    }

    void ChangeSpriteDirection()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), 1f);
        }         
    }

    void OnJump(InputValue value)
    {        
        if (myRigidBody.IsTouchingLayers(ground))
        {            
            myRigidBody.velocity += new Vector2(0f, jumpSpeed);
        }        
    }

    void StartClimbing()
    {
        bool playerHasVerticalalSpeed = Mathf.Abs(myRigidBody.velocity.y) > Mathf.Epsilon;
        bool isTouchingClimableObject = myRigidBody.IsTouchingLayers(climbableObject);
        myAnimator.SetBool("isClimbing", isTouchingClimableObject);
        if (!isTouchingClimableObject) 
        {
            myRigidBody.gravityScale = defaultGravity;
            return; 
        }                    
        if (isTouchingClimableObject)
        {
            Vector2 climbVelocity = new Vector2(myRigidBody.velocity.x, moveInput.y * climbSpeed);
            myRigidBody.velocity = climbVelocity;
            myRigidBody.gravityScale = 0;
        }        
        if (!playerHasVerticalalSpeed)
        {
            myAnimator.SetBool("isClimbing", playerHasVerticalalSpeed);
        }
    }
}
