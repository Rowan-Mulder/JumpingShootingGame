﻿using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public float mouseSensitivity = 1000f;
    public bool mouseInvertX = false;
    public bool mouseInvertY = false;
    public Transform playerFirstPersonCamera;
    public Transform playerThirdPersonCamera;
    public Transform playerNeckbone;
    public Transform playerUpperArmGlobal;
    public Transform playerUpperArmLocal;
    private Quaternion playerUpperArmGlobalRotation;
    float xRotation = 0f;
    float yRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        // Lock the cursor in the center of the window
        Cursor.lockState = CursorLockMode.Locked;

        playerUpperArmGlobalRotation = playerUpperArmGlobal.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        ApplyRotations();
    }

    void ApplyRotations()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        switch (mouseInvertX)
        {
            case true:
                yRotation -= mouseX;
                break;
            case false:
                yRotation += mouseX;
                break;
        }

        switch (mouseInvertY)
        {
            case true:
                xRotation += mouseY;
                break;
            case false:
                xRotation -= mouseY;
                break;
        }

        // Limits the xRotation from rotating too far.
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Applies the xRotation to the player's first-person camera.
        playerFirstPersonCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Applies the xRotation to the player's third-person camera.
        playerThirdPersonCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Applies the xRotation to the player's neckbone.
        playerNeckbone.localRotation = Quaternion.Euler(0f, 0f, xRotation);

        // Applies the xRotation to the player's global lower arm.
        playerUpperArmGlobal.localRotation = playerUpperArmGlobalRotation * Quaternion.Euler(0f + xRotation, 0f, 0f);

        // Applies the xRotation to the player's local lower arm.
        //playerUpperArmLocal.localRotation = Quaternion.Euler(0f + xRotation, 0f, 0f);

        // Applies the yRotation to the player object.
        transform.localRotation = Quaternion.Euler(0f, yRotation, 0f);
    }
}
