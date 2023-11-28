using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationLucyScript : MonoBehaviour
{
    //The Animator name
    Animator lucyAnimator; 
    // Start is called before the first frame update
    void Start()
    {
        //getting the component animator in the object
        lucyAnimator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (songManager.instance.gameState == true)
        {
        if (Input.GetKey("w")|| Input.GetKey("a")||Input.GetKey("s")||Input.GetKey("d"))
        {
            lucyAnimator.SetBool("isRunning", true);
        }
        else{
            lucyAnimator.SetBool("isRunning", false);
        }
        }//SongManager Close
    }
}
