using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//
//Code Written by Edgar Gunther
//

[RequireComponent(typeof(Rigidbody))]
public class ThirdPersonMovementRB : MonoBehaviour
{
    //Variables
    //--------------------------------------
    //camera values
    public Transform cam;
    Vector3 camRotation = Vector3.zero;
    bool camLock = false;

    //movement values
    public float speed = 6f;
    public float airMultiplier = 0.25f;
    [SerializeField] float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    Vector3 inputDirection = Vector3.zero;
    Vector3 moveDirection = Vector3.zero;

    //collision values
    Rigidbody rb;
    bool groundCheck = false;
    Vector3 moveVel = Vector3.zero;
    //--------------------------------------
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        //checks if the camera lock is on
        if (!camLock)
        {
            FreeMovement();
        }
        else
        {
            LockedMovement();
        }

        //checks if on ground and applies velocity based off check
        if (groundCheck)
        {
            moveVel = moveDirection * speed;
            moveVel.y = rb.velocity.y;
            rb.velocity = moveVel;
        }
        else
        {
            moveVel = rb.velocity + (moveDirection * airMultiplier);
            moveVel.y = rb.velocity.y;
            rb.velocity = moveVel;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        string objectTag = collision.gameObject.tag;

        if(objectTag == "Ground")
        {
            groundCheck = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        string objectTag = collision.gameObject.tag;

        if (objectTag == "Ground")
        {
            groundCheck = false;
        }
    }

    private void OnMove(InputValue value)
    {
        inputDirection.x = value.Get<Vector2>().x;
        inputDirection.z = value.Get<Vector2>().y;
    }

    private void OnJump()
    {
        if (groundCheck)
        {
            rb.AddForce(Vector3.up * 10f, ForceMode.Impulse);
        }
    }

    //changes camera mode
    private void OnCamera()
    {
        camLock = camLock ? false : true;
    }

    //free 3d movement where player faces direction of velocity
    private void FreeMovement()
    {
        if (inputDirection.magnitude >= 0.1f)
        {
            //calculates angle for player direction to move and face
            float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            //smoothes the angle transfer between current and target angle
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            //sets the rotation to the current angle
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            //creates a movement direction off of the given angle
            moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward; 
        }
        else
        {
            moveDirection = Vector3.zero;
        }
    }

    //movement where player faces camera forward
    private void LockedMovement()
    {
        if (inputDirection.magnitude >= 0.1f)
        {
            //calculates angle for player direction to move
            float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + cam.eulerAngles.y; 
            //creates a movement direction off of the given angle
            moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward; 
        }
        else
        {
            moveDirection = Vector3.zero;
        }

        camRotation.y = cam.eulerAngles.y;
        camRotation.x = transform.rotation.x;
        camRotation.z = transform.rotation.z;
        transform.rotation = Quaternion.Euler(camRotation);
    }
}
