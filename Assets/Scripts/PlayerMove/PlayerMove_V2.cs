using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove_V2 : MonoBehaviour
{
    public Rigidbody rigid;
    public Transform groundChecker;
    public LayerMask collisionMask;
    float moveX;
    float moveZ;

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

    int totalJumps;

    bool isGrounded;
    Vector3 velocity;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ApplyMovements();
    }

    void ApplyMovements()
    {
        isGrounded = Physics.CheckSphere(groundChecker.position, 0.10f, collisionMask);

        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");

        transformedMove = transform.right * moveX + transform.forward * moveZ;

        //if (isGrounded)
        //{
        //    velocity.y = -2f;
        //}

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
            }/*
            else if (isConnectedToWall && consecutiveWalljumps < walljumpLimit && jumpState == 0)
            {
                // Wall jump
                // TODO: Prevent reaching walljump limit with one single jump by doing checkups each frame, like a normal jump. (watch out for when autojumping is enabled it should still work)

                WallPush();
                Jump();
                jumpState = 1;
                totalWallJumps++;
                consecutiveWalljumps++;
            }*/
        }

        //rigid.AddForce(transformedMove * speed * Time.deltaTime);

        // Movement
        //rigid.MovePosition(transformedMove * speed * Time.deltaTime);

        // Jumps
        rigid.velocity = (velocity );
    }

    void Jump()
    {
        velocity.y += jumpHeight;
        totalJumps++;
    }

    private void OnGUI()
    {
        GUIDebuggingInfo(new string[] {
            $"velocity.x: {rigid.velocity.x}",
            $"velocity.y: {rigid.velocity.y}",
            $"velocity.z: {rigid.velocity.z}",
            $"moveX: {moveX}",
            $"moveZ: {moveZ}",
            $"totalJumps: {totalJumps}"
        });
    }

    private void GUIDebuggingInfo(string[] debugInfo)
    {
        GUI.color = Color.black;

        for (int i = 0; i < debugInfo.Length; i++)
        {
            GUI.Label(new Rect(140, 100 + (i * 15), 200, 25), debugInfo[i]);
        }
    }
}
