using System.Collections;
using UnityEngine;

/// <summary>
/// Example enemy AI that uses the advanced AI controller
/// Demonstrates patrol, chase, and attack behaviors
/// </summary>


public abstract class AIAutoControl : Entity , ICharacter
{
    public enum AIState { Patrolling, Chasing, Attacking, Idle }
    

    [Header("AI Stats")]
    public int maxHealth { get; set; } = 50;
    public int currentHealth { get; set; } = 50;
    public int baseAttackDamage { get; set; } = 5;
    public int buffAttackDamage { get; set; } = 0;

    public int _currentAttackDamage
    {
        get
        {
            return baseAttackDamage + buffAttackDamage;
        }
    }
    
    public float moveSpeed = 3f;

    [Header("References")]
    [SerializeField] private AdvancedAIController navigationController;
    [SerializeField] private Transform[] patrolWaypoints;
    [SerializeField] private Color patrolColor;
    
    [Header("Detection")]
    [SerializeField] private float detectionRange = 15f;
    [SerializeField] private string targetTag = "Player"; // Tag of the target to detect Change later
    
    [Header("Attack Mechanics")]
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackCooldown = 1f;




    
    private AIState currentState = AIState.Patrolling;
    private int currentWaypointIndex = 0;

    int _currentWaypointIndex
    {
        get { return currentWaypointIndex; }
        set
        {
            if(!(value < 0 || value >= patrolWaypoints.Length))
            {
                currentWaypointIndex = value;
            }

        }
    }

    private Transform targetEnemy;
    private float lastAttackTime = 0f;
    private float detectionTimer = 0.5f;
    private float detectionCounter = 1.5f;

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

        //eAnim.SetFloat("Speed", eRigi.linearVelocity.magnitude);

        Debug.Log("Current State: " + currentState.ToString());
        Debug.Log($"Path Progress : {navigationController.GetPathProgress()}");
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

    protected virtual void DetectTarget()
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

    protected virtual void PatrolBehavior()
    {
        if (patrolWaypoints.Length == 0)
        {
            currentState = AIState.Idle;
            return;
        }

        Transform currentWaypoint = patrolWaypoints[_currentWaypointIndex];
        navigationController.NavigateTo(currentWaypoint.position);

        if (navigationController.HasReachedTarget())
        {
            Debug.Log( this.name + "Reached waypoint : " + _currentWaypointIndex);
            for(float t = 0; t < 1f; t += Time.deltaTime)
            {
                Debug.Log("Delay");
            }
            _currentWaypointIndex++;
            return;
        }
    }

    protected virtual void ChaseBehavior()
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

    protected virtual void AttackBehavior()
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
            Attack();
            lastAttackTime = Time.time;
        }
    }

    private void IdleBehavior()
    {
        // Just stand in place
    }

    public virtual IEnumerator Attack()
    {
        // Implement your attack logic here
        Debug.Log("Enemy attacking: " + gameObject.name);
        
        yield break;
    }
    
    public void TakeDamage(int _amount)
    {
        currentHealth -= _amount;
        if (currentHealth <= 0)
        {
            Debug.Log("Enemy defeated: " + gameObject.name);
            Destroy(gameObject);
        }
    }

    public void Heal(int _amount)
    {
        currentHealth += _amount;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
        Debug.Log($"Enemy healed for {_amount}. Current Health: {currentHealth}/{maxHealth}");
    }

    private void StartPatrol()
    {
        Debug.Log("Starting Patrol");
        currentState = AIState.Patrolling;
        _currentWaypointIndex = 0;
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
            Gizmos.color = patrolColor;
            for (int i = 0; i < patrolWaypoints.Length; i++)
            {
                Gizmos.DrawSphere(patrolWaypoints[i].position, 0.3f);
                if (i < patrolWaypoints.Length - 1)
                    Gizmos.DrawLine(patrolWaypoints[i].position, patrolWaypoints[i + 1].position);
            }
        }
    }
}
