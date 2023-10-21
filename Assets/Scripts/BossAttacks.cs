using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttacks : MonoBehaviour
{
    public songManager songManager;

    public Transform AttackSpawnPoint;
    public GameObject AttackPrefab;
    public float AttackSpeed = 10;
    public float attackFrequency = 1;
    public float rangeXMin = -1;
    public float rangeXMax = 1;
    public float rangeYMin = -1;
    public float rangeYMax = 1;

    private float tempPosition;

    //public AudioSource song;

    //public float bpm;


    private void Awake()
    {
        //songManager.instance.song = song;
        //songManager.instance.attacks = this;
        //songManager.instance.songBpm = bpm;
    }

    //on start, begin attacking at set intervals. 
    //the attackFrequency variable controls how many seconds each new attack is fired
    void Start()
    {
        //InvokeRepeating("createAttack", attackFrequency, attackFrequency);
    }

    //this command instantiates the attack prefab

    private void Update()
    {
        if (songManager.instance.gameState == true)
        {
            attackTiming();
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
}
