// This script defines the behavior of the player character in the game.
// It handles player movement, jumping, and interactions with 
//collectibles and power-ups. 
using System.Collections;
using UnityEngine;
using DLLCollectables;
public class PlayerCharacter : CharacterBase
{
    [Header("Player Movement")]
    [SerializeField] private float forwardSpeed = 20f;
    [SerializeField] private float laneWidth = 11f;
    [SerializeField] private float normalMoveSpeed = 15f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float groundCheckDistance = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private UIManager uiManager;

    private float xInput;
    private bool jumpQueued;
    public bool isGrounded;

    private float normalForwardSpeed;
    
    public float NormalMoveSpeed => normalMoveSpeed;
    //coins collected 
    public int TotalCoinsCollected { get; private set; }

    // ===== Speed boost state =====
    private Coroutine speedRoutine;

    // ===== Shield state (FUNCTION ONLY) =====
    public bool IsShieldActive { get; private set; }
    private Coroutine shieldRoutine;

    public static PlayerCharacter Instance { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        Instance = this;

        normalForwardSpeed = forwardSpeed;
        normalMoveSpeed = moveSpeed;
    }

    private void Update()
    {
        xInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
            jumpQueued = true;
        Debug.Log("[Jump] Space pressed -> jumpQueued TRUE");
    }
    

    protected override bool Tick()
    {
        //Debug.Log($"[Tick] {name} id={GetInstanceID()} grounded={isGrounded} jumpQueued={jumpQueued} rb={(rb != null)}");
        Debug.Log($"[Tick] grounded={isGrounded} jumpQueued={jumpQueued} rb={(rb != null)}");
        isGrounded = CheckGrounded();
        bool moved = ApplyMovement();
        bool jumped = ApplyJump();
        return moved || jumped;
    }

    private bool ApplyMovement()
    {
        if (rb == null) return false;

        Vector3 move = new Vector3(xInput * moveSpeed, 0f, forwardSpeed) * Time.fixedDeltaTime;
        Vector3 targetPos = rb.position + move;

        targetPos.x = Mathf.Clamp(targetPos.x, -laneWidth, laneWidth);

        rb.MovePosition(targetPos);
        return true;
    }

    private bool ApplyJump()
    {
        //Debug.Log($"[Jump] Space pressed on {name} id={GetInstanceID()}");
        if (!jumpQueued)
        {
            Debug.Log("[Jump] ApplyJump: jumpQueued FALSE");
            return false;
        } 
        

       if (!isGrounded || rb == null)
        {
            Debug.Log($"[Jump BLOCKED] grounded={isGrounded} | rbNull={(rb == null)}");
            return false;
        }
        jumpQueued = false;

        Debug.Log("[Jump] Applying impulse");
        Vector3 v = rb.velocity;
        v.y = 0f;
        rb.velocity = v;

        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        return true;
    }

    private bool CheckGrounded()
    {
        bool grounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);
        Debug.DrawRay(transform.position, Vector3.down * groundCheckDistance, Color.red);
        return grounded;
    }

    public bool AddCoin(int amount)
    {
        TotalCoinsCollected += amount;

        //Debug.Log($"[Collectible] Coin Collected | Total Coins = {TotalCoinsCollected}");

        return true;
    }

    // ===== Speed boost =====
    public bool ActivateSpeedBoost(float multiplier, float duration)
    {
        if (speedRoutine != null)
        {
            StopCoroutine(speedRoutine);
            speedRoutine = null;

            forwardSpeed = normalForwardSpeed;
            moveSpeed = normalMoveSpeed;
        }

        speedRoutine = StartCoroutine(SpeedBoostRoutine(multiplier, duration));
        return true;
    }
    // public event System.Action<float> OnMoveSpeedChanged;

    // private IEnumerator SpeedBoostRoutine(float multiplier, float duration)
    // {
    //     moveSpeed = normalMoveSpeed * multiplier;
    //     forwardSpeed = normalForwardSpeed * multiplier;

    //     Debug.Log($"[SpeedBoost] Boosted moveSpeed = {moveSpeed}");
    //     OnMoveSpeedChanged?.Invoke(moveSpeed);

    //     yield return new WaitForSeconds(duration);

    //     moveSpeed = normalMoveSpeed;
    //     forwardSpeed = normalForwardSpeed;

    //     OnMoveSpeedChanged?.Invoke(moveSpeed);

    //     speedRoutine = null;
    // }

    private IEnumerator SpeedBoostRoutine(float multiplier, float duration)
    {
        forwardSpeed = normalForwardSpeed * multiplier;
        moveSpeed = normalMoveSpeed * multiplier;

        // Update UI immediately
        UIManager.Instance.RefreshSpeedUI();
        UIManager.Instance.ShowBoostMessage("Booster Activated", 1.1f);

        yield return new WaitForSeconds(duration);

        forwardSpeed = normalForwardSpeed;
        moveSpeed = normalMoveSpeed;

        // Update UI back to normal
        UIManager.Instance.RefreshSpeedUI();
        

        speedRoutine = null;
    }
    
    public float CurrentSpeed
    {
        get { return forwardSpeed; }
    }

    //property for current move speed (sideways)
    public float CurrentMoveSpeed
    {
        get { return moveSpeed; }
    }
    

    // ===== Shield (FUNCTION ONLY) =====
    public bool ActivateShield(float durationSeconds)
    {
        if (shieldRoutine != null)
        {
            StopCoroutine(shieldRoutine);
            shieldRoutine = null;
        }

        shieldRoutine = StartCoroutine(ShieldRoutine(durationSeconds));
        return true;
    }

    private IEnumerator ShieldRoutine(float durationSeconds)
    {
        IsShieldActive = true;
        Debug.Log($"[Shield] Activated for {durationSeconds:0.0}s");

        yield return new WaitForSeconds(durationSeconds);

        IsShieldActive = false;
        Debug.Log("[Shield] Ended");

        shieldRoutine = null;
    }
}