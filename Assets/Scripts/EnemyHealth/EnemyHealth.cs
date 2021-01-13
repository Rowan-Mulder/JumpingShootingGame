using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    #pragma warning disable IDE0051 // Removes warning for 'unused' methods (like Awake() and Update())

    public int maxHealthpoints = 10;
    public int currentHealthpoints = 10;
    public bool dead = false;

    void UpdateHealth()
    {
        // May not be needed (yet)
    }

    // Enemy gets shot
    public void Damage(int damage)
    {
        if ((currentHealthpoints -= damage) <= 0) {
            currentHealthpoints = 0;
            Death();
        }

        UpdateHealth();
    }

    private void Death()
    {
        // Activate death animation and destroy current object after some time for performance?

        dead = true;

        Destroy(gameObject);
    }

    void Revive()
    {
        dead = false;
        currentHealthpoints = maxHealthpoints;
        UpdateHealth();
    }
}
