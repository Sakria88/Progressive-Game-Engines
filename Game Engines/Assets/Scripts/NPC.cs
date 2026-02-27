using System.Collections;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public enum NPCMovementType
    {
        ForwardBackward,
        SideToSide
    }

    public NPCMovementType movementType;
    public float moveSpeed = 5f;
    public float moveRange = 5f;
    public float waitTime = 1f;

    private Vector3 startPos;
    private Vector3 targetPos;
    private bool movingToTarget = true;

    // Side-to-side limits (only used when movementType == SideToSide)
    private bool hasXLimits = false;
    private float minXLimit = 0f;
    private float maxXLimit = 0f;

    // Call this from NPC1Manager after instantiating to lock NPC1 to the floor width
    public void SetSideToSideLimits(float minX, float maxX)
    {
        hasXLimits = true;
        minXLimit = minX;
        maxXLimit = maxX;
    }

    private void Start()
    {
        startPos = transform.position;
        SetTargetPosition();
        StartCoroutine(MoveRoutine());
    }

    private void SetTargetPosition()
    {
        switch (movementType)
        {
            case NPCMovementType.ForwardBackward:
            {
                targetPos = startPos + transform.forward * moveRange;
                break;
            }
            case NPCMovementType.SideToSide:
            {
                float desiredTargetX = startPos.x + (transform.right.x >= 0f ? moveRange : -moveRange);

                if (hasXLimits)
                {
                    desiredTargetX = Mathf.Clamp(desiredTargetX, minXLimit, maxXLimit);
                }

                targetPos = new Vector3(desiredTargetX, startPos.y, startPos.z);
                break;
            }
        }
    }

    private IEnumerator MoveRoutine()
    {
        while (true)
        {
            Vector3 destination = movingToTarget ? targetPos : startPos;

            // If we have limits, keep both endpoints clamped (in case something changes at runtime)
            if (movementType == NPCMovementType.SideToSide && hasXLimits)
            {
                destination.x = Mathf.Clamp(destination.x, minXLimit, maxXLimit);
                Vector3 clampedStart = startPos;
                clampedStart.x = Mathf.Clamp(clampedStart.x, minXLimit, maxXLimit);
                startPos = clampedStart;
            }

            while (Vector3.Distance(transform.position, destination) > 0.05f)
            {
                Vector3 next = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);

                if (movementType == NPCMovementType.SideToSide && hasXLimits)
                {
                    next.x = Mathf.Clamp(next.x, minXLimit, maxXLimit);
                }

                transform.position = next;
                yield return null;
            }

            yield return new WaitForSeconds(waitTime);

            movingToTarget = !movingToTarget;

            // Optional: re-pick target each reversal if you want
            // (keeps it stable even if startPos was clamped)
            // SetTargetPosition();
        }
    }
}