using UnityEngine;

/// <summary>
/// Example enemy AI that uses the advanced AI controller
/// Demonstrates patrol, chase, and attack behaviors
/// </summary>
public class EnemyAI : MonoBehaviour
{
    public enum AIState { Patrolling, Chasing, Attacking, Idle }
    
    [Header("References")]
    [SerializeField] private AdvancedAIController navigationController;
    [SerializeField] private Transform[] patrolWaypoints;
    
    [Header("Detection")]
    [SerializeField] private float detectionRange = 15f;
    [SerializeField] private string targetTag = "Player";
    
    [Header("Attack")]
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackCooldown = 1f;
    
    private AIState currentState = AIState.Patrolling;
    private int currentWaypointIndex = 0;
    private Transform targetEnemy;
    private float lastAttackTime = 0f;
    private float detectionTimer = 0.5f;
    private float detectionCounter = 0f;

    private void Start()
    {
        if (navigationController == null)
            navigationController = GetComponent<AdvancedAIController>();
        
        if (patrolWaypoints.Length > 0)
            StartPatrol();
    }

    private void Update()
    {
        detectionCounter -= Time.deltaTime;
        
        if (detectionCounter <= 0)
        {
            DetectTarget();
            detectionCounter = detectionTimer;
        }

        switch (currentState)
        {
            case AIState.Patrolling:
                PatrolBehavior();
                break;
            case AIState.Chasing:
                ChaseBehavior();
                break;
            case AIState.Attacking:
                AttackBehavior();
                break;
            case AIState.Idle:
                IdleBehavior();
                break;
        }
    }

    private void DetectTarget()
    {
        // Find target in range
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange);
        targetEnemy = null;

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag(targetTag))
            {
                targetEnemy = collider.transform;
                break;
            }
        }

        // Update state based on detection
        if (targetEnemy != null)
        {
            if (Vector3.Distance(transform.position, targetEnemy.position) < attackRange)
                currentState = AIState.Attacking;
            else
                currentState = AIState.Chasing;
        }
        else if (currentState == AIState.Chasing || currentState == AIState.Attacking)
        {
            currentState = AIState.Patrolling;
            StartPatrol();
        }
    }

    private void PatrolBehavior()
    {
        if (patrolWaypoints.Length == 0)
        {
            currentState = AIState.Idle;
            return;
        }

        Transform currentWaypoint = patrolWaypoints[currentWaypointIndex];
        navigationController.NavigateTo(currentWaypoint.position);

        if (navigationController.HasReachedTarget())
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % patrolWaypoints.Length;
        }
    }

    private void ChaseBehavior()
    {
        if (targetEnemy == null)
        {
            currentState = AIState.Patrolling;
            return;
        }

        navigationController.NavigateTo(targetEnemy.position);

        float distanceToTarget = Vector3.Distance(transform.position, targetEnemy.position);
        if (distanceToTarget < attackRange)
        {
            currentState = AIState.Attacking;
        }
    }

    private void AttackBehavior()
    {
        if (targetEnemy == null)
        {
            currentState = AIState.Patrolling;
            return;
        }

        float distanceToTarget = Vector3.Distance(transform.position, targetEnemy.position);

        if (distanceToTarget > attackRange * 1.5f)
        {
            currentState = AIState.Chasing;
            return;
        }

        // Face target
        Vector3 directionToTarget = (targetEnemy.position - transform.position).normalized;
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            Quaternion.LookRotation(directionToTarget),
            Time.deltaTime * 5f);

        // Attack if cooldown is ready
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            PerformAttack();
            lastAttackTime = Time.time;
        }
    }

    private void IdleBehavior()
    {
        // Just stand in place
    }

    private void PerformAttack()
    {
        // Implement your attack logic here
        Debug.Log("Enemy attacking: " + gameObject.name);
        
        // Example: Deal damage
        // if (targetEnemy.TryGetComponent<IDamageable>(out var damageable))
        //     damageable.TakeDamage(10);
    }

    private void StartPatrol()
    {
        currentState = AIState.Patrolling;
        currentWaypointIndex = 0;
    }

    public AIState GetCurrentState()
    {
        return currentState;
    }

    private void OnDrawGizmos()
    {
        // Draw detection range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Draw attack range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // Draw patrol waypoints
        if (patrolWaypoints != null && patrolWaypoints.Length > 0)
        {
            Gizmos.color = Color.green;
            for (int i = 0; i < patrolWaypoints.Length; i++)
            {
                Gizmos.DrawSphere(patrolWaypoints[i].position, 0.3f);
                if (i < patrolWaypoints.Length - 1)
                    Gizmos.DrawLine(patrolWaypoints[i].position, patrolWaypoints[i + 1].position);
            }
        }
    }
}
