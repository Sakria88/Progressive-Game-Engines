using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public float moveSpeed = 40f;
    //40f= 40 meters per second or frame.
    void Start()
    {
      HowToPlay();
    }
    
   void FixedUpdate()
    {
        Move();
    }

    void HowToPlay()
        {
        Debug.Log("Welcome to the game!"); 
        Debug.Log("Use the WASD keys to move your character around.");
        // Code to display instructions on how to play the game
    }
    void Move()
    {
        // Get input from the horizontal and vertical axes
        // and move the player accordingly
        // The Input.GetAxis function returns a value or float between -1 and 1
        //for both keyboard and joystick input devices
        // depending on the input from the player
        // The moveSpeed variable is used to control how fast the player moves
        // Time.deltaTime is used to make the movement frame rate independent
        float xValue = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float zValue = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        //transform.Translate used to store and manipulate the scale,
        //rotation and position of an obj
        transform.Translate(xValue, 0, zValue);
    }

    //FixedUpdate function is similarly to Update function
    //but differs in that it is called on a regular
    //timeline(i.e.the same time between call
    //The HowToPlay function simply prints to console a
    //message about how to play the game
}
