using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    public Transform playerBody;

    [Header("Mouse Settings")]
    public float mouseSensitivity = 1f;
    //direction mouse moved towards
    Vector2 mouseDirection = Vector2.zero;
    float xRotation;
    public float verticalLookClamp = 90f;

    [HideInInspector]
    public bool mouseLock = false;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (!mouseLock)
        {
            mouseDirection.x = Mouse.current.delta.x.ReadValue() * mouseSensitivity;
            mouseDirection.y = Mouse.current.delta.y.ReadValue() * mouseSensitivity;

            xRotation -= mouseDirection.y;
            xRotation = Mathf.Clamp(xRotation, -verticalLookClamp, verticalLookClamp);

            playerBody.Rotate(Vector3.up * mouseDirection.x);
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }
    }
}
