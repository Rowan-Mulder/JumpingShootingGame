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
    public GameObject playerModelLocal;
    public GameObject weapons;
    public GameObject handgunsShadow;
    public Transform handgunsGlobal;
    private Vector3 startingRotation;
    float xRotation = 0f;
    float yRotation = 0f;
    public bool aimingWeapon = true;

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
        startingRotation = transform.localRotation.eulerAngles;
    }

    void Update()
    {
        ApplyRotations();
    }

    void LateUpdate()
    {
        // Applies the yRotation to the player's neckbone.
        playerNeckbone.localRotation = Quaternion.Euler(0f, 0f, yRotation);
        playerNeckboneShadow.localRotation = Quaternion.Euler(0f, 0f, yRotation);

        // Applies the xRotation to the entire player.
        transform.localRotation = Quaternion.Euler(0f, startingRotation.y + xRotation, 0f);

        // Weapons are only visible when aiming (partially necessary to prevent mispositions due to complications of ParticleSystem bug workaround)
        weapons.SetActive(aimingWeapon);
        handgunsShadow.SetActive(aimingWeapon);
        playerModelLocal.SetActive(aimingWeapon);

        if (aimingWeapon) {
            //*/ Pistol aiming
                // Applies the yRotation to the player's lower arm.
                playerUpperArmGlobal.localRotation = playerUpperArmGlobalRotation * Quaternion.Euler(0f + yRotation, 0f, 0f);
                playerUpperArmShadow.localRotation = playerUpperArmShadowRotation * Quaternion.Euler(0f + yRotation, 0f, 0f);

                // Stops animating specific bodyparts for a more steady aim.
                playerLowerArmGlobal.localRotation = playerLowerArmGlobalRotation;
                playerShoulderGlobal.localRotation = playerShoulderGlobalRotation;
                playerSpineGlobal.localRotation = playerSpineGlobalRotation;
                playerLowerArmShadow.localRotation = playerLowerArmShadowRotation;
                playerShoulderShadow.localRotation = playerShoulderShadowRotation;
                playerSpineShadow.localRotation = playerSpineShadowRotation;
            //*/

            /*/ RPG aiming (example)
                // Applies the yRotation to the player's spine.
                playerSpineGlobal.localRotation = playerSpineGlobalRotation * *Quaternion.Euler(0f, 0f + yRotation, 0f);
                // Add Shadow here too
                
                // Stops animating specific bodyparts for a more steady aim.
                playerUpperArmGlobal.localRotation = playerUpperArmGlobalRotation;
                playerShoulderGlobal.localRotation = playerShoulderGlobalRotation;
                // Add LowerArm and Shadow here too
            //*/
        }

        // Workaround for possible Unity bug with varying Execution Order in ParticleSystem. Particles and light would be seperated when programmatically overriding animations. Thread about this issue: https://forum.unity.com/threads/order-of-execution-of-a-particle-system.997364/
        handgunsGlobal.rotation = playerUpperArmGlobal.rotation;
    }

    private void ApplyRotations()
    {
        float mouseX = Input.GetAxis("Cam X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Cam Y") * mouseSensitivity;

        switch (mouseInvertX) {
            case true:
                xRotation -= mouseX;
                break;
            case false:
                xRotation += mouseX;
                break;
        }

        switch (mouseInvertY) {
            case true:
                yRotation += mouseY;
                break;
            case false:
                yRotation -= mouseY;
                break;
        }

        // Limits the yRotation from rotating too far.
        yRotation = Mathf.Clamp(yRotation, -90f, 90f);

        // Applies the yRotation to the player's first-person camera.
        playerFirstPersonCamera.localRotation = Quaternion.Euler(yRotation, 0f, 0f);

        // Applies the yRotation to the player's third-person camera.
        playerThirdPersonCamera.localRotation = Quaternion.Euler(yRotation, 0f, 0f);
    }
}
