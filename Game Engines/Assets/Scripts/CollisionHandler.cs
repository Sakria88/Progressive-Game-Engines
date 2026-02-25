using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;



public class CollisionHandler : MonoBehaviour
{
    public BackPack myBackpack;

    private Vector3 spawnPlayer;
    private PlayerController player;
    private Rigidbody rb;

    private bool hitObstacle = false;

    void Start()
    {
        player = GetComponent<PlayerController>();
        spawnPlayer = transform.position;   // ✅ YOU MUST HAVE THIS

        myBackpack = new BackPack(100);
        myBackpack.AddToBackpack();
        StartCoroutine(CheckObstacleTimer());
    }

    IEnumerator CheckObstacleTimer()
    {
        yield return new WaitForSeconds(5f);

        if (!hitObstacle)
            Debug.Log("5 seconds passed and no obstacle was hit!");
    }

    void OnCollisionEnter(Collision collision)
    {
        // Grounded for jump (only if you tag your floor "Ground")
        if (collision.gameObject.CompareTag("Ground"))
            if (player != null) player.SetGrounded(true);

        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Player collided with an enemy!");
            RespawnPlayer();
        }

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Player hit an obstacle!");
            hitObstacle = true;
            RespawnPlayer();
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            if (player != null) player.SetGrounded(true);
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            if (player != null) player.SetGrounded(false);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Touched something named: " + other.name + " with Tag: " + other.tag);

        if (other.CompareTag("Coin"))
        {
            Debug.Log("Player picked up a coin!");
            myBackpack.AddToBackpack();

            if (CollectiblesManager.Instance != null)
                CollectiblesManager.Instance.AddCoins(1);

            Destroy(other.gameObject);
        }

        if (other.CompareTag("SpeedBooster"))
        {
            Debug.Log("Speed boost collected!");
            Destroy(other.gameObject);
        }
    }

    void RespawnPlayer()
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        if (rb != null && !rb.isKinematic)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            rb.position = spawnPlayer;      // ✅ use rb.position (physics-safe)
            rb.rotation = Quaternion.identity; // optional, only if you want reset rotation
        }
        else
        {
            transform.position = spawnPlayer;
        }

        Physics.SyncTransforms();
        Debug.Log("Teleported to: " + spawnPlayer);
    }

}
