using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public TMP_Text playerHealth;
    
    float maxHealth;
    public float currentHealth;
    
    // damage taken from boss' attack
    float damage = 20f;
    
    void Start()
    {
        // player starts with max health
        maxHealth = 100f;
        currentHealth = maxHealth;
    }

    void Update()
    {
        // player runs out of health and dies
        if (currentHealth <= 10){
            Destroy(gameObject); // temporary result
        }
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        // boss is hit by an attack
        if (collision.gameObject.tag == "Attack") {
            currentHealth -= damage;
            
            // show health on UI
            playerHealth.text = "Player Health: " + currentHealth;
        }
    }
}
