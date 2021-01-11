using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    #pragma warning disable IDE0051 // Removes warning for 'unused' methods (like Awake() and Update())
    
    public float mouseSensitivity = 5f;
    public bool mouseInvertX = false;
    public bool mouseInvertY = false;
    public Transform playerFirstPersonCamera;
    public Transform playerThirdPersonCamera;
    public Transform playerNeckbone;
    public Transform playerNeckboneShadow;
    public Transform playerSpineGlobal;
    public Transform playerSpineShadow;
    public Transform playerUpperArmLocal;
    public Transform playerLowerArmGlobal;
    public Transform playerUpperArmGlobal;
    public Transform playerShoulderGlobal;
    public Transform playerLowerArmShadow;
    public Transform playerUpperArmShadow;
    public Transform playerShoulderShadow;
    private Quaternion playerLowerArmGlobalRotation;
    private Quaternion playerUpperArmGlobalRotation;
    private Quaternion playerShoulderGlobalRotation;
    private Quaternion playerLowerArmShadowRotation;
    private Quaternion playerUpperArmShadowRotation;
    private Quaternion playerShoulderShadowRotation;
    private Quaternion playerSpineGlobalRotation;
    private Quaternion playerSpineShadowRotation;
    float xRotation = 0f;
    float yRotation = 0f;
    bool aimingWeapon = true;

    // Start is called before the first frame update
    void Start()
    {
        // Lock the cursor in the center of the window
        Cursor.lockState = CursorLockMode.Locked;

        playerLowerArmGlobalRotation = playerLowerArmGlobal.localRotation;
        playerUpperArmGlobalRotation = playerUpperArmGlobal.localRotation;
        playerShoulderGlobalRotation = playerShoulderGlobal.localRotation;
        playerLowerArmShadowRotation = playerLowerArmShadow.localRotation;
        playerUpperArmShadowRotation = playerUpperArmShadow.localRotation;
        playerShoulderShadowRotation = playerShoulderShadow.localRotation;
        playerSpineGlobalRotation = playerSpineGlobal.localRotation;
        playerSpineShadowRotation = playerSpineShadow.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        ApplyRotations();
    }

    // LateUpdate is called once per frame with lower priority than Update
    void LateUpdate()
    {
        // Applies the xRotation to the player's neckbone.
        playerNeckbone.localRotation = Quaternion.Euler(0f, 0f, xRotation);
        playerNeckboneShadow.localRotation = Quaternion.Euler(0f, 0f, xRotation);

        // Applies the yRotation to the entire player.
        transform.localRotation = Quaternion.Euler(0f, yRotation, 0f);

        if (aimingWeapon) {
            //*/ Pistol aiming
                // Applies the xRotation to the player's lower arm.
                playerUpperArmGlobal.localRotation = playerUpperArmGlobalRotation * Quaternion.Euler(0f + xRotation, 0f, 0f);
                playerUpperArmShadow.localRotation = playerUpperArmShadowRotation * Quaternion.Euler(0f + xRotation, 0f, 0f);

                // Stops animating specific bodyparts for a more steady aim.
                playerLowerArmGlobal.localRotation = playerLowerArmGlobalRotation;
                playerShoulderGlobal.localRotation = playerShoulderGlobalRotation;
                playerSpineGlobal.localRotation = playerSpineGlobalRotation;
                playerLowerArmShadow.localRotation = playerLowerArmShadowRotation;
                playerShoulderShadow.localRotation = playerShoulderShadowRotation;
                playerSpineShadow.localRotation = playerSpineShadowRotation;
            //*/

            /*/ RPG aiming (example)
                // Applies the xRotation to the player's spine.
                playerSpineGlobal.localRotation = playerSpineGlobalRotation * *Quaternion.Euler(0f, 0f + xRotation, 0f);
                // Add Shadow here too
                
                // Stops animating specific bodyparts for a more steady aim.
                playerUpperArmGlobal.localRotation = playerUpperArmGlobalRotation;
                playerShoulderGlobal.localRotation = playerShoulderGlobalRotation;
                // Add LowerArm and Shadow here too
            //*/
        }
    }

    void ApplyRotations()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        switch (mouseInvertX) {
            case true:
                yRotation -= mouseX;
                break;
            case false:
                yRotation += mouseX;
                break;
        }

        switch (mouseInvertY) {
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
    }
}
