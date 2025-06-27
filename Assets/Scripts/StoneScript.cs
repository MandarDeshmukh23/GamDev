using UnityEngine;

public class StoneScript : MonoBehaviour
{
    public Rigidbody2D rb;

    // Map boundaries
    public float minX = -9f;
    public float maxX = 9f;
    public float minY = -5f;
    public float maxY = 5f;

    private bool hasStopped = false;

    void Update()
    {
        if (!hasStopped && IsOutOfBounds(transform.position))
        {
            StopStoneAtPosition(transform.position);
        }
    }

    bool IsOutOfBounds(Vector3 pos)
    {
        return pos.x < minX || pos.x > maxX || pos.y < minY || pos.y > maxY;
    }

    void StopStoneAtPosition(Vector3 pos)
    {
        hasStopped = true;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        transform.position = pos; // Already there, but ensures precision
    }

}
