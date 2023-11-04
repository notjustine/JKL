using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    [SerializeField] private Rigidbody rb;
    void Start()
    {
        
    }

    void Update()
    {
        if (songManager.instance.gameState == true)
        {
            var movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            rb.velocity = movement * speed;
        }
    }
}
