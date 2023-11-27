using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shieldBehavior : MonoBehaviour
{
    public float life = 3;

    //Upon being instantiated, the shield will destroy itself after x many seconds
    //x being the 'life' variable
    void Awake()
    {
        Destroy(gameObject, life);
    }

    void Update()
    {

    }
}
