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
    public float groundCheckDistance = 0.2f;
    public LayerMask groundLayer;
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
       
    }
    
    private void Update()
    {
    // Read input here (reliable)
        xInput = Input.GetAxisRaw("Horizontal");  // A/D, Left/Right

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space pressed!");
            jumpQueued = true;
        }

    }
   private void FixedUpdate()
    {
        Move();
        HandleJump();
        CheckGround();
        
    }

    private void HowToPlay()
        {
        Debug.Log("Welcome to the game!"); 
        Debug.Log("Use the WASD keys to move your character around.");
        // Code to display instructions on how to play the game
    }

    
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
        Debug.Log("JumpQueued: " + jumpQueued + " | Grounded: " + isGrounded);
        if (!jumpQueued) return;

        if (isGrounded)
        {
            Debug.Log("Jump executed!");

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
    private void CheckGround()
    {
        isGrounded = Physics.Raycast(
            transform.position,
            Vector3.down,
            groundCheckDistance,
            groundLayer
        );

        Debug.DrawRay(transform.position, Vector3.down * groundCheckDistance, Color.red);
    }

}
