using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttacks : MonoBehaviour
{
    public songManager songManager;

    public Transform AttackSpawnPoint;
    public GameObject AttackPrefab;

    public enum attackType
    {
        Neutral,
        Projectile,
        Charge,
        FireCharge
    }

    public attackType currentAttack;

 
    private bool randomized = false;

    private Vector3 savedPosition;
    private Quaternion savedRotation;

    public float AttackSpeed = 10;
    public float attackFrequency = 1;
    public float rangeXMin = -1;
    public float rangeXMax = 1;
    public float rangeYMin = -1;
    public float rangeYMax = 1;

    private float tempPosition;

    [SerializeField] private float chargeSpeed = 0.5f;



    void Start()
    {
        savedPosition = transform.position;
        savedRotation = transform.rotation;
    }

   

    private void Update()
    {
        if (songManager.instance.gameState == true)
        {
            /*switch (currentAttack) {
                case attackType.Projectile:
                    print("Projectile attacks");
                    break;
                case attackType.Charge:
                    print("Charge attack!");
                    break;
                case attackType.FireCharge:
                    print("The Ground is ablaze!");
                    break;                
            }*/

            if (currentAttack == attackType.Projectile)
            {
                attackTiming();
            }

            if (currentAttack == attackType.Charge)
            {   
                if (Input.GetKeyDown(KeyCode.I))
                {
                    StartCoroutine(chargeAttack());
                }
                
            }

        }
    }
    
    void attackTiming()
    {
        if (tempPosition == 0)
        {
            tempPosition = songManager.instance.songPositionInBeats;
        }

        if (songManager.instance.songPositionInBeats - tempPosition >= attackFrequency)
        {
            createAttack();
            tempPosition = 0;
        }
    }

    public void createAttack()
    {
        var randomPosition = new Vector3(Random.Range(rangeXMin, rangeXMax), Random.Range(rangeYMin, rangeYMax), AttackSpawnPoint.position.z);
        var attack = Instantiate(AttackPrefab, randomPosition, AttackSpawnPoint.rotation);
        attack.GetComponent<Rigidbody>().velocity = AttackSpawnPoint.forward * AttackSpeed;
    }


    IEnumerator chargeAttack()
    {
        //List of the random positions that the bus can start charging from
        float[] randomPositions = { -15, -5, 5, 15 };

        //setting a target position so the bus stops moving after it charges to this point
        Vector3 targetPosition = new Vector3(transform.position.x, transform.position.y, -34);

        //setting a newPosition variable to save the current transform.position value
        Vector3 newPosition = transform.position;

        // Set the initial position and rotation
        // randomized variable prevents the bus from changing its position each frame
        if(randomized == false)
        {
            newPosition = new Vector3(randomPositions[Random.Range(0, 4)], savedPosition.y, savedPosition.z);
            transform.rotation = new Quaternion(0, 0.70711f, 0, 0.70711f);

            transform.position = newPosition;
            randomized = true;
        }        

        // Wait for 1 second before charging
        yield return new WaitForSeconds(1);

        // Moves the bus from the current position to the target position
        while (transform.position.z > targetPosition.z + 0.1f)
        {
            newPosition.z = newPosition.z - chargeSpeed;
            transform.position = newPosition;

            yield return null;
        }

        // Reset the boss position after reaching the target
        transform.position = savedPosition;
        transform.rotation = savedRotation;
        randomized = false;

        //End coroutine
        yield break;
    }


} 
