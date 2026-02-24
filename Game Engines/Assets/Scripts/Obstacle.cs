using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor.VersionControl;
using UnityEngine;


public class Obstacle : MonoBehaviour
{
   public string typeOfItem; 
   void Awake()
    {
        // Set the tag to "Obstacle" so the CollisionHandler can identify it
        gameObject.tag = "Obstacle";
    }
    
  
}