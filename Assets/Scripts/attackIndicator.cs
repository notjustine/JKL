using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class attackIndicator : MonoBehaviour
{
    //private Inputs beatTracker;

    public songManager songManager;
    public GameObject bar;
    private Renderer barRenderer;

    private Color RGBgrey = new Color(137, 137, 137);
    private Color RGBwhite = new Color(255, 255, 255);

    private int songBeat;

    void Start()
    {
        //beatTracker = playerInput.GetComponent<Inputs>();

        barRenderer = bar.GetComponent<Renderer>();

        //whenOffBeat = Color.black;
        //whenOnBeat = Color.cyan;
    }


    void Update()
    {

        //if (songManager.instance.isOnBeat)
        //{
        //    barRenderer.material.SetColor("_Color", cyan);
        //}
        //else
        //{
        //    barRenderer.material.SetColor("_Color", black);
        //}



        songBeat = (int)songManager.instance.songPosWithOffset;

        if (songBeat % 2 == 0)
        {
            // barRenderer.material.color = Color.cyan;
            barRenderer.material.color = Color.white;
        }
        else
        {
            barRenderer.material.color = Color.grey;
        }

        //if(beatTracker.onBeat == false)
        //{
        //    barRenderer.material.color = whenOffBeat;
        //}
        //else
        //{
        //    barRenderer.material.color = whenOnBeat;
        //}
    }
}
