using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissedMovement : MonoBehaviour
{
    public float life = 1;
    public Vector3 offset = new Vector3(-2, 2, 0);

    void Start()
    {
        Destroy(gameObject, life);
        transform.localPosition += offset;
    }
}
