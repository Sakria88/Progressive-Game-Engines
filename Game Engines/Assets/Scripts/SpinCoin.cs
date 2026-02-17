using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public class SpinCoin : MonoBehaviour
{
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
    }
}
