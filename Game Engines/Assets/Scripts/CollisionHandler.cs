// This script handles collisions for the player character. 
//It manages interactions with enemies, NPCs, and obstacles, 
//including respawning the player at the start position when necessary. 
//It also integrates with the player's backpack system and resets relevant 
//game elements upon respawn. 
using System.Collections;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    public BackPack myBackpack;

    private Vector3 spawnPlayer;
    public InfiniteFloor[] floors;

    [SerializeField] private EnemyCharacter enemyCharacter;

    private bool hitObstacle;

    private void Start()
    {
        spawnPlayer = transform.position;

        myBackpack = new BackPack(100);
        myBackpack.AddToBackpack();

        StartCoroutine(CheckObstacleTimer());
    }

    private IEnumerator CheckObstacleTimer()
    {
        yield return new WaitForSeconds(5f);
        if (!hitObstacle) Debug.Log("5 seconds passed and no obstacle was hit!");
    }

    private void OnCollisionEnter(Collision collision)
    {
        PlayerCharacter player = GetComponent<PlayerCharacter>();

        // Enemy (Character-derived) - still respawns (you did NOT ask to block this)
        if (collision.gameObject.GetComponent<EnemyCharacter>() != null)
        {
            StartCoroutine(RespawnRoutine());
            return;
        }

        // NPC (Character-derived) - block respawn if shield active
        if (collision.gameObject.GetComponent<NPCCharacter>() != null)
        {
            if (player != null && player.IsShieldActive) return;
            StartCoroutine(RespawnRoutine());
            return;
        }

        // Obstacle - block respawn if shield active
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            if (player != null && player.IsShieldActive) return;

            hitObstacle = true;
            RespawnPlayer();
        }
    }

    private IEnumerator RespawnRoutine()
    {
        yield return null;
        RespawnPlayer();
    }

    private bool RespawnPlayer()
    {
        PlayerCharacter player = GetComponent<PlayerCharacter>();
        if (player != null)
        {
            player.TeleportTo(spawnPlayer, Quaternion.identity);
        }
        else
        {
            transform.position = spawnPlayer;
            Physics.SyncTransforms();
        }

        if (enemyCharacter != null)
        {
            enemyCharacter.ResetEnemy();
        }

        if (floors != null)
        {
            foreach (InfiniteFloor f in floors)
            {
                if (f != null) f.ResetFloor();
            }
        }

        return true;
    }
}