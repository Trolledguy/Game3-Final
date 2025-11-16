using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Advanced AI controller combining pathfinding with steering behaviors
/// </summary>
public class AdvancedAIController : MonoBehaviour
{
    [Header("Navigation")]
    [SerializeField] private PathfindingGrid pathfindingGrid;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float stoppingDistance = 0.5f;
    [SerializeField] private float waypointCheckDistance = 1f;
    
    [Header("Vision")]
    [SerializeField] private float sightRange = 20f;
    [SerializeField] private float sightAngle = 90f;
    
    [Header("Avoidance")]
    [SerializeField] private float avoidanceRadius = 2f;
    [SerializeField] private LayerMask obstacleLayer;
    
    private Rigidbody rb;
    private List<Vector3> currentPath = new List<Vector3>();
    private int currentPathIndex = 0;
    private Vector3 currentTarget = Vector3.zero;
    private bool hasTarget = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (pathfindingGrid == null)
            pathfindingGrid = GameObject.FindAnyObjectByType<PathfindingGrid>();
    }

    private void FixedUpdate()
    {
        if (!hasTarget || currentPath.Count == 0)
            return;

        MoveAlongPath();
    }

    /// <summary>
    /// Navigate to a target position using pathfinding
    /// </summary>
    public void NavigateTo(Vector3 targetPosition)
    {
        currentTarget = targetPosition;
        hasTarget = true;
        currentPathIndex = 0;

        if (pathfindingGrid != null)
            currentPath = pathfindingGrid.FindPath(transform.position, targetPosition);
        else
            currentPath = new List<Vector3> { targetPosition };
    }

    private void MoveAlongPath()
    {
        if (currentPathIndex >= currentPath.Count)
        {
            hasTarget = false;
            rb.linearVelocity = Vector3.zero;
            return;
        }

        Vector3 targetWaypoint = currentPath[currentPathIndex];
        Vector3 directionToWaypoint = (targetWaypoint - transform.position).normalized;
        float distanceToWaypoint = Vector3.Distance(transform.position, targetWaypoint);

        // Avoidance steering
        Vector3 avoidanceDir = CalculateAvoidance(directionToWaypoint);
        Vector3 desiredDirection = (directionToWaypoint + avoidanceDir * 0.5f).normalized;

        // Move
        rb.linearVelocity = new Vector3(
            desiredDirection.x * moveSpeed,
            rb.linearVelocity.y,
            desiredDirection.z * moveSpeed);

        // Rotate
        if (desiredDirection.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(desiredDirection);
            rb.rotation = Quaternion.Lerp(rb.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }

        // Check waypoint progress
        if (distanceToWaypoint < waypointCheckDistance)
        {
            currentPathIndex++;
        }

        // Check final destination
        if (currentPathIndex >= currentPath.Count)
        {
            hasTarget = false;
            rb.linearVelocity = Vector3.zero;
        }
    }

    private Vector3 CalculateAvoidance(Vector3 desiredDirection)
    {
        Vector3 avoidance = Vector3.zero;
        
        // Check for obstacles in front
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, avoidanceRadius, desiredDirection, out hit, avoidanceRadius * 2, obstacleLayer))
        {
            // Calculate perpendicular direction
            Vector3 normal = hit.normal;
            avoidance = normal * (1f - (hit.distance / (avoidanceRadius * 2)));
        }

        return avoidance;
    }

    /// <summary>
    /// Check if target is visible (within sight range and angle)
    /// </summary>
    public bool CanSeeTarget(Vector3 targetPos)
    {
        float distanceToTarget = Vector3.Distance(transform.position, targetPos);
        if (distanceToTarget > sightRange)
            return false;

        Vector3 directionToTarget = (targetPos - transform.position).normalized;
        float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

        return angleToTarget < sightAngle / 2f;
    }

    public bool HasReachedTarget()
    {
        return !hasTarget;
    }

    public float GetPathProgress()
    {
        return currentPath.Count > 0 ? (float)currentPathIndex / currentPath.Count : 1f;
    }
}
