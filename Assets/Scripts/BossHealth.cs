using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BossHealth : MonoBehaviour
{
    public TMP_Text bossHealth;
    
    float maxHealth;
    public float currentHealth;
    
    // damage taken from player's attack
    float damage = 5f; 

    void Start()
    {
        // boss starts with max health
        maxHealth = 100f;
        currentHealth = maxHealth;
    }

    void Update()
    {
        // boss runs out of health and dies
        if (currentHealth <= 0){
            Destroy(gameObject); // temporary result
        }
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        // boss is hit by an attack
        if (collision.gameObject.tag == "Attack") {
            currentHealth -= damage;
            
            // show health on UI
            bossHealth.text = "Boss Health: " + currentHealth;
        }
    }
}
