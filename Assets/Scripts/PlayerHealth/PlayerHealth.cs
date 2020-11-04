using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealthpoints = 100;
    public int currentHealthpoints = 100;
    public Text TextHealthpoints;

    // Start is called before the first frame update
    void Start()
    {
        TextHealthpoints.text = $"{currentHealthpoints}/{maxHealthpoints}";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage(int damage)
    {
        if ((currentHealthpoints -= damage) <= 0)
        {
            currentHealthpoints = 0;
            Death();
        }

        TextHealthpoints.text = $"{currentHealthpoints}/{maxHealthpoints}";
    }

    void Death()
    {
        // Wait 5 seconds before reset to last checkpoint
        // Display some UI?
        // Blur/darken the screen?


    }
}
