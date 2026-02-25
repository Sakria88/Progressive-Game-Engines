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
                targetPos = startPos + transform.forward * moveRange;
                break;
            case NPCMovementType.SideToSide:
                targetPos = startPos + transform.right * moveRange;
                break;
        }
    }

    private IEnumerator MoveRoutine()
    {
        while (true)
        {
            Vector3 destination = movingToTarget ? targetPos : startPos;

            // Move towards the target
            while (Vector3.Distance(transform.position, destination) > 0.05f)
            {
                transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);
                yield return null;
            }

            // Wait at the end
            yield return new WaitForSeconds(waitTime);

            // Reverse direction
            movingToTarget = !movingToTarget;
        }
    }
}