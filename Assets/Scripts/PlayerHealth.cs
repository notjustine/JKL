using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public TMP_Text playerHealth;
    
    float maxHealth;
    public float currentHealth;
    
    // damage taken from boss projectiles
    float pDamage = 10f;

    // damage taken from boss charge attack
    float cDamage = 30f;
    
    void Start()
    {
        // player starts with max health
        maxHealth = 100f;
        currentHealth = maxHealth;
    }

    void Update()
    {
        // player runs out of health and dies
        if (currentHealth <= 0){
            Destroy(gameObject); // temporary result
        }
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        // player is hit by an attack
        if (collision.gameObject.tag == "Attack") {
            currentHealth -= pDamage;
            
            // show health on UI
            playerHealth.text = "Player Health: " + currentHealth;
        }

        if(collision.gameObject.tag == "Boss")
        {
            currentHealth -= cDamage;

            // show health on UI
            playerHealth.text = "Player Health: " + currentHealth;
        }
    }
}
