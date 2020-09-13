using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMove : MonoBehaviour
{
    /// <summary>
    /// Players may have issues jumping against walls, where the jumps jitter a bit.
    ///     To fix this:
    ///         Adjust the CharacterController its "Slope Limit" to 90.
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

    public CharacterController controller;
    public Transform playerCamera;
    public Transform groundChecker;
    public Transform wallChecker;
    public LayerMask collisionMask;

    public float moveX;
    public float moveZ;
    Vector3 transformedMove;

    public float speed;
    public float walkSpeed = 5f;
    public float runningSpeed = 10f;
    public float gravity = -30f;
    public float jumpHeight = 2f;
    public float velocityLimitY = 100f;
    public bool autoJumping = false;

    int jumpState = 0;
    // 0 == Jump button has been released and player may now jump.
    // 1 == Currently in a jump, haven't landed yet.
    // 2 == When autojumping is off, this state will prevent you from jumping until you release the jump button.

    bool isGrounded;
    bool isConnectedToWall;
    bool isCrouching = false;
    float standingHeight;
    float crouchingHeight;
    int consecutiveWalljumps;
    int walljumpLimit = 3;
    int totalJumps;
    int totalWallJumps;
    Vector3 velocity;
    Vector3 wallJumpVelocity;



    // Initialises base elements
    private void Awake()
    {
        standingHeight = playerCamera.position.y;
        crouchingHeight = (standingHeight - 1f);
    }

    // Update is called once per frame
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
            $"velocityLimitY: {velocityLimitY}",
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
        isGrounded = Physics.CheckSphere(groundChecker.position, 0.10f, collisionMask);
        // May not continue on wallrunning feature, but just walljumping!
        isConnectedToWall = Physics.CheckSphere(wallChecker.position, 0.11f, collisionMask);

        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");

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
            transformedMove += wallJumpVelocity;
        }

        // Sprinting/Walking
        if (Input.GetKey(KeyCode.LeftShift) && consecutiveWalljumps == 0)
        {
            speed = runningSpeed;
        }
        else
        {
            speed = walkSpeed;
        }

        // Crouching/Standing
        // TODO: do collisionchecking before standing back up
        if (Input.GetKey(KeyCode.LeftControl))
        {
            // TODO: crouchingHeight
            // TODO: playerCamera.position = ;
            isCrouching = true;
        }
        else
        {
            // TODO: standingheight
            // TODO: playerCamera.position = ;
            isCrouching = false;
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
        if (velocity.y > -velocityLimitY)
            velocity.y += gravity * Time.deltaTime;

        WallJumpDrag();

        // Movements
        controller.Move(transformedMove * speed * Time.deltaTime);

        // Jumps
        controller.Move(velocity * Time.deltaTime);

        //*/ Testing sliding ice movements. Try calculating which direction the player is moving to with only X and Y movements.
        //controller.Move(new Vector3(0.0f, 0f, 0f));
        //*/
    }

    private void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        totalJumps++;
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
        wallJumpVelocity.x = (wallJumpVelocity.x < 0.001f) ? 0 : wallJumpVelocity.x / 1.0001f;
        wallJumpVelocity.z = (wallJumpVelocity.z < 0.001f) ? 0 : wallJumpVelocity.z / 1.0001f;
    }

    private void GUIDebuggingInfo(string[] debugInfo)
    {
        GUI.color = UnityEngine.Color.black;
        
        for (int i = 0; i < debugInfo.Length; i++)
        {
            GUI.Label(new Rect(140, 100 + (i * 15), 200, 25), debugInfo[i]);
        }
    }
}
