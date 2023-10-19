using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inputs : MonoBehaviour
 
{
    private Vector3 scaleChange;
    private bool comboMode;
  
    public KeyCode[] KeyCodeSeries = new KeyCode[] { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3 };

    public Transform AttackSpawnPoint;
    public Transform MissedSpawnPoint;
    public GameObject AttackPrefab;
    public GameObject MissedPrefab;
    public float AttackSpeed = 10;

    public bool onBeat = false;
    private bool attackOnce = false;

    [SerializeField] private float attackWindow;
    

    private float tempPosition;


    // Specify in the inspector the function you want to call
    // after the series have been completed
    public UnityEngine.Events.UnityEvent OnSeriesComplete;

    // Specify in the inspector the function you want to call
    // after the series have been failed because a wrong key has been pressed
    public UnityEngine.Events.UnityEvent OnSeriesFailed;

    // The index in the array of the next key to press in order to continue the series
    private int keyCodeIndex;


    void Start()
    {
        comboMode = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(songManager.instance.gameState == true)
        {
            canAttack();
        }
        

        if (comboMode == false)
        {
            if (Input.GetKeyDown(KeyCode.J) && onBeat == true && attackOnce == false)
            {
                createAttack();
                attackOnce = true;
            }

            if (Input.GetKeyDown(KeyCode.J) && onBeat == false)
            {
                print("Missed the beat!");
                createMissedAttack();
            }

            if (Input.GetKey(KeyCode.L))
            {
                //transform.localScale -= scaleChange;
            }

            if (Input.GetKey(KeyCode.K))
            {
                print("Combo Initiated!");
                comboMode = true;
            }
        }else if (comboMode == true)
        {
            // Correct key pressed!
            if (Input.GetKeyDown(KeyCodeSeries[keyCodeIndex]))
            {
                print(KeyCodeSeries[keyCodeIndex]);
                keyCodeIndex++;

                // Series completed!
                if (keyCodeIndex >= KeyCodeSeries.Length)
                {
                    if (OnSeriesComplete != null)
                        //OnSeriesComplete.Invoke();
                        print("Combo Done!");

                    // Reset index to allow to start again
                    keyCodeIndex = 0;
                    comboMode = false;
                }
            }
            // Wrong key pressed!
            else if (Input.anyKeyDown)
            {
                keyCodeIndex = 0;
                if (OnSeriesFailed != null)
                    //OnSeriesFailed.Invoke();
                    print("Combo failed!");
                comboMode = false;
            }
        }

        // Make sure some keys have been specified in the inspector 
        if (KeyCodeSeries.Length == 0)
            return;

    }

    void createAttack()
    {
        var attack = Instantiate(AttackPrefab, AttackSpawnPoint.position, AttackSpawnPoint.rotation);
        Vector3 bossPosition = GameObject.FindWithTag("Boss").transform.position;
        Vector3 direction = bossPosition - AttackSpawnPoint.position;
        direction = direction.normalized;
        attack.GetComponent<Rigidbody>().velocity = direction * AttackSpeed;
    }

    void createMissedAttack()
    {
        Instantiate(MissedPrefab, MissedSpawnPoint.position, MissedSpawnPoint.rotation);
    }

    //Determines whether or not the user can fire an attack. They have to hit the note on beat to attack.
    void canAttack()
    {
        if (tempPosition == 0)
        {
            tempPosition = songManager.instance.songPositionInBeats;
            //Invoke("changeOnBeat", 0.5f);
       
        }

        if (songManager.instance.songPositionInBeats - tempPosition >= attackWindow)
        {
            changeOnBeat();
            //onBeat = true;
            tempPosition = 0;
            if(onBeat == true)
            {
                attackOnce = false;
            }
        }
    }

    void changeOnBeat()
    {
        onBeat = !onBeat;
        //onBeat = false;
        
    }
}
