using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class songManager : MonoBehaviour
{
    //an AudioSource attached to this GameObject that will play the music.
    public AudioSource song;

    [SerializeField]
    private float songBpm;

    //How many seconds have passed since the song started
    private float dspSongTime;

    //Current song position, in seconds
    private float songPositionInSec;

    //Current song position, in beats
    [HideInInspector]
    public float songPositionInBeats;

    //The number of seconds for each song beat
    private float secPerBeat;

    // song position (beats) in integer
    public int beatCount = 0;

    // song position for tracking consecutive beats
    // public int beatSnapshot = 0;

    // the offset to the first beat of the song in seconds
    [SerializeField]
    private float firstBeatOffset;

    // slight delay for the attack indicator 
    [HideInInspector]
    public float songPosWithOffset;

    //Whether the song start time has been recorded or not. 
    //Added to prevent the dspSongTime variable from being overwritten since it's in the update() loop
    private bool songStartRecorded = false;
    
    //Whether the game is on or off. If the game is on, boss music will play and the boss will start attacking.
    [HideInInspector] 
    public bool gameState = false;

    //Whether the song is playing or not. This prevents the song start command from looping.
    private bool songState = false;

    public bool isOnBeat = false;

    public TMP_Text spaceToStart;


    //[HideInInspector] public BossAttacks attacks;


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
        // load the AudioSource attached to the Conductor GameObject
        song = GetComponent<AudioSource>();

        //Calculate the number of seconds in each beat
        secPerBeat = 60f / songBpm;

        song.Stop();
    }

    void Update()
    {

        //If the space bar is pressed, start the game
        if (Input.GetKeyDown(KeyCode.Space))
        {
            gameState = true;
            Destroy(spaceToStart);
        }

        //If game is started
        if (gameState == true)
        {
            //Start the music
            if (songState == false)
            {
                song.Play();
                songState = true;
            }

            // Record the time when the music starts
            if (songStartRecorded == false)
            {
                dspSongTime = (float)AudioSettings.dspTime;
                songStartRecorded = true;
            }
            
            //Determine how many seconds since the song started
            songPositionInSec = (float)(AudioSettings.dspTime - dspSongTime);
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

            // if (beatCount == (beatSnapshot + 1))
            //     {
            //         print("attack now!");
            //     }
            
        }
        else
        {
            //song.Pause();
            songState = false;
        }


        //print(isOnBeat);

            //Start the music
            //if (gameState == true && songState == false)
            //{
            //    song.Play();
            //    songState = true;
            //}

    }
   
}
