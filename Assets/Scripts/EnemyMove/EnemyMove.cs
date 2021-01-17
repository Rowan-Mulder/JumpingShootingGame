using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
    /*  Vaak voorkomen problemen bij NavMesh AI:
     *      "SetDestination" can only be called on an active agent that has been placed on a NavMesh.
     *          Bij gebruik van custom Agent profielnamen (buiten default "Humanoid") moet je schijnbaar NavMeshSurface gebruiken.
     *              Makkelijker is om de standaard "Humanoid" Agent te gebruiken.
     */
    #pragma warning disable IDE0051 // Removes warning for 'unused' methods (like Awake() and Update())

    public Transform playerLocation;
    public Transform playerHead;
    public Vector3 moveDirection;
    public NavMeshAgent agent;
    public bool followingGivenDirections = false;
    public Transform enemyHead;
    public float maxPlayerSigthDistance = 30f;
    private bool playerInSight = false;

    void Start()
    {
        // Repeats a function call every second after a delay of 0.5 seconds.
        InvokeRepeating("LookForPlayer", 0.5f, 1f);
    }

    void Update()
    {
        MovementDecider();

        /*/ Debugging - Visualize whether enemies can see you or not, and if not, what they can see instead.
        if (name == "Enemy_1" && Physics.Raycast(enemyViewpoint.position, playerHead.position - enemyViewpoint.position, out RaycastHit hit, 10f))
            if (LayerMask.LayerToName(hit.transform.gameObject.layer) == "PlayerGeneral")
                Debug.DrawLine(enemyViewpoint.position, enemyViewpoint.position + ((playerHead.position - enemyViewpoint.position).normalized * hit.distance), Color.green);
            else {
                Debug.DrawLine(enemyViewpoint.position, enemyViewpoint.position + ((playerHead.position - enemyViewpoint.position).normalized * hit.distance), Color.red);
                Debug.Log($"I can't see the player, but I can see: {LayerMask.LayerToName(hit.transform.gameObject.layer)}");
            }
        //*/
    }

    private void LookForPlayer()
    {
        Vector3 enemySigth = enemyHead.position;
        Vector3 direction = playerHead.position - enemySigth;

        if (Physics.Raycast(enemySigth, direction, out RaycastHit hit, maxPlayerSigthDistance)) {
            if (LayerMask.LayerToName(hit.transform.gameObject.layer) == "PlayerGeneral")
                playerInSight = true;
            else
                playerInSight = false;
        }
    }

    private void MovementDecider()
    {
        if (playerInSight) {
            if (/*!isPlayerCloseEnough*/ true) {
                agent.SetDestination(playerLocation.position);
                followingGivenDirections = false;
            }
        } else {
            if (followingGivenDirections)
                agent.SetDestination(moveDirection);
        }
    }
}
