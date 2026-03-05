using UnityEngine;

public class EnemyCharacter : CharacterBase
{
    [Header("Enemy")]
    [SerializeField] private Transform player;
    [SerializeField] private float chaseDelaySeconds = 3f;

    private bool isChasing;

    protected override void Awake()
    {
        base.Awake();
        isChasing = false;
        Invoke(nameof(EnableChasing), chaseDelaySeconds);
    }

    protected override bool Tick()
    {
        if (!isChasing) return false;
        if (player == null) return false;

        float step = moveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, player.position, step);
        transform.LookAt(player);
        return true;
    }

    private void EnableChasing()
    {
        isChasing = true;
    }

    public bool ResetEnemy()
    {
        CancelInvoke();
        isChasing = false;
        bool ok = ResetToStart();
        Invoke(nameof(EnableChasing), chaseDelaySeconds);
        return ok;
    }
}