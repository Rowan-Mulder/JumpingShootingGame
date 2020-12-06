﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealthpoints = 100;
    public int currentHealthpoints = 100;
    public Text TextHealthpoints;
    public Slider healthSlider;
    public bool dead = false;

    // Start is called before the first frame update
    void Start()
    {
        UpdateHealth();
    }

    void UpdateHealth()
    {
        TextHealthpoints.text = $"{currentHealthpoints}/{maxHealthpoints}";
        healthSlider.value = currentHealthpoints;
    }

    public void Damage(int damage)
    {
        if ((currentHealthpoints -= damage) <= 0) {
            currentHealthpoints = 0;
            Death();
        }

        UpdateHealth();
    }

    void Death()
    {
        // Wait 5 seconds before reset to last checkpoint
        // Display some UI?
        // Blur/darken the screen?

        dead = true;
    }

    void Revive()
    {
        dead = false;
        currentHealthpoints = maxHealthpoints;
        UpdateHealth();
    }
}
