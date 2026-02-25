using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEditor.VersionControl;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private bool isGrounded;
    private float jumpForce = 7f;
    private float forwardSpeed = 10f;

    // private Rigidbody rb;
    // Start is called before the first frame update
    public float moveSpeed = 40f;
    public static PlayerController Instance;
    public float laneWidth = 11f; // The width of each lane
    //40f= 40 meters per second or frame.
    private float xInput;
    private bool jumpQueued;
    private void Start()
    {
      HowToPlay();
      rb = GetComponent<Rigidbody>();
        // rb = GetComponent<Rigidbody>();

        // // Safety check: If you forgot to add a Rigidbody in Unity, this will tell you
        // if (rb == null)
        // {
        //     Debug.LogError("Player is missing a Rigidbody component!");
        // }
    }
    
    private void Update()
    {
    // Read input here (reliable)
        xInput = Input.GetAxisRaw("Horizontal");  // A/D, Left/Right

        if (Input.GetKeyDown(KeyCode.Space))
            jumpQueued = true;
    }
   private void FixedUpdate()
    {
        Move();
        HandleJump();
        
    }

    private void HowToPlay()
        {
        Debug.Log("Welcome to the game!"); 
        Debug.Log("Use the WASD keys to move your character around.");
        // Code to display instructions on how to play the game
    }

    // private void Move()
    // {
    //     float direction = Input.GetAxis("Horizontal"); // Get horizontal mouse movement
    //     Vector3 lateralvelocity = rb.velocity;
    //     lateralvelocity.x = direction * moveSpeed; // Adjust the lateral velocity based on mouse movement
    //     rb.velocity = lateralvelocity; // Apply the new velocity to the Rigidbody
    // }
    private void Move()
    {
    Vector3 move = new Vector3(xInput * moveSpeed, 0f, forwardSpeed) * Time.fixedDeltaTime;

    Vector3 targetPos = rb.position + move;

    // clamp X so player stays in lanes
    targetPos.x = Mathf.Clamp(targetPos.x, -laneWidth, laneWidth);

    rb.MovePosition(targetPos);
    }

    private void HandleJump()
    {
        if (!jumpQueued) return;

        if (isGrounded)
        {
            // optional: reset vertical velocity so jump is consistent
            Vector3 v = rb.velocity;
            v.y = 0f;
            rb.velocity = v;

            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }

        jumpQueued = false;
    }

public void SetGrounded(bool grounded)
{
    isGrounded = grounded;
}

}
