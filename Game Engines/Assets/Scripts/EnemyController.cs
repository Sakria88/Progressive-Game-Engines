using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //30f= 30 meters per second or frame.
    //Generally slower than player to give the player a chance to escape or fight back.
    public float moveSpeed = 30f;
    // Reference to the player's transform
    public Transform player;
    
    void Start()
    {
       
    }

    void FixedUpdate()
    {
      if (player != null)
        {
            ChasePlayer();
        }
    }

    void ChasePlayer()
    {
        //Calculate the step based on the moveSpeed and the time between frames
        float step = moveSpeed * Time.deltaTime;
        //MoveTowards( Current Position, Target Positionb, Max Distance Delta)
        transform.position = Vector3.MoveTowards(transform.position, player.position, step);
        //Make the enemy look at the player
        //transform.LookAt(player.transform.position)- specific coordinates of the player to look at
        //transform.LookAt(player) - the entire player object to look at
        transform.LookAt(player);
    }
    // Start is called before the first frame update
  
}
