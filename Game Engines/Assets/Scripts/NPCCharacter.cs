// This script defines the behavior of a 
//non-player character (NPC) that moves in a 
//simple pattern (side to side or forward and backward) 
//around an anchor point. The movement is based on a sine 
//wave to create smooth oscillation, and the NPC can be 
//configured to wait for a certain time before starting to move. 
//The NPC's position is updated in the Tick method, which is 
//called every fixed update cycle.
using UnityEngine;

public class NPCCharacter : CharacterBase
{
    public enum NPCMovementType
    {
        SideToSide,
        ForwardBackward
    }

    [Header("NPC Movement")]
    public NPCMovementType movementType = NPCMovementType.SideToSide;
    public float moveRange = 7f;
    public float waitTime = 0f;

    private Vector3 anchorPos;
    private float t;

    protected override void Awake()
    {
        base.Awake();
        anchorPos = transform.position;
        t = Random.Range(0f, 999f);
    }

    protected override bool Tick()
    {
        t += Time.fixedDeltaTime * Mathf.Max(0.01f, moveSpeed);

        float offset = Mathf.Sin(t) * moveRange;
        Vector3 target = anchorPos;

        if (movementType == NPCMovementType.SideToSide) target.x += offset;
        else target.z += offset;

        if (rb != null && rb.isKinematic == false) rb.MovePosition(target);
        else transform.position = target;

        return true;
    }
}