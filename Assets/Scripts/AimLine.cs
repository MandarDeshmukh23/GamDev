using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class AimLine2D : MonoBehaviour
{
    public Transform ballHoldPoint; // Where the ball is held
    private bool isHoldingBall; // Set this from another script when needed
    public float aimLength = 3f; // Length of the aim line
    private Vector2 aimDirection = Vector2.right; // You can update this based on input
    public player_movement script;

    private LineRenderer line;

    private void Start()
    {
        line = GetComponent<LineRenderer>();
        isHoldingBall = script.holdingBall;
        
    }

    private void Update()
    {
        if (isHoldingBall)
        {
            line.enabled = true;
            aimDirection = transform.up;

            // Start from the hold point
            Vector3 start = ballHoldPoint.position;
            Vector3 end = start + (Vector3)(aimDirection.normalized * aimLength);

            line.positionCount = 2;
            line.SetPosition(0, start);
            line.SetPosition(1, end);
        }
        else
        {
            line.enabled = false;
        }
    }
}
