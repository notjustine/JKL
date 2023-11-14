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
                chargeAttack();
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

    public void chargeAttack()
    {
        var speed = 60;
        float[] randomPositions = {-15, -5, 5, 15};
        Vector3 targetPosition = new Vector3(transform.position.x, transform.position.y, -34);

        if (randomized == false)
        {
            transform.position = new Vector3(randomPositions[Random.Range(0, 4)], savedPosition.y, savedPosition.z);
            transform.rotation = new Quaternion(0, 0.70711f, 0, 0.70711f);
            randomized = true;
        }
        
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if(transform.position.z <= targetPosition.z)
        {
            //transform.position = savedPosition;
            //transform.rotation = savedRotation;
            randomized = false;
        }
    }

}
