using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
public class CollectibleItem : MonoBehaviour
{
    // Matches itemName in your BackPack class to identify the 
    // type of item in the backpack, 
    // such as "Coin" or "Health Potion"


    //Array used to hold multiple strings
    public string [] typeOfItem = {"Coin", "Speed Booster" , "Health Potion" , "Damage Booster" , "Shield" , "Key"}; 
    
    private Vector3 startPos;
    public float floatFrequency = 1f; // Speed of floating
    public float floatAmplitude = 0.5f; // Height of floating

    void Start()
    {
        // Store the starting position to handle the floating effect
        startPos = transform.position;
    }

    //the argument: Vector3.up helps the rotation on the Y-axis. 
    // Vector3.up refers to the Global up, this means
    // no matter how and where you look at the object the value 
    //returned will always be a normalized positive value in 
    // the Y-axis direction. 
    // Space.World refers to movement relative to the world around 
    // the object, think of it as the coordinate system for the entire scene. 
    void Update()
    {
        transform.Rotate(Vector3.up, Space.World);
        // Make the item float up and down
        Vector3 tempPos = startPos;
        tempPos.y += Mathf.Sin(Time.fixedTime * floatFrequency) * floatAmplitude;
        transform.position = tempPos;
    }
    
}