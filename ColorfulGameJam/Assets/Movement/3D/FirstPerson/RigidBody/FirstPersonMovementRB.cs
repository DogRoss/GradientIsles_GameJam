
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class FirstPersonMovementRB : Move3DStateMachine
{
    [Header("Move Values")]
    public float moveSpeed = 6f;
    public float sprintMultiplier = 1.5f;
    float currentSprintVal = 1f; //current multiplier for walking, if shift it sets to sprintMultiplier

    [Header("Air Move Values")]
    [SerializeField] float airMultiplier = 0.25f; //when in air, how much movement is affected
    [SerializeField] float airMaxSpeed = 20f; //max speed of player while in the air
    [SerializeField] float jumpForce = 10f; //how much force is exerted when jumping
    bool jumping = false;

    [Header("Ice Move Values")]
    [SerializeField] float iceMultiplier = 6f; //how slippery ice is
    [SerializeField] float iceSlowDownRate = 2f; //rate at which the player slows down
    Vector3 iceVel = Vector3.zero; //stored velocity when on ice

    RaycastHit hit;
    [HideInInspector]
    public Vector3 direction = Vector3.zero;
    Vector3 finalDirection = Vector3.zero; //final direction after direction is passed through an equation
    Vector3 moveVel = Vector3.zero; //velocity applied to rigidbody
    [HideInInspector]
    public Rigidbody rb;

    [Header("")]
    [SerializeField] float groundCheckYDistance = 1f;

    [Header("Camera")]
    public Transform cam;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

    }

    private void FixedUpdate()
    {
        Move();
    }

    private void OnCollisionEnter(Collision collision)
    {
        HandleCheck();
    }

    private void OnCollisionExit(Collision collision)
    {
        HandleCheck();
    }

    /*!
    handles input to direction
    */
    private void OnMovement(InputValue value)
    {
        direction.x = value.Get<Vector2>().x;
        direction.z = value.Get<Vector2>().y;
    }

    private void OnJump(InputValue value)
    {
        HandleCheck();
        if (value.Get<float>() > 0.1f)
        {
            switch (moveState)
            {
                case MoveState.Air:
                    break;
                default:
                    rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                    jumping = true;
                    break;
            }
        }
        else
        {
            jumping = false;
        }
    }

    private void OnSprint(InputValue value)
    {
        currentSprintVal = value.Get<float>() >= 0.1f ? sprintMultiplier : 1;
    }

    /// <summary>
    /// Checks below the player to set move state properly
    /// </summary>
    private void HandleCheck()
    {
        //if spherecast hits
        if (Physics.SphereCast(transform.position, .3f, Vector3.down, out hit, groundCheckYDistance))
        {
            string tag = hit.transform.tag;
            switch (tag)
            {
                case "Ground":
                    SetMoveState(0);
                    break;
                case "Ice":
                    SetMoveState(1);
                    break;
            }
            if (moveState == MoveState.Air && !jumping) //if jump is released midair
            {
                rb.velocity += Vector3.down * 2;
            }
        } //if it doesnt hit
        else
        {
            SetMoveState(2);
        }
    }

    /*!
    handles applying movement to player
    */
    private void Move()
    {
        //if inputing
        if (direction.magnitude >= 0.1f) 
        {
            //calculates angle for player direction to move
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            //gets direction from angle
            finalDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            //checks if you are on ground

            //calculates velocity based on the moveState
            switch (moveState)
            {
                //ground has snappy movement
                case MoveState.Ground:
                    moveVel = (finalDirection.normalized) * moveSpeed * currentSprintVal;
                    moveVel.y = rb.velocity.y;
                    break;

                //ice has floaty slippery movement
                case MoveState.Ice:
                    moveVel = rb.velocity + finalDirection.normalized / iceMultiplier;
                    moveVel.y = rb.velocity.y;
                    moveVel.x = Mathf.Clamp(moveVel.x, -airMaxSpeed, airMaxSpeed);
                    moveVel.z = Mathf.Clamp(moveVel.z, -airMaxSpeed, airMaxSpeed);
                    iceVel = moveVel;
                    break;

                //air has floaty movement
                case MoveState.Air:
                    moveVel = rb.velocity + (finalDirection.normalized * airMultiplier);
                    moveVel.y = rb.velocity.y;
                    moveVel.x = Mathf.Clamp(moveVel.x, -airMaxSpeed, airMaxSpeed);
                    moveVel.z = Mathf.Clamp(moveVel.z, -airMaxSpeed, airMaxSpeed);
                    break;
            }
        } //if not inputing
        else 
        {
            switch (moveState)
            {
                case MoveState.Ground:
                    direction = Vector3.zero;
                    moveVel = Vector3.zero;
                    break;

                case MoveState.Ice:
                    moveVel = iceVel - ((iceVel / iceMultiplier) * iceSlowDownRate);
                    break;

                case MoveState.Air:
                    moveVel = rb.velocity;
                    break;
            }
            moveVel.y = rb.velocity.y;
        }
        //apply velocity
        rb.velocity = moveVel;
    }
    
    public Vector3 FinalDirection
    {
        get
        {
            return finalDirection;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position + Vector3.down, .3f);
    }

}
