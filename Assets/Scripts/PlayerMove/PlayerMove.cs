using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using UnityEditor;
//using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMove : MonoBehaviour
{
    /// <summary>
    /// Players may have issues jumping against walls, where the jumps jitter a bit. See: https://answers.unity.com/questions/1696314/charactercontroller-glitches-if-it-reaches-a-ledge.html
    ///     To fix this:
    ///         Adjust the CharacterController its "Slope Limit" to 90
    ///         or
    ///         Set the Step-Offset to 0.
    /// Players may be unable to jump.
    ///     To fix this:
    ///         Check if isGrounded is true.
    ///         The build-in controller.isGrounded doesn't seem to work when standing still, no solutions found on this problem as of yet.
    /// Players may slowly increase falling speed (velocity.y) because it can't detect ground.
    ///     To fix this:
    ///         Make sure the GroundChecker is placed in the center of the rounded bottom of the CharacterController build-in capsule collider and then move it ever so slightly downwards
    ///             (so it won't touch the walls, but it will touch the ground for only the first frame of a jump).
    ///         Also make sure the given radius in the CheckSphere() then matches the radius of the CharacterController.
    /// </summary>

    public Animator animatorGlobal; // Global animations
    public Animator animatorLocal; // Local (shadow-only) animations
    public string currentAnimationState;
    readonly Dictionary<string, int> animatorParameters = new Dictionary<string, int>();
    
    public CharacterController controller;
    public Transform playerCamera;
    public Transform groundChecker;
    public Transform wallChecker;
    public LayerMask collisionMask;

    public float moveX;
    public float moveZ;
    public Vector3 previousFramePosition;
    private float movingSpeed; // Calculated movement speed (walking into walls will affect this speed)
    Vector3 transformedMove;
    Vector3 respawnPoint;
    public bool flightMode;

    public float speed;
    public float crouchingSpeed = 2.5f;
    public float walkSpeed = 5f;
    public float runningSpeed = 10f;
    public float gravity = -30f;
    public float jumpHeight = 3f;
    public float fallingSpeedLimit = 100f;
    public bool autoJumping = false;
    public int walljumpLimit = 3;

    int jumpState = 0;
    // 0 == Jump button has been released and player may now jump.
    // 1 == Currently in a jump, haven't landed nor released jump-button yet.
    // 2 == When autojumping is off, this state will prevent you from jumping until you release the jump button.

    bool isGrounded;
    bool isConnectedToWall;
    bool isCrouching = false;
    public Transform firstPersonCameraPosition;
    private Vector3 characterCapsuleStart;
    private Vector3 characterCapsuleEnd;
    int consecutiveWalljumps;
    int totalJumps;
    int totalWallJumps;
    Vector3 velocity;
    Vector3 wallJumpVelocity;
    //float test1 = 0;
    //float test2 = 200;

    public bool haltedAnimations = false;
    public enum Animations
    {
        Idle,
        WalkForward,
        WalkBackward,
        WalkRight,
        RunForward,
        RunBackward,
        // Note when adding animations and all (or some) animations are not displayed: Save the model. Export the model to .fbx with the right settings. Add the animations, set them to the right time and loop them if required. In the animator, add new animations and make sure old animations are still linked.
        /*  To add an animation:
         *      Blender:
         *          Dope Sheet -> Action Editor
         *              New Action
         *          Save both as project and as .fbx (default export settings seem fine for now)
         *      Unity:
         *          In the Hierarchy, open the inspector of 'PlayerModel global' from the 'Player' object
         *              
         *      C#:
         *          PlayerMove.cs
         *              Add the animation to the enumeration
         *              Uncomment or write the logic
         *                  ChangeAnimationState()
         */
    };

    // Initialises base elements
    private void Awake()
    {
        respawnPoint = transform.position; // This can be updated to be used for a checkpoint system.
        ChangeAnimationState(Animations.Idle);
    }

    // Triggered every frame
    void Update()
    {
        ApplyMovements();
    }

    // Draws some information to the screen for debugging purposes.
    private void OnGUI()
    {
        /*/ Only enable when absolutely required for debugging.
        GUIDebuggingInfo(new string[] {
            $"velocity.x: {moveX}",
            $"velocity.y: {velocity.y}",
            $"velocity.z: {moveZ}",
            $"transformedMove.x: {transformedMove.x}",
            $"transformedMove.z: {transformedMove.z}",
            $"wallJumpVelocity.x: {wallJumpVelocity.x}",
            $"wallJumpVelocity.z: {wallJumpVelocity.z}",
            $"fallingSpeedLimit: {fallingSpeedLimit}",
            $"isGrounded: {isGrounded}",
            $"isConnectedToWall: {isConnectedToWall}",
            $"isCrouching: {isCrouching}",
            $"consecutiveWalljumps: {consecutiveWalljumps}",
            $"totalJumps: {totalJumps}",
            $"totalWallJumps: {totalWallJumps}",
            $"jumpState: {jumpState}"
        });
        //*/
    }

    void ApplyMovements()
    {
        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");

        transformedMove = transform.right * moveX + transform.forward * moveZ;

        // Checks if standing on objects (that are of the selected LayerMask)
        isGrounded = Physics.CheckSphere(groundChecker.position, 0.1f, collisionMask);
        isConnectedToWall = Physics.CheckSphere(wallChecker.position, 0.11f, collisionMask);

        /*/ Experimental movement, building up speed
        if (moveZ > 0 && test1 < test2)
            test1++;
        if (moveZ == 0 && test1 > 0)
            test1 -= 20;
        if (test1 <= 0)
            test1 = 0;
        if (test1 > test2)
            test1 -= 10;

        // Divide test1 by 100 if using it instead of moveZ in transformedMove
        // These experimental movements don't have support for moving backwards yet
        //*/

        if (isGrounded) {
            /* Resets walljump limit
             * Resets fallingspeed
             * Resets velocity from last walljump
             */

            consecutiveWalljumps = 0;
            velocity.y = -2f;
            wallJumpVelocity = Vector3.zero;
        } else
            transformedMove += wallJumpVelocity / 3;

        // Crouching/Standing
        if (Input.GetKey(KeyCode.LeftControl) && !isCrouching) {
            isCrouching = true;
            controller.height = 1.1f;
            controller.center = new Vector3(0, (controller.height / 2), 0);
            playerCamera.position = new Vector3(playerCamera.position.x, (firstPersonCameraPosition.position.y - 0.7f), playerCamera.position.z);
        } else if (!Input.GetKey(KeyCode.LeftControl) && isCrouching) {
            CharacterCapsule();

            if (!Physics.CheckCapsule(characterCapsuleStart, characterCapsuleEnd, controller.radius, collisionMask)) {
                isCrouching = false;
                controller.height = 2.0f;
                controller.center = new Vector3(0, (controller.height / 2), 0);
                playerCamera.position = new Vector3(playerCamera.position.x, (firstPersonCameraPosition.position.y), playerCamera.position.z);
            }
        }

        bool movingLeft = false;
        bool movingRight = false;
        bool movingForwards = false;
        bool movingBackwards = false;
        bool standingStill = false;

        // 0.04 due to idle animation never triggered due to Unity not dealing with controller thumbstick deadzones.
        if (moveX > 0.04)
            movingRight = true;
        else if (moveX < -0.04)
            movingLeft = true;

        if (moveZ > 0.04)
            movingForwards = true;
        else if (moveZ < -0.04)
            movingBackwards = true;

        if (!movingForwards && !movingBackwards && !movingLeft && !movingRight)
            standingStill = true;

        // ADDITIONAL_FEATURE: If crouching, slide instead?
        if (Input.GetKey(KeyCode.LeftShift) && !isCrouching && consecutiveWalljumps == 0) {
            // Sprinting
            speed = runningSpeed;
            //test2 = 400;

            if (!movingRight && !movingLeft) {
                if (movingForwards && isGrounded)
                    ChangeAnimationState(Animations.RunForward);

                if (movingBackwards && isGrounded)
                    ChangeAnimationState(Animations.RunBackward);
            } else {
                //if (movingLeft)
                    //Animate(Animations.RunLeft);

                //if (movingRight)
                    //Animate(Animations.RunRight);
            }
        } else if (isCrouching) {
            // Crouching
            speed = crouchingSpeed;
            //test2 = 100;

            //if (movingForwards && isGrounded)
                //Animate(Animations.CrouchForward);

            //if (movingBackwards && isGrounded)
                //Animate(Animations.CrouchBackward);
        } else {
            // Walking
            speed = walkSpeed;
            //test2 = 200;

            if (!movingRight && !movingLeft) {
                if (movingForwards && isGrounded)
                    ChangeAnimationState(Animations.WalkForward);

                if (movingBackwards && isGrounded)
                    ChangeAnimationState(Animations.WalkBackward);
            } else {
                //if (movingLeft)
                    //Animate(Animations.WalkLeft);

                if (movingRight)
                    ChangeAnimationState(Animations.WalkRight);
            }
        }

        //*/ Flying around the scene - Meant for playtesting.
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (flightMode) {
                velocity.y = -2f;
                flightMode = false;
            } else
                flightMode = true;
        }

        if (flightMode) {
            if (movingForwards)
                controller.Move(playerCamera.forward * moveZ * speed * Time.deltaTime);

            if (movingBackwards)
                controller.Move(-playerCamera.forward * Math.Abs(moveZ) * speed * Time.deltaTime);

            if (movingLeft)
                controller.Move(new Vector3(transformedMove.x, transformedMove.y, transformedMove.z) * speed * Time.deltaTime);

            if (movingRight)
                controller.Move(new Vector3(transformedMove.x, transformedMove.y, transformedMove.z) * speed * Time.deltaTime);

            return;
        }
        //*/

        // Will not idle if specific animations should finish first (using Invoke()). This only applies to full animations, not partial animations (like moving an arm).
        if (standingStill && isGrounded && !haltedAnimations) {
            // ADDITIONAL_FEATURE: Random idle animations, using animator.GetCurrentAnimatorStateInfo(0).length to determine when an idle animation has finished to activate a new one when still idle.

            if (!isCrouching)
                ChangeAnimationState(Animations.Idle);
            //else
                //Animate(Animations.IdleCrouch);
        }

        if (!isGrounded && jumpState == 0) {
            //ChangeAnimationState("Falling");
            //animator.speed = Math.Abs(velocity.y) / fallingSpeedLimit;
        }

        if (Input.GetKeyUp(KeyCode.Space))
            jumpState = 0;

        if (isGrounded && jumpState == 1 && !autoJumping)
            jumpState = 2;

        if (Input.GetKey(KeyCode.Space)) {
            if (isGrounded && jumpState < 2) {
                // Regular jump
                // ADDITIONAL_FEATURE: The longer it's held, the higher your jump will be.
                // INFO: This is triggered between 10 and 20 times on initial jump, then once each reoccuring jump (might be a bug, but it's not a problem yet). Limiting the initial jump to 1 makes the jump extremely weak.
                
                Jump();
                jumpState = 1;
            } else if (isConnectedToWall && consecutiveWalljumps < walljumpLimit && jumpState == 0) {
                // Wall jump
                // ADDITIONAL_FEATURE: When autojumping is enabled and jump peak has been reached, schedule next walljump for when isConnectedToWall is true and isGrounded hasn't been true until then. (Could use wallJumpState like jumpState?)

                WallJump();
                Jump();
                jumpState = 1;
                totalWallJumps++;
                consecutiveWalljumps++;
            }
        }

        // Limits fallingspeed
        if (velocity.y > -fallingSpeedLimit)
            velocity.y += gravity * Time.deltaTime;

        //WallJumpDrag();

        // Movements
        controller.Move(transformedMove * speed * Time.deltaTime);

        // Jumps
        controller.Move(velocity * Time.deltaTime);

        //*/ Testing sliding ice movements. Try calculating which direction the player is moving to with only X and Y movements.
        //controller.Move(new Vector3(0.0f, 0f, 0f));
        //*/

        // On pressing 'R', it will teleport you to the original set position
        if (Input.GetKey(KeyCode.R))
            transform.position = respawnPoint;

        // Calculates movement speed from displacement
        float movementPerFrame = Vector3.Distance(previousFramePosition, transform.position);
        movingSpeed = movementPerFrame / Time.deltaTime;
        previousFramePosition = transform.position;
    }

    private void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        totalJumps++;
        //ChangeAnimationState("Jump");
    }

    private void WallJump()
    {
        RaycastHit hit;

        if (Physics.Raycast(wallChecker.position, transformedMove, out hit)) {
            var reflectedDirection = Vector3.Reflect(transformedMove, hit.normal);
            wallJumpVelocity = reflectedDirection * 2;
        }
    }

    private void WallJumpDrag()
    {
        wallJumpVelocity.x = (wallJumpVelocity.x < 0.001f) ? 0 : wallJumpVelocity.x / 1.001f;
        wallJumpVelocity.z = (wallJumpVelocity.z < 0.001f) ? 0 : wallJumpVelocity.z / 1.001f;
    }

	// Preperation needed to calculate collisions for the character capsule when un-crouching.
    private void CharacterCapsule()
    {
        characterCapsuleStart = new Vector3(transform.position.x, (transform.position.y + 0.1f), transform.position.z);
        characterCapsuleEnd = new Vector3(transform.position.x, ((transform.position.y + 2f) - 0.1f), transform.position.z);
    }

    // Activeert animaties voor het huidige object met dit script. Voor uitvoering van animaties na een bepaalde tijd, gebruik: Invoke("SomeMethodWithoutUsingParams", 0.5f);
    public void ChangeAnimationState(Animations animation)
    {
        switch (animation) {
            case Animations.WalkForward:
            case Animations.WalkBackward:
            case Animations.WalkRight:
            //case Animations.WalkLeft:
                animatorGlobal.speed = movingSpeed / walkSpeed;
                animatorLocal.speed = movingSpeed / walkSpeed;
                break;
            case Animations.RunForward:
            case Animations.RunBackward:
            //case Animations.RunRight:
            //case Animations.RunLeft:
                animatorGlobal.speed = movingSpeed / runningSpeed;
                animatorLocal.speed = movingSpeed / runningSpeed;
                break;
            //case Animations.CrouchForward:
            //case Animations.CrouchBackward:
            //case Animations.CrouchRight:
            //case Animations.CrouchLeft:
            //animatorGlobal.speed = movingSpeed / crouchingSpeed;
            //animatorLocal.speed = movingSpeed / crouchingSpeed;
            //break;
            case Animations.Idle:
                break;
            default:
                Debug.Log($"ChangeAnimationState() couldn't handle animation: \"{animation}\"");
                break;
        }

        string newAnimationState = animation.ToString();

        if (currentAnimationState != newAnimationState) {
            animatorGlobal.speed = 1.0f;
            animatorLocal.speed = 1.0f;
            animatorGlobal.Play(newAnimationState);
            animatorLocal.Play(newAnimationState);
            currentAnimationState = newAnimationState;
        }
    }

    /*/ Loads the Animator Parameters. This method is no longer used.
    public void LoadAnimatorParameters()
    {
        bool continues = true;

        for (int i = 0; continues; i++) {
            try {
                animatorParameters.Add(animator.GetParameter(i).name, i);
                //Debug.Log("ADDED     Name:" + animator.GetParameter(i).name + " Index:" + i);
            } catch (IndexOutOfRangeException) {
                continues = false;
            }
        }
    }
    //*/

    private void GUIDebuggingInfo(string[] debugInfo)
    {
        GUI.color = UnityEngine.Color.black;
        
        for (int i = 0; i < debugInfo.Length; i++)
            GUI.Label(new Rect(140, 100 + (i * 15), 200, 25), debugInfo[i]);
    }
}
