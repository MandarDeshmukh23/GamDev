using System.Globalization;
using Unity.Netcode;
using UnityEngine;

public class player_movement : NetworkBehaviour
{
    // for movement
    public Rigidbody2D rb;
    Vector2 movement;
    public float movespeed;

    // for collecting and releasing the stone 
    public GameObject stonePrefab;
    public bool holdingStone = false;
    private bool isNearPlayer = false;
    private bool isInMainStoneZone = false;
    private GameObject isnearbystone;

    public GameObject ballPrefab;
    public float ballForce = 10f;

    private GameObject isnearball = null;

    public bool holdingBall = false;


    public Transform ballHoldPoint; // Where ball is thrown from

    [SerializeField] private GameObject networkedBallPrefab;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // for movement
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");

        // ======= STONE LOGIC =======

        // Collect stone
        if (Input.GetKeyDown(KeyCode.C) && isNearPlayer && !holdingStone && !holdingBall)
        {
            holdingStone = true;
            if (isnearbystone != null)
            {
                Destroy(isnearbystone);
            }
        }

        // Release stone
        if (Input.GetKeyDown(KeyCode.Space) && holdingStone)
        {
            Instantiate(stonePrefab, transform.position, Quaternion.identity);
            holdingStone = false;
        }

        // Drop stone in main zone
        if (Input.GetKeyDown(KeyCode.Q) && isInMainStoneZone && holdingStone)
        {
            if (isnearbystone != null)
            {
                Destroy(isnearbystone);
            }
            holdingStone = false;
        }

        // ======= BALL LOGIC =======

        // Collect ball
        if (Input.GetKeyDown(KeyCode.B) && isNearPlayer && !holdingBall && !holdingStone)
        {
            holdingBall = true;
            if (isnearball != null)
            {
                Destroy(isnearball);

            }
        }

        // Shoot ball in facing direction
        if (Input.GetKeyDown(KeyCode.V) && holdingBall)
        {
            // Ensure only the local owner initiates the ServerRpc
            if (IsOwner)
            {
                ThrowBallServerRpc(ballHoldPoint.position, ballHoldPoint.right);
            }
            holdingBall = false;
        }
    }
    private void FixedUpdate()
    {
        
        rb.MovePosition(rb.position + movement * movespeed *  Time.deltaTime);
        UpdateFacingDirection();
    }

    void UpdateFacingDirection()
    {
        if (movement == Vector2.zero)
            return;  // No movement, no change

        // Face right
        if (movement.x > 0)
            transform.rotation = Quaternion.Euler(0, 0, -90);

        // Face left
        else if (movement.x < 0)
            transform.rotation = Quaternion.Euler(0, 0, 90);

        // Face up
        if (movement.y > 0)
            transform.rotation = Quaternion.Euler(0, 0, 0);

        // Face down
        else if (movement.y < 0)
            transform.rotation = Quaternion.Euler(0, 0, 180);
    }

    // ======= COLLISION DETECTION =======

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Stone"))
        {
            isNearPlayer = true;
            isnearbystone = collision.gameObject;
        }

        if (collision.gameObject.CompareTag("Ball"))
        {
            isNearPlayer = true;
            isnearball = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Stone"))
        {
            isNearPlayer = false;
            isnearbystone = null;
        }

        if (collision.gameObject.CompareTag("Ball"))
        {
            isNearPlayer = false;
            isnearball = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Main Stone"))
        {
            isInMainStoneZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Main Stone"))
        {
            isInMainStoneZone = false;
        }
    }

    [ServerRpc]
    void ThrowBallServerRpc(Vector3 spawnPos, Vector3 direction)
    {
        Debug.Log("ThrowBallServerRpc called");
        GameObject ball = Instantiate(networkedBallPrefab, spawnPos, Quaternion.identity);
        var netObj = ball.GetComponent<NetworkObject>();
        netObj.Spawn();

        Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction * ballForce;
        }
    }

}
