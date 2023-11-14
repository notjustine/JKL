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

    // data for Lucy's attack/miss
    public GameObject AttackPrefab;
    public GameObject MissedPrefab;
    public float AttackSpeed = 10;

    // data for handling attack timing
    private bool onBeat = false;
    [SerializeField] private bool attackable;       // can player attack or not
    [SerializeField] private float attackWindow;    // delay between attacks
    [SerializeField] private float missedDelay;     // delay for missing a beat

    // data for handling combo system
    [SerializeField] private bool comboMode;
  
    public KeyCode[] Combo1 = new KeyCode[] { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3 };
    public KeyCode[] Combo2 = new KeyCode[] { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3 };

    public int Combo1Test = 0;
    public int Combo2Test = 0;

    public List<KeyCode> comboInputs = new List<KeyCode>();

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
        // check beat
        onBeat = songManager.instance.isOnBeat;
        
        // attack logic
        if(songManager.instance.gameState == true)
        {
            // input on beat
            if (onBeat)
            {
                // not in a combo 
                if (comboMode == false)
                {
                    // J is pressed
                    if (Input.GetKeyDown(KeyCode.J) && attackable == true)
                    {
                        createAttack();     // instantiate a player attack

                        attackable = false;                     // player cannot attack;
                        Invoke("attackAgain", attackWindow);    // delay set after a successful attack
                                                                // until player can attack again
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
                    }

                    // L is pressed
                    if (Input.GetKey(KeyCode.L))
                    {
                        // reduce next damage
                    }


                }

                // in a combo
                else if (comboMode == true)
                {
                    
                    //After pressing the combo key (K), if J, K, or L are pressed, they are added to the comboInputs list
                    //A list was used over an array because the array.push() method was having issues with KeyCodes
                    if (Input.GetKeyDown(KeyCode.J))
                    {
                        comboInputs.Add(KeyCode.J);
                        print("J");

                        comboCheck();
                    }

                    if (Input.GetKeyDown(KeyCode.K))
                    {
                        comboInputs.Add(KeyCode.K);
                        print("K");

                        comboCheck();
                    }

                    if (Input.GetKeyDown(KeyCode.L))
                    {
                        comboInputs.Add(KeyCode.L);
                        print("L");

                        comboCheck();
                    }

                    //If the maximum number of inputs expected is reached (3), check for completed combos

                    if (comboInputs.Count == 3)
                    {
                        if (Combo1Test == 3)
                        {
                            print("Combo 1 Completed Successfully!");
                        }
                        else if (Combo2Test == 3)
                        {
                            print("Combo 2 Completed Successfully!");
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
                if (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.L))
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
        else
        {
            return;
        }
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

    void attackAgain()
    {
        attackable = true;
    }

    void comboReset()
    {
        comboMode = false;
    }

    void comboCheck()
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

}
