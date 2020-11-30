using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
    /// <summary>
    /// Vaak voorkomen problemen bij NavMesh AI:
    ///     "SetDestination" can only be called on an active agent that has been placed on a NavMesh.
    ///         Bij gebruik van custom Agent profielnamen (buiten default "Humanoid") moet je schijnbaar NavMeshSurface gebruiken. Makkelijker is om de standaard "Humanoid" Agent te gebruiken.
    ///         
    /// </summary>

    public Transform moveDirection;
    public NavMeshAgent agent;
    public bool movingToPlayer = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		// Add some pseudocode to structure logic, for the responding to detection and walking (partially) towards the player.
		
        if (movingToPlayer)
        {
            agent.SetDestination(moveDirection.position);
        }
    }
}
