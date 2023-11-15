using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public TMP_Text playerHealth;
    
    float maxHealth;
    public float currentHealth;

    public Inputs_Lucy_Test inputsLucy;
    
    // damage taken from boss projectiles
    float pDamage = 10f;
    private float pDamageTemp;

    // damage taken from boss charge attack
    float cDamage = 25f;
    private float cDamageTemp;
    private bool hasReducedDamage = false;

    [SerializeField] private float reductionValue = 0.5f;
 
    
    void Start()
    {
        // player starts with max health
        maxHealth = 100f;
        currentHealth = maxHealth;

        pDamageTemp = pDamage;
        cDamageTemp = cDamage;

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

            if (inputsLucy.damageReduction == true && !hasReducedDamage)
            {
                pDamageTemp *= reductionValue;
                hasReducedDamage = true;
            }
            else
            {
                pDamageTemp = pDamage;
            }

            currentHealth -= pDamage;
            
            // show health on UI
            playerHealth.text = "Player Health: " + currentHealth;
        }

        if(collision.gameObject.tag == "Boss")
        {
            if (inputsLucy.damageReduction == true && !hasReducedDamage)
            {
                cDamageTemp *= reductionValue;
                hasReducedDamage = true;
            }
            else
            {
                cDamageTemp = cDamage;
            }

            currentHealth -= cDamageTemp;

            // show health on UI
            playerHealth.text = "Player Health: " + currentHealth;
        }
    }
}
