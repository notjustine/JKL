using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class songManager : MonoBehaviour
{
    //an AudioSource attached to this GameObject that will play the music.
    public AudioSource Intro;
    public AudioSource Verse;

    public BossHealth bossHealth;
    public PlayerHealth playerHealth;

    [SerializeField]
    private double songBpm;

    //How many seconds have passed since the song started
    private double dspSongTime;

    //Current song position, in seconds
    private double songPositionInSec;

    //Current song position, in beats
    [HideInInspector]
    public double songPositionInBeats;

    //The number of seconds for each song beat
    private double secPerBeat;

    // song position (beats) in integer
    public int beatCount = 0;

    // records the amount of time paused in dsp time so it can be adjusted after it is unpaused
    private double pauseTime;

    // used to control the order of texts in the combo teaching phase
    private int textCount = 0;

    // the offset to the first beat of the song in seconds
    [SerializeField]
    private double firstBeatOffset;

    // slight delay for the attack indicator 
    [HideInInspector]
    public double songPosWithOffset;

    //Whether the song start time has been recorded or not. 
    //Added to prevent the dspSongTime variable from being overwritten since it's in the update() loop
    private bool songStartRecorded = false;
    
    //Whether the game is on or off. If the game is on, boss music will play and the boss will start attacking.
    [HideInInspector] 
    public bool gameState = false;

    //Whether the song has started or not. This prevents the song start command from looping.
    private bool songStart = false;

    public bool isOnBeat = false;

    //Boolean to control whether the player can use the attack keys (J and K) or not
    public bool enableAttacks = false;

    public TMP_Text spaceToStart;

    [SerializeField] public GameObject pauseCanvas;
    public TMP_Text bossText;

    // these are variables to create a boss pattern manager
    public enum attackType
    {
        Phase0,  //Introduction. No boss attacks. No player attacks.
        Phase1, //Boss starts attacking with projectiles. Player must learn to dodge using WASD.
        Phase2, //Player must learn to attack back using J key
        Phase3, 
        Phase4,
        Phase5,
        Phase6
    }
    public attackType currentAttack;

    //makes this script a singleton so its variables can be accessed easily in other scripts
    public static songManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        //Calculate the number of seconds in each beat
        secPerBeat = 60d / songBpm;

        // Sets the boss' attack phase to Phase 0)
        currentAttack = attackType.Phase0;
    }

    void Update()
    {
        //If the space bar is pressed, start the game
        if (Input.GetKeyDown(KeyCode.Space))
        {
            gameState = true;
            spaceToStart.SetText(string.Empty);
        }

        if(playerHealth.currentHealth <= 0)
        {
            endGame();
        }

        //If game is started
        if (gameState == true)
        {
            //Code to control boss phases
            switch (currentAttack)
            {
                default:

                case attackType.Phase0: //intro song plays. No boss attacks. No player attacks. Players can move.
                    bossText.SetText("A  monstrous  bus  appears");
                    //Play the Intro Music
                    if (songStart == false) //songStart is used to make sure the song clip is only started once
                    {
                        Intro.Play();
                        
                        songStart = true; 
                    }

                    //When the song reaches x amount of beats, switch to the next phase
                    if (beatCount >= 16)
                    {
                        currentAttack = attackType.Phase1;
                        Intro.Stop();                      
                        songStart = false;
                    }
                    break;

                case attackType.Phase1: //Boss begins to shoot projectiles. Players must dodge. 
                    bossText.SetText("It's  attacking!");

                    //Play the Verse Music
                    if (songStart == false)//songStart is used to make sure the song clip is only started once
                    {
                        Verse.Play();
                        songStart = true;
                    }
                    
                    // Phase 1 lasts for 64 beats before switching to the next phase
                    if (beatCount >= 32)
                    {
                        currentAttack = attackType.Phase2;
                    }
                    break;

                case attackType.Phase2: //Players learn to attack back.

                    bossText.SetText("Press  J  to  Fight  Back");
        
                    //Allows the players to use J and K
                    enableAttacks = true;

                    // When the boss health falls to 90 health or below, switch to next phase
                    if (bossHealth.currentHealth <= 90)
                    {
                        currentAttack = attackType.Phase3;
                    }                  
                    break;

                case attackType.Phase3:

                    if(textCount == 0)
                    {
                        bossText.SetText("Press  K  to  start a combo");
                    }else if(textCount == 1)
                    {
                        bossText.SetText("Follow  up  with  J  and  J  for  a  heavy attack");
                    }

                    if (Input.GetKey(KeyCode.K))
                    {
                        textCount = 1;
                    }
                    
                    if (bossHealth.currentHealth <= 75)
                    {
                        currentAttack = attackType.Phase4;
                    }
                    break;

                case attackType.Phase4:

                    bossText.SetText("Press  KKJ  for  a  shield");
                    if (bossHealth.currentHealth <= 20)
                    {
                        currentAttack = attackType.Phase5;
                    }
                    break;

                case attackType.Phase5:
                    bossText.SetText("The  bus  is  getting  low");

                    if(bossHealth.currentHealth <= 0)
                    {
                        currentAttack = attackType.Phase6;
                    }
                    break;

                case attackType.Phase6:

                    bossText.SetText("The  bus  has  been  defeated!");
                    Verse.Stop();
                    break;
            }


            // Record the time when the music starts
            if (songStartRecorded == false)
            {
                dspSongTime = (double)AudioSettings.dspTime;
                songStartRecorded = true;
            }

            //Determine how many seconds since the song started
            songPositionInSec = (double)(AudioSettings.dspTime - dspSongTime);
            songPosWithOffset = songPositionInSec + firstBeatOffset;

            //Determine how many beats since the song started
            songPositionInBeats = songPositionInSec / secPerBeat;
            songPosWithOffset = songPosWithOffset / secPerBeat;

            //Convert song position into integer beats
            beatCount = (int)songPositionInBeats;

            // if the song is on or off beat (every other count)
            if (beatCount % 2 == 0)
            {
                isOnBeat = false;
            }
            else
            {
                // when all attacks should happen
                isOnBeat = true;

                // will snapshot every other beat
                // beatSnapshot = beatCount;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) && songStart == true)
        {
            pauseGame();
        }
    }


    public void pauseGame()
    {
        if(gameState == true)
        {
            gameState = false;
            Verse.Pause();
            Time.timeScale = 0;
            pauseTime = (float) AudioSettings.dspTime; // records the exact time the game paused
            //(Roann / UI Pause) Shows up the Pause canvas UI
            pauseCanvas.GetComponent<Canvas>().enabled = true;
        }
        else
        {
            gameState = true;
            Verse.UnPause();
            Time.timeScale = 1;
            double tempTime = (double)AudioSettings.dspTime - pauseTime; // saves the amount of time the game was paused for
            dspSongTime += tempTime; // adjusts the dspSongTime variable with the time the game was paused for so the beat count does not jump ahead to where dspTime currently is
            //(Roann / UI Pause) Hides up the Pause canvas UI
            pauseCanvas.GetComponent<Canvas>().enabled = false;
        }
    }

    public void endGame()
    {
        if(gameState == true)
        {
            gameState = false;
            Verse.Stop();
            Time.timeScale = 0;
            spaceToStart.SetText("You have been defeated!");
        }
    }
   
}
