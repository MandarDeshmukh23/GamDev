using UnityEngine;
using System.Collections;

public class BallScript : MonoBehaviour
{
    public float minX = -8.5f;
    public float maxX = 8.5f;
    public float minY = -4.5f;
    public float maxY = 4.5f;
    public float respawnDelay = 2f;
    private bool isRespawning = false;
    private Vector3 exitPosition;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isRespawning && IsOutOfBounds(transform.position))
        {
            exitPosition = transform.position;
            StartCoroutine(RespawnAfterDelay());
        }
    }

    bool IsOutOfBounds(Vector3 pos)
    {
        return pos.x < minX || pos.x > maxX || pos.y < minY || pos.y > maxY;
    }

    IEnumerator RespawnAfterDelay()
    {
        isRespawning = true;

        // Optional: hide and disable physics
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Rigidbody2D>().simulated = false;

        yield return new WaitForSeconds(respawnDelay);

        // Respawn at the point where it exited
        transform.position = exitPosition;
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<Rigidbody2D>().simulated = true;

        isRespawning = false;
    }
}
