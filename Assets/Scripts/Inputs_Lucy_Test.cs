using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inputs_Lucy_Test : MonoBehaviour
{
    public GameObject theGameObject;
    private Renderer GORenderer;

    private Color green = new Color(0, 0.65f, 0);
    private Color red = new Color(0.5f, 0, 0);

    [SerializeField] private bool comboMode;
  
    
    public KeyCode[] Combo1 = new KeyCode[] { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3 };
    public KeyCode[] Combo2 = new KeyCode[] { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3 };

    public int Combo1Test = 0;
    public int Combo2Test = 0;

    public List<KeyCode> comboInputs = new List<KeyCode>();

    public Transform AttackSpawnPoint;
    public Transform MissedSpawnPoint;
    public GameObject AttackPrefab;
    public GameObject MissedPrefab;
    public float AttackSpeed = 10;

    private bool onBeat = false;
    //private bool attackOnce = false;
    [SerializeField] private bool attackable;

    // delay between attacks
    [SerializeField] private float attackWindow;

    // delay for missing a beat
    [SerializeField] private float missedDelay;


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
        //GORenderer stuff was for the capsule to change materials.
        //These related lines of code are currently not under use.
        //However, there are plans for Lucy's model to flash red when taking damage and also
        //Contain other visual cues when the attack cooldown is occuring due to a miss
        //Leaving this bit of code in for now so that later we can come back to it and update it as needed

        GORenderer = theGameObject.GetComponent<Renderer>();
        //GORenderer.material.SetColor("_Color", green);

        comboMode = false;
        attackable = true;
    }

    // Update is called once per frame
    void Update()
    {
        onBeat = songManager.instance.isOnBeat;
        //print(attackOnce);
        
        //Attack logic
        if(songManager.instance.gameState == true)
        {
            if (comboMode == false)
            {
                if (Input.GetKeyDown(KeyCode.J) && onBeat == true && attackable == true)
                {
                    createAttack();
                    attackable = false;
                    //GORenderer.material.SetColor("_Color", green);

                    Invoke("attackAgain", attackWindow);
                }
                else if (Input.GetKeyDown(KeyCode.J))
                {
                    //print("Missed the beat!");
                    CancelInvoke();
                    createMissedAttack();
                    attackable = false;
                    //GORenderer.material.SetColor("_Color", red);

                    Invoke("attackAgain", missedDelay);
                }

                if (Input.GetKey(KeyCode.L))
                {
                    
                }

                if (Input.GetKey(KeyCode.K))
                {
                    print("Combo Initiated!");
                    comboMode = true;
                }
            }
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

                if(comboInputs.Count == 3)
                {
                    if (Combo1Test == 3)
                    {
                        print("Combo 1 Completed Successfully!");
                    }else if(Combo2Test == 3)
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
