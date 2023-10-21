using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class songManager : MonoBehaviour
{
    public float songBpm;

    //Current song position, in seconds
    public float songPositionInSec;

    //Current song position, in beats
    public float songPositionInBeats;

    // song position (beats) in integer
    public int beatCount = 0;

    //The number of seconds for each song beat
    private float secPerBeat;

    //How many seconds have passed since the song started
    private float dspSongTime;

    // the offset to the first beat of the song in seconds
    public float firstBeatOffset;

    //Whether the song start time has been recorded or not.
    //Added to prevent the dspSongTime variable from being overwritten since it's in the update() loop
    private bool songStartRecorded = false;
    


    //Whether the game is on or off. If the game is on, boss music will play and the boss will start attacking.
    [HideInInspector] public bool gameState = false;

    //Whether the song is playing or not. This prevents the song start command from looping.
    private bool songState = false;

    public bool isOnBeat = false;


    //an AudioSource attached to this GameObject that will play the music.
    public AudioSource song;

    [HideInInspector] public BossAttacks attacks;


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
    }

    void Update()
    {

        //If any key is pressed, start the game
        if (Input.anyKeyDown)
        {
            gameState = true;
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
            songPositionInSec = (float)(AudioSettings.dspTime - dspSongTime + firstBeatOffset);

            //Determine how many beats since the song started
            songPositionInBeats = songPositionInSec / secPerBeat;

            //Convert song position into integer beats
            beatCount = (int)songPositionInBeats;

            //If the song is on or off beat (every other count)
            if (beatCount % 2 == 0)
            {
                isOnBeat = false;
            }
            else
            {
                isOnBeat = true;
            }
        }
        else
        {
            song.Pause();
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
