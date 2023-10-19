using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackIndicator : MonoBehaviour
{

    public GameObject playerInput;
    private Inputs beatTracker;
    private Renderer barRenderer;

    private Color whenOnBeat;
    private Color whenOffBeat;

    void Start()
    {
        beatTracker = playerInput.GetComponent<Inputs>();
        barRenderer = GetComponent<Renderer>();
        whenOffBeat = Color.black;
        whenOnBeat = Color.cyan;
    }


    void Update()
    {
        if(beatTracker.onBeat == false)
        {
            barRenderer.material.color = whenOffBeat;
        }
        else
        {
            barRenderer.material.color = whenOnBeat;
        }
    }
}
