using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public float moveSpeed = 40f;
    public float laneWidth = 6f; // The width of each lane
    //40f= 40 meters per second or frame.
    private void Start()
    {
      HowToPlay();
    }
    
   private void FixedUpdate()
    {
        Move();
    }

    private void HowToPlay()
        {
        Debug.Log("Welcome to the game!"); 
        Debug.Log("Use the WASD keys to move your character around.");
        // Code to display instructions on how to play the game
    }
    private void Move()
    {
        // Get input from the horizontal and vertical axes
        // and move the player accordingly
        // The Input.GetAxis function returns a value or float between -1 and 1
        //for both keyboard and joystick input devices
        // depending on the input from the player
        // The moveSpeed variable is used to control how fast the player moves
        // Time.deltaTime is used to make the movement frame rate independent
        float xValue = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed ;
        float zValue = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed ;
        //transform.Translate used to store and manipulate the scale,
        //rotation and position of an obj
        transform.Translate(xValue, 0, zValue);
        // Clamp the player's position to stay within the lanes
        float clampedX = Mathf.Clamp(transform.position.x, -laneWidth, laneWidth);
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
    }

    //FixedUpdate function is similarly to Update function
    //but differs in that it is called on a regular
    //timeline(i.e.the same time between call
    //The HowToPlay function simply prints to console a
    //message about how to play the game
}
