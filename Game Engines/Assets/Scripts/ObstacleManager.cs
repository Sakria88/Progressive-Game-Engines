using System.Collections.Generic;
using UnityEngine;


public class ObstacleManager : MonoBehaviour
{
    public Transform player;
    public List<GameObject> obstacleList = new List<GameObject>();

    void Start()
    {
        foreach (GameObject obj in obstacleList)
        {
            if (obj != null)
            {
                //Ensure the object has an Obstacle component 
                if (obj.GetComponent<Obstacle>() == null)
                {
                    obj.AddComponent<Obstacle>();
                }

                //Ensure the object has the "Obstacle" tag
                obj.tag = "Obstacle";

                //Ensure the object has a Collider so it can be hit
                if (obj.GetComponent<Collider>() == null)
                {
                    // Adds a BoxCollider by default if none exists
                    obj.AddComponent<BoxCollider>();
                    Debug.LogWarning(obj.name + " was missing a collider, one has been added.");
                }
            }
        }
    }
}