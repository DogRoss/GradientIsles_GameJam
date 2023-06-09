using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//
//Code Written by Edgar Gunther
//

public class ThirdPersonMovementCC : MonoBehaviour
{
    //Variables
    //--------------------------------------
    //Character controller objects
    public CharacterController controller;
    public Transform cam;

    //movement values
    public float speed = 6f;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    Vector3 direction = Vector3.zero;
    //--------------------------------------

    private void OnMove(InputValue value)
    {
        direction.x = value.Get<Vector2>().x;
        direction.z = value.Get<Vector2>().y;
    }

    // Update is called once per frame
    void Update()
    {

        if (direction.magnitude >= 0.1f)
        {
            //calculates angle for player direction to move and face
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            //smoothes the angle transfer between current and target angle
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime); 
            //sets the rotation to the current angle
            transform.rotation = Quaternion.Euler(0f, angle, 0f); 

            //gets the movement direction off the angle of intended direction
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            //moves character
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
        
    }
}
