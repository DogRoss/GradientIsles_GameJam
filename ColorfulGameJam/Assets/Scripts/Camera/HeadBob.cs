using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ScreenShake))]
[RequireComponent(typeof(FirstPersonMovementRB))]
public class HeadBob : MonoBehaviour
{
    [Header("Head Bobbing Values")]
    ScreenShake screenShake;
    FirstPersonMovementRB fpMovement;
    [SerializeField] float bobAmount = 0.5f;
    [SerializeField] float bobRate = 0.5f;
    float timer = 0f;
    Transform cam;
    [SerializeField] Transform camOrigin;

    // Start is called before the first frame update
    void Start()
    {
        screenShake = GetComponent<ScreenShake>();
        fpMovement = GetComponent<FirstPersonMovementRB>();
        cam = fpMovement.cam;

    }

    // Update is called once per frame
    void Update()
    {

        
        if (fpMovement.moveState == Move3DStateMachine.MoveState.Ground || fpMovement.moveState == Move3DStateMachine.MoveState.Ice)
        {
            if (!screenShake.IsShaking())
            {
                if (fpMovement.direction.magnitude > 0.1f)
                {
                    timer += Time.deltaTime * bobRate;
                    cam.localPosition = new Vector3(cam.localPosition.x, camOrigin.localPosition.y + Mathf.Sin(timer) * bobAmount, cam.localPosition.z);
                }
                else
                {
                    timer = 0;
                    cam.localPosition = new Vector3(cam.localPosition.x, Mathf.Lerp(cam.localPosition.y, camOrigin.localPosition.y, Time.deltaTime * bobRate), cam.localPosition.z);
                }
            }
            else
            {
                cam.localPosition = camOrigin.localPosition;
            }
        }
        else
        {
            timer = 0;
            cam.localPosition = new Vector3(cam.localPosition.x, Mathf.Lerp(cam.localPosition.y, camOrigin.localPosition.y, Time.deltaTime * bobRate), cam.localPosition.z);
        }
        
    }
}
