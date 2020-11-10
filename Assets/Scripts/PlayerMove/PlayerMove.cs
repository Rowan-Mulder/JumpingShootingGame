using System;
using System.Collections;
using System.Collections.Generic;
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

    public Animator animator;
    public string currentAnimationState;
    Dictionary<string, int> animatorParameters = new Dictionary<string, int>();
    
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

    public float speed;
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
    float standingHeight;
    float crouchingHeight;
    int consecutiveWalljumps;
    int totalJumps;
    int totalWallJumps;
    Vector3 velocity;
    Vector3 wallJumpVelocity;
    //float test1 = 0;
    //float test2 = 200;



    // Initialises base elements
    private void Awake()
    {
        standingHeight = playerCamera.position.y; // Kan beter CharacterController.Height wezen en crouchingHeight is ongeveer de helft hiervan.
        crouchingHeight = (standingHeight - 1f);
        respawnPoint = transform.position;
        ChangeAnimationState("Idle");
    }

    // Triggered every frame
    void Update()
    {
        ApplyMovements();
    }

    // Draws some information to the screen for debugging purposes.
    private void OnGUI()
    {
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
    }

    void ApplyMovements()
    {
        // Checks if standing on objects (that are of the selected LayerMask)
        isGrounded = Physics.CheckSphere(groundChecker.position, 0.1f, collisionMask);
        isConnectedToWall = Physics.CheckSphere(wallChecker.position, 0.11f, collisionMask);

        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");

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

        transformedMove = transform.right * moveX + transform.forward * moveZ;

        if (isGrounded)
        {
            /* Resets walljump limit
             * Resets fallingspeed
             * Resets velocity from last walljump
             */

            consecutiveWalljumps = 0;
            velocity.y = -2f;
            wallJumpVelocity = Vector3.zero;
        }
        else
        {
            transformedMove += wallJumpVelocity / 3;
        }

        // Crouching/Standing
        // TODO: do collisionchecking before standing back up
        if (Input.GetKey(KeyCode.LeftControl))
        {
            // TODO: crouchingHeight or CharacterController.Height
            // TODO: playerCamera.position = ;
            isCrouching = true;
        }
        else
        {
            // TODO: standingheight or CharacterController.Height / 2
            // TODO: playerCamera.position = ;
            isCrouching = false;
        }

        bool movingLeft = false;
        bool movingRight = false;
        bool movingForwards = false;
        bool movingBackwards = false;
        bool standingStill = false;

        if (moveX > 0)
        {
            movingRight = true;
        }
        else if (moveX < 0)
        {
            movingLeft = true;
        }

        if (moveZ > 0)
        {
            movingForwards = true;
        }
        else if (moveX < 0)
        {
            movingBackwards = true;
        }

        if (movingForwards == false && movingBackwards == false && movingLeft == false && movingRight == false)
        {
            standingStill = true;
        }

        // If crouching, slide instead?
        // Sprinting/Walking
        if (Input.GetKey(KeyCode.LeftShift) && consecutiveWalljumps == 0)
        {
            speed = runningSpeed;
            //test2 = 400;

            if (movingForwards && isGrounded)
            {
                ChangeAnimationState("RunForward");
                animator.speed = movingSpeed / runningSpeed;
            }

            if (movingBackwards && isGrounded)
            {
                //ChangeAnimationState("RunBackward");
                animator.speed = movingSpeed / runningSpeed;
            }
        }
        else
        {
            speed = walkSpeed;
            //test2 = 200;

            if (movingForwards && isGrounded)
            {
                ChangeAnimationState("WalkForward");
                animator.speed = movingSpeed / walkSpeed;
            }

            if (movingBackwards && isGrounded)
            {
                //ChangeAnimationState("WalkBackward");
                animator.speed = movingSpeed / walkSpeed;
            }
        }

        if (standingStill && isGrounded)
        {
            // Random idle animations?
            ChangeAnimationState("Idle");
        }

        if (!isGrounded && jumpState == 0)
        {
            //ChangeAnimationState("Falling");
            //animator.speed = Math.Abs(velocity.y) / fallingSpeedLimit;
        }

        if (Input.GetKeyUp(KeyCode.Space))
            jumpState = 0;

        if (isGrounded && jumpState == 1 && !autoJumping)
            jumpState = 2;

        if (Input.GetKey(KeyCode.Space))
        {
            if (isGrounded && jumpState < 2)
            {
                // Regular jump
                // TODO: The longer it's held, the higher your jump will be.
                // TIP: This is triggered between 10 and 20 times on initial jump, then once each reoccuring jump (might be a bug, but it's not a problem yet). Limiting the initial jump to 1 makes the jump extremely weak.
                
                Jump();
                jumpState = 1;
            }
            else if (isConnectedToWall && consecutiveWalljumps < walljumpLimit && jumpState == 0)
            {
                // Wall jump
                // TODO: Prevent reaching walljump limit with one single jump by doing checkups each frame, like a normal jump. (watch out for when autojumping is enabled it should still work)

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

        if (Physics.Raycast(wallChecker.position, transformedMove, out hit))
        {
            var reflectedDirection = Vector3.Reflect(transformedMove, hit.normal);
            wallJumpVelocity = reflectedDirection * 2;
        }
    }

    private void WallJumpDrag()
    {
        wallJumpVelocity.x = (wallJumpVelocity.x < 0.001f) ? 0 : wallJumpVelocity.x / 1.001f;
        wallJumpVelocity.z = (wallJumpVelocity.z < 0.001f) ? 0 : wallJumpVelocity.z / 1.001f;
    }

    public void ChangeAnimationState(string newAnimationState)
    {
        if (currentAnimationState != newAnimationState)
        {
            animator.speed = 1.0f;
            animator.Play(newAnimationState);
            currentAnimationState = newAnimationState;
        }
    }

    /*/ Loads the Animator Parameters. Can't later set these parameters, so now using: animator.speed
    public void LoadAnimatorParameters()
    {
        bool continues = true;

        for (int i = 0; continues; i++)
        {
            try
            {
                animatorParameters.Add(animator.GetParameter(i).name, i);
                //Debug.Log("ADDED     Name:" + animator.GetParameter(i).name + " Index:" + i);
            }
            catch (IndexOutOfRangeException)
            {
                continues = false;
            }
        }
    }
    //*/

    private void GUIDebuggingInfo(string[] debugInfo)
    {
        GUI.color = UnityEngine.Color.black;
        
        for (int i = 0; i < debugInfo.Length; i++)
        {
            GUI.Label(new Rect(140, 100 + (i * 15), 200, 25), debugInfo[i]);
        }
    }
}
