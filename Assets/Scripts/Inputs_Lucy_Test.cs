using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inputs_Lucy_Test : MonoBehaviour
{
    // data for Lucy's mesh
    //public GameObject theGameObject;
    //private Renderer GORenderer;

    // data for Lucy's position
    public Transform AttackSpawnPoint;
    public Transform MissedSpawnPoint;
    public Transform ShieldSpawnPoint;

    // data for Lucy's attack/miss
    public GameObject AttackPrefab; // basic attack prefab (J)
    public GameObject combo1Prefab; // combo 1 attack prefab
    public GameObject combo2Prefab; // combo 2 shield prefab
    public GameObject MissedPrefab;
    public float AttackSpeed = 10;

    // data for handling attack timing
    private int beatCount;                          // beat of the song
    private int beatSnapshot;                       // snapshotting the beat of the song

    private bool onBeat = false;

    [SerializeField] private bool attackable;       // can player attack or not
    [SerializeField] private float attackWindow;    // delay between attacks
    [SerializeField] private float missedDelay;     // delay for missing a beat

    // data for handling combo system
    [SerializeField] private bool comboMode;
  
    public KeyCode[] Combo1 = new KeyCode[] { KeyCode.Alpha1, KeyCode.Alpha2};
    public KeyCode[] Combo2 = new KeyCode[] { KeyCode.Alpha1, KeyCode.Alpha2};

    public int Combo1Test = 0;
    public int Combo2Test = 0;

    public List<KeyCode> comboInputs = new List<KeyCode>();

    // variable to determine if player has damage reduction

  /*public bool damageReduction = false;
    public float damageReductionDuration = 1;*/

    // variables to determine if player has damage negation

    public bool damageNegation = false;
    // if the negation duration is changed, also change the time that the shield is visually present in shieldBehavior.cs so they match
    public float damageNegationDuration = 3;


    void Start()
    {
        /*
        GORenderer stuff was for the capsule to change materials. 
        These related lines of code are currently not under use.

        However, there are plans for Lucy's model to flash red when taking damage and 
        also contain other visual cues when the attack cooldown is occuring due to a miss.

        Leaving this bit of code in for now so that later we can 
        come back to it and update it as needed.
         */

        //GORenderer = theGameObject.GetComponent<Renderer>();
        //GORenderer.material.SetColor("_Color", green);

        attackable = true;
        comboMode = false;
        
    }

    void Update()
    {
        beatCount = songManager.instance.beatCount;     // grab song beat count from song manager
        onBeat = songManager.instance.isOnBeat;         // grab song beat boolean from song manager

        // attack logic
        if(songManager.instance.gameState == true && songManager.instance.enableAttacks == true)
        {
            // input on beat
            if (onBeat)
            {
                // not in combo mode 
                if (comboMode == false)
                {
                    // J is pressed
                    if (Input.GetKeyDown(KeyCode.J) && attackable == true)
                    {

                        createAttack();     // instantiate a player attack

                        attackable = false;                     // player cannot attack;
                        Invoke("attackAgain", attackWindow);    // delay set after a successful attack
                                                                // until player can attack again

                        // if (!consecutive) // first attack
                        // {
                        //     consecutive = true;
                        //     beatSnapshot = beatCount;
                        // } 
                        // else // not first attack
                        // {
                        //     if (beatCount == beatSnapshot + 2)
                        //     {
                        //         beatSnapshot = beatCount;
                        //     }
                        // }

                        /*
                        1. player initiates combo
                        2. snapshot the beat
                        3. under combo mode, check if the current time is 
                        
                        
                        
                        
                        */

                        // check if first attack or not
                        // first attack = no consecutive attack previous to this
                        // go straight to snapshotting the time

                        // not first attack = consectuive attack previous to this
                        // compare previously snapshotted time to current time

                        // snapshot the time
                        
                        
                    }
                    else if (Input.GetKeyDown(KeyCode.J) && attackable == false)
                    {
                        CancelInvoke();     // cancel any current delay timers

                        createMissedAttack();   // instantiate "Missed!" message

                        Invoke("attackAgain", missedDelay);     // delay set after missed attack
                                                                // until player can attack again
                    }

                    // K is pressed
                    if (Input.GetKeyDown(KeyCode.K))
                    {
                        comboMode = true;   // initiate a combo
                        print("Combo Initiated!");

                        beatSnapshot = beatCount;
                        // print ("Snapshot on " + beatSnapshot + " and expect next attack on " + (beatSnapshot + 2));
                    }

                    // L is pressed
                    /*if (Input.GetKey(KeyCode.L) && damageReduction == false)
                    {
                        // set the damage reduction boolean to true. this variable is used in the PlayerHealth.cs script
                        damageReduction = true;
                        Invoke("damageReductionClear", damageReductionDuration);
                    }*/


                }

                // in combo mode
                else if (comboMode == true)
                {
                    /*
                    check if the player attacks consecutively.
                    tracks the current time of the keypress and expects another keypress 2 beats later, on the next onBeat.
                    */
                    if (beatCount == beatSnapshot + 2){

                        /*
                        After pressing the combo key (K), if J, K, or L are pressed, they are added to the comboInputs list.
                        List was used over an array because the array.push() method was having issues with KeyCodes.
                        */
                        
                        // J is pressed
                        if (Input.GetKeyDown(KeyCode.J))
                        {   
                            comboInputs.Add(KeyCode.J);
                            print("J");

                            comboCheck();

                            beatSnapshot = beatCount;
                        }

                        // K is pressed
                        if (Input.GetKeyDown(KeyCode.K))
                        {
                            comboInputs.Add(KeyCode.K);
                            print("K");

                            comboCheck();

                            beatSnapshot = beatCount;
                        }

                        // L is pressed
                        /*if (Input.GetKeyDown(KeyCode.L))
                        {
                            comboInputs.Add(KeyCode.L);
                            print("L");

                            comboCheck();

                            beatSnapshot = beatCount;
                        }*/
                    } 
                    
                    // if the player doesn't attack consecutively (waits too long)
                    else if (beatCount > (beatSnapshot + 2))
                    {
                        print("Combo Failed!");

                        comboInputs.Clear();
                        Combo1Test = 0;
                        Combo2Test = 0;

                        //Added invoke command so that if a combo happens to end in 'K', it does not immediately initiate another combo
                        Invoke("comboReset", 1);
                    }

                    //If the maximum number of inputs expected is reached (3), check for completed combos
                    if (comboInputs.Count == 2)
                    {
                        if (Combo1Test == 2)
                        {
                            print("Combo 1 Completed Successfully!");
                            combo1();
                        }
                        else if (Combo2Test == 2)
                        {
                            print("Combo 2 Completed Successfully!");
                            combo2();
                        }
                        else
                        {
                            print("Combo Failed!");
                        }

                        comboInputs.Clear();
                        Combo1Test = 0;
                        Combo2Test = 0;

                        //Added invoke command so that if a combo happens to end in 'K', it does not immediately initiate another combo
                        Invoke("comboReset", 1);
                    }
                }
            }
            
            // input off-beat
            else
            {
                // J/K/L is pressed
                if (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.K))
                {
                    CancelInvoke();     // cancel any current delay timers

                    createMissedAttack();   // instantiate "Missed!" message

                    Invoke("attackAgain", missedDelay);     // delay set after missed attack
                                                            // until player can attack again
                    
                    comboMode = false;

                    comboInputs.Clear();
                    Combo1Test = 0;
                    Combo2Test = 0;
                }

            }

            
        }
        // if gameState is not true
        else
        {
            return;
        }
    }

    void createAttack()  // create basic attack (J)
    {
        var attack = Instantiate(AttackPrefab, AttackSpawnPoint.position, AttackSpawnPoint.rotation);
        Vector3 bossPosition = GameObject.FindWithTag("Boss").transform.position;
        bossPosition.y += 6;
        Vector3 direction = bossPosition - AttackSpawnPoint.position;
        direction = direction.normalized;
        attack.GetComponent<Rigidbody>().velocity = direction * AttackSpeed;
    }

    void createMissedAttack() // Text prefab when player misses
    {
        Instantiate(MissedPrefab, MissedSpawnPoint.position, MissedSpawnPoint.rotation);
    }

    void attackAgain() // used for Invoke function to allow player to attack again
    {
        attackable = true;
    }

    /*void damageReductionClear() // used for Invoke function to turn off damage reduction
    {
        damageReduction = false;
    }*/

    void damageNegationClear() //used for Invoke function to turn off damage negation
    {
        damageNegation = false;
    }

    void comboReset() // used for Invoke function to turn off combo mode
    {
        comboMode = false;
    }

    void comboCheck() // check player inputs and whether they fulfill combo requirements
    {
        if (comboInputs[(comboInputs.Count) - 1] == Combo1[Combo1Test])
        {
            Combo1Test++;
        }

        if (comboInputs[(comboInputs.Count) - 1] == Combo2[Combo2Test])
        {
            Combo2Test++;
        }
    }

    void combo1() // create combo 1 attack
    {
        var attack = Instantiate(combo1Prefab, AttackSpawnPoint.position, AttackSpawnPoint.rotation);
        Vector3 bossPosition = GameObject.FindWithTag("Boss").transform.position;
        bossPosition.y += 6;
        Vector3 direction = bossPosition - AttackSpawnPoint.position;
        direction = direction.normalized;
        attack.GetComponent<Rigidbody>().velocity = direction * (AttackSpeed * 0.3f);
    }

    void combo2() // create combo 2 shield
    {
        GameObject player = GameObject.FindWithTag("Player");
        Instantiate(combo2Prefab, ShieldSpawnPoint.position, ShieldSpawnPoint.rotation, player.transform);
        damageNegation = true;
        Invoke("damageNegationClear", damageNegationDuration);
    }

}
