using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackIndicator : MonoBehaviour
{
    //private Inputs beatTracker;

    public songManager songManager;

    public GameObject bar;
    private Renderer barRenderer;

    private Color black = new Color(0, 0, 0);
    private Color cyan = new Color(0, 1, 1);

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
            barRenderer.material.SetColor("_Color", cyan);
        }
        else
        {
            barRenderer.material.SetColor("_Color", black);
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
