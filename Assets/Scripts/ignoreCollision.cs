using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ignoreCollision : MonoBehaviour
{
    void Start()
    {
        //Prevents collisions between the player and player attacks        
        Physics.IgnoreLayerCollision(6, 8);

        //Prevents collisions between the boss and boss attacks
        Physics.IgnoreLayerCollision(7, 9);

        //Prevents player attacks and boss attacks from deleting each other
        Physics.IgnoreLayerCollision(8, 9);
    }

    
}
