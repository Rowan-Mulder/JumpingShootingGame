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

    public Vector3 moveDirection;
    public NavMeshAgent agent;
    public bool followingGivenDirections = false;

    void Update()
    {
        // Add some pseudocode to structure logic, for the responding to detection and walking (partially) towards the player.

        if (followingGivenDirections)
            agent.SetDestination(moveDirection);
    }
}
