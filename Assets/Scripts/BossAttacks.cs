using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttacks : MonoBehaviour
{
    public Transform AttackSpawnPoint;
    public GameObject AttackPrefab;

    
    // these are variables for the boss projectiles
    public float AttackSpeed = 10;
    public float attackFrequency = 1;
    public float rangeXMin = -1;
    public float rangeXMax = 1;
    public float rangeYMin = -1;
    public float rangeYMax = 1;

    // these are variables for the bus charge coroutine
    private Vector3 savedPosition;
    private Quaternion savedRotation;
    private bool randomized = false;
    [SerializeField] private float chargeSpeed = 0.5f;

    [SerializeField] private bool hasCharged = false;

    // used for timing attacks to beats. So far this is being used for the projectile and charge attacks.
    private double tempPosition;

    void Start()
    {
        // saves the current position and rotation values of the bus so it can reset to this later
        savedPosition = transform.position; 
        savedRotation = transform.rotation;
    }


    private void Update()
    {
        if (songManager.instance.gameState == true)
        {
            if(songManager.instance.currentAttack == songManager.attackType.Phase1)
            {
                attack3(3);
            }

            if (songManager.instance.currentAttack == songManager.attackType.Phase2)
            {
                attack1(1.3f);
            }

            if (songManager.instance.currentAttack == songManager.attackType.Phase3)
            {
                attack1(0.6f);
            }

            if (songManager.instance.currentAttack == songManager.attackType.Phase4)
            {
                attack2(1f);
            }

            if (songManager.instance.currentAttack == songManager.attackType.Phase5)
            {
                attack2(0.5f);
            }


        }
    }

    private void chargeTimer(float chargeInterval)
    {
        hasCharged = true;
        StartCoroutine(chargeAttack()); 
        Invoke("chargeReset", chargeInterval);
        
    }
     
    private void chargeReset()
    {
        hasCharged = false;
    }


    private void attack1(float frequency) //the lower the number, the more frequent the attacks. Yes it's confusing, too lazy to change it. 
    {
        if (tempPosition == 0)
        {
            tempPosition = songManager.instance.songPositionInBeats;
        }

        if (songManager.instance.songPositionInBeats - tempPosition >= frequency)
        {
            createProjectile();
            tempPosition = 0;
        }
    }

    private void attack2(float frequency) 
    {
        if (tempPosition == 0)
        {
            tempPosition = songManager.instance.songPositionInBeats;
        }

        if (songManager.instance.songPositionInBeats - tempPosition >= frequency)
        {
            createSmartProjectile();
            tempPosition = 0;
        }
    }

    private void attack3(float chargeInterval)
    {
        if (hasCharged == false)
        {
            chargeTimer(chargeInterval);
        }
    }

    public void createProjectile()
    {
        var randomPosition = new Vector3(Random.Range(rangeXMin, rangeXMax), Random.Range(rangeYMin, rangeYMax), AttackSpawnPoint.position.z);
        var attack = Instantiate(AttackPrefab, randomPosition, AttackSpawnPoint.rotation);
        attack.GetComponent<Rigidbody>().velocity = AttackSpawnPoint.forward * AttackSpeed;
    }

    public void createSmartProjectile()
    {
        var attack = Instantiate(AttackPrefab, AttackSpawnPoint.position, AttackSpawnPoint.rotation);
        Vector3 playerPosition = GameObject.FindWithTag("Player").transform.position;
        playerPosition.y += 2;
        Vector3 direction = playerPosition - AttackSpawnPoint.position;
        direction = direction.normalized;
        attack.GetComponent<Rigidbody>().velocity = direction * AttackSpeed;
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
            transform.rotation = new Quaternion(0, 1, 0, 0);

            transform.position = newPosition;
            randomized = true;
        }

        // Wait for 1 second before charging
        yield return new WaitUntil(chargeReady);

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

    bool chargeReady() // used in the coroutine to make the bus wait X number of beats before charging at the player
    {
        if (tempPosition == 0)
        {
            tempPosition = songManager.instance.songPositionInBeats;
        }

        if (songManager.instance.beatCount - tempPosition >= 2) // number of beats. Currently set to 2.
        {
            tempPosition = 0;
            return true;
        }
        else
        {
            return false;
        }
    }



} 
