using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackMovement : MonoBehaviour
{
    public float life = 5;

    //Upon being instantiated, the attack will destroy itself after x many seconds
    //x being the 'life' variable
    void Awake()
    {
        Destroy(gameObject, life);
    }

    //Upon hitting another object, perform a certain command.
    //In this case, when the attack hits the floor, it should just destroy the prefab.
    //However, if the attack hits the player, it should destroy the prefab and damage the player's HP
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
