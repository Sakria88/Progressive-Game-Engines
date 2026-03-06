// This is the base class for all characters (player and NPCs). It provides common functionality like movement speed, teleportation, and resetting to start position.
using UnityEngine;

public abstract class CharacterBase : MonoBehaviour
{
    [Header("Shared Character Settings")]
    [SerializeField] public float moveSpeed = 10f;   // public so managers can set it



    protected Rigidbody rb;
    protected Vector3 startPosition;
    protected Quaternion startRotation;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    protected virtual void FixedUpdate()
    {
        Tick();
    }

    protected abstract bool Tick();

    // Teleports the character to a specific position and rotation. Returns true if successful.
    public bool TeleportTo(Vector3 position, Quaternion rotation)
    {
        if (rb != null && rb.isKinematic == false)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.position = position;
            rb.rotation = rotation;
        }
        else
        {
            transform.position = position;
            transform.rotation = rotation;
        }

        Physics.SyncTransforms();
        return true;
    }

    public bool ResetToStart()
    {
        return TeleportTo(startPosition, startRotation);
    }
}