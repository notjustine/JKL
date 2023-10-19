using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class songManager : MonoBehaviour
{
    [HideInInspector] public float songBpm;

    //Current song position, in seconds
    public float songPositionInSec;

    //Current song position, in beats
    public float songPositionInBeats;

    //The number of seconds for each song beat
    private float secPerBeat;

    //How many seconds have passed since the song started
    private float dspSongTime;

    //Whether the song start time has been recorded or not.
    //Added to prevent the dspSongTime variable from being overwritten since it's in the update() loop
    private bool songStartRecorded = false;

    //Whether the game is on or off. If the game is on, boss music will play and the boss will start attacking.
    [HideInInspector] public bool gameState = false;

    //Whether the song is playing or not. This prevents the song start command from looping.
    private bool songState = false;

    

    //an AudioSource attached to this GameObject that will play the music.
    [HideInInspector] public AudioSource song;

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
        //Calculate the number of seconds in each beat
        secPerBeat = 0.31f;
    }

    void Update()
    {
        if(gameState == true)
        {
            if(songStartRecorded == false)
            {
                //Record the time when the music starts
                dspSongTime = (float)AudioSettings.dspTime;

                songStartRecorded = true;
            }
            

            //determine how many seconds since the song started
            songPositionInSec = (float)(AudioSettings.dspTime - dspSongTime);

            //determine how many beats since the song started
            songPositionInBeats = songPositionInSec / secPerBeat;
        }

        //If any key is pressed, start the game
        if (Input.anyKeyDown)
        {
            gameState = true;
        }

        //Start the music
        if (gameState == true && songState == false)
        {
            song.Play();
            songState = true;
        }

    }
   
}
