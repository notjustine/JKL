using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public TMP_Text playerHealth;
    
    float maxHealth;
    public float currentHealth;

    //public Inputs_Lucy_Test inputsLucy;
    
    // damage taken from boss projectiles
    float pDamage = 10f;
    private float pDamageTemp;

    // damage taken from boss charge attack
    float cDamage = 25f;
    [SerializeField] private float cDamageTemp;

    Animator lucyAnimator; //Referencing the animator as LucyAnimator

    //private bool hasReducedDamage = false;
    //private float reductionValue = 0.5f;
 
    
    void Start()
    {
        // player starts with max health
        maxHealth = 100f;
        currentHealth = maxHealth;

        pDamageTemp = pDamage;
        cDamageTemp = cDamage;

        //
        lucyAnimator = GetComponent<Animator>();//Getting component of the Animator

    }

    void Update()
    {
        // player runs out of health and dies
        if (currentHealth <= 0){

            //Instead of destroying the gameObject, this plays the Death animation.
            // lucyAnimator.SetBool("isDead", true);
            //THIS LINE DOESNT WORK, the animation plays but it I forgot that the game pauses it will still take damages when the object isnt destroyed.

            Destroy(gameObject); // temporary result
            songManager.instance.pauseGame();
            
        }

    }
    
    private void OnCollisionEnter(Collision collision)
    {
        // player is hit by an attack
        if (collision.gameObject.tag == "Attack") {

            Inputs_Lucy_Test inputsLucy = GetComponentInParent<Inputs_Lucy_Test>();

            /*if (inputsLucy.damageReduction == true && !hasReducedDamage)
            {
                pDamageTemp *= reductionValue;
                hasReducedDamage = true;
            }
            else*/
            if(inputsLucy.damageNegation == true)
            {
                pDamageTemp = 0;
            }
            else{
                pDamageTemp = pDamage;
            }

            currentHealth -= pDamageTemp;
            
            // show health on UI
            playerHealth.text = "Player Health: " + currentHealth;

            //(ANIMATION) The animation isHitTrigger is set to true and exits back to idle or running animation
            lucyAnimator.SetTrigger("isHitTrigger");

        }

        if(collision.gameObject.tag == "Boss")
        {
            Inputs_Lucy_Test inputsLucy = GetComponentInParent<Inputs_Lucy_Test>();

            /*if (inputsLucy.damageReduction == true && !hasReducedDamage)
            {
                cDamageTemp *= reductionValue;
                hasReducedDamage = true;
            }
            else*/
            if(inputsLucy.damageNegation == true){
                cDamageTemp = 0;
            }
            else{
                cDamageTemp = cDamage;
            }

            currentHealth -= cDamageTemp;

            // show health on UI
            playerHealth.text = "Player Health: " + currentHealth;
        }
    }
}
