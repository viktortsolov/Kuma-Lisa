using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image fillBar;
    public float health;

    public void LoseHealth(int value)
    {
        //Do nothing if your health is zero
        if (health <= 0)
        {
            return;
        }
        //Reduce the health
        health -= value;
        //Resfresh the ui fillBar
        fillBar.fillAmount = health / 100;
        //Check if your health is zero or less => Dead
        if (health <= 0)
        {
            FindObjectOfType<Fox>().Die();
        }
    }

    private void Update()
    {

    }
}
