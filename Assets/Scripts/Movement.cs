using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    [SerializeField] private Rigidbody rb;

    //(Roann) Rotation speed from turning one direction to another:
    [SerializeField] public float rotationSpeed;
    void Start()
    {
        
    }

    void Update()
    {
        if (songManager.instance.gameState == true)
        {
            UnityEngine.Vector3 movement = new UnityEngine.Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            // movement.Normalize();
            rb.velocity = movement * speed;
        //Roann's Movement Rotation Script

        // transform.Translate(movement);

        if (movement != UnityEngine.Vector3.zero) //Checks if the object/character is not moving - if it IS then run this statement
        {

            // transform.forward = movement; //Faces the object forward where the the vector 3 is moving INSTANTLY

            //I had to use "UnityEngine." in Quaternion and Vector3 because its referring to UnityEngine and not System.Numerics. 
            //I gotta specify that Im using UnityEngine namespace and not the other.
            
            UnityEngine.Quaternion toRotation =  UnityEngine.Quaternion.LookRotation(movement, UnityEngine.Vector3.up); //This line rotates the object in the desired direction where FORWARD is MOVEMENT and UP is the Y-axis (it never change).
            transform.rotation = UnityEngine.Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }

        }


    }
}


/*Rotation smoothing reference:
https://www.youtube.com/watch?v=BJzYGsMcy8Q - Rotating a Character in the Direction of Movement (Unity Tutorial)

*/
