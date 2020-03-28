﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{
    public Slider healthBar;
    public Gradient gradient;
    public Image fill;

    public void setMaxHealth(float health)
    {
        healthBar.maxValue = health;
        healthBar.value = health;
        fill.color = gradient.Evaluate(1f);
    }

    public void setHealth(float health)
    {        
        if (healthBar.value >= 0)
        {
            healthBar.value -= health;
            fill.color = gradient.Evaluate(healthBar.normalizedValue);
        }
    }

    public void set(float health)
    {
        healthBar.value = health;
        fill.color = gradient.Evaluate(healthBar.normalizedValue);
    }

}