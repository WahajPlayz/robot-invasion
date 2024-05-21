using System.Collections;
using cowsins;
using UnityEngine;
using UnityEngine.AI;
using Color = System.Drawing.Color;

public class CowsinsAI : MonoBehaviour
{
    #region Base Variables
    [Tooltip("Displays Current State (WILL NOT SAVE CHANGES MADE IN EDITOR MODE WHEN ENTERING PLAY)")]
    public States currentState;

    [Tooltip("Whether the AI should attack the player")]
    public bool dumbAI = false;

    [Tooltip("How long the AI should wait until going to the next waypoint")]
    public float waypointWaitTime = 1f;

    [Tooltip("If the AI should use waypoints, randomly move around or stand idle")]
    public MoveMode moveMode;

    public Waypoints _waypoints;

    [Tooltip("The animator the AI will use for Shooter Mode")]
    public Animator shooterAnimator;

    [Tooltip("The animator the AI will use if AI Type is NPC")]
    public Animator NPCAnimator;

    [Tooltip("The damage dealt to the player")]
    public float damageAmount;

    public enum States
    {
        Idle,
        Attack,
        Search
    }

    public enum MoveMode
    {
        Waypoints,
        Random,
        Idle
    }

    public bool useRagdoll;

    public bool useDeathAnimation;

    public bool destroyAfterTime;

    public float destroyTimer;
    
    #endregion

    #region Wander Settings

    private Vector3 destination;

    private bool wandering;
    public float waitTimeBetweenWander = 2f;
    private float wanderWaitTimer = 0f;
    private float wanderWaitTime = 5f;

    private Vector3 idleStartPos;
    private Quaternion idleStartRot;

    [Tooltip("How far the AI should wander until changing path")]
    public float wanderRadius = 10f;

    [Tooltip("Minimum distance it should go until moving again")]
    public float minWanderDistance = 2f;

    [Tooltip("Maximum distance it should go until moving again")]
    public float maxWanderDistance = 5f;

    #endregion

    #region Location Variables

    [Tooltip("The radius in what the AI can see")]
    public float searchRadius;

    [Range(0, 360)] [Tooltip("AI FOV (Field of View)")]
    public float searchAngle;

    public bool increaseSightOnAttack;

    [Range(0, 360)] [Tooltip("What FOV the AI will have after attack")]
    public float attackSearchAngle;

    [HideInInspector] public GameObject player;

    [Tooltip("The layer in which the AI will attack")]
    public LayerMask targetMask;
    
    [Tooltip("What the raycast will hit")] public LayerMask hitMask;

    [Tooltip("The layer in which the AI cannot see through")]
    public LayerMask obstructionMask;

    [Tooltip("Debug variable, changing will not make any difference in Edit Mode / Play Mode")]
    public bool canSeePlayer;

    [Tooltip("How long the AI will spend searching for the player after losing sight")]
    public float searchWaitTime;

    #endregion

    #region Shooter Settings

    public bool usingHitscan;

    public bool usingProjectile;

    public Transform weaponHolder;

    [Tooltip("How far the AI should start shooting the target from")]
    public float shootDistance;

    [Tooltip("How long the AI should wait inbetween each shot")]
    public float timeBetweenShots;

    [Tooltip("The projectile prefab that the AI will use")]
    public GameObject projectile;

    [Tooltip("Where the bullet will shoot from")]
    public Transform firePoint;

    [Tooltip("Prefab that has a 'TrailRenderer' on. View the 'Prefabs' folder for example")]
    public TrailRenderer bulletTrail;

    [Tooltip("How much the bullets should spread")]
    public float spreadAmount;
    
    [Tooltip("Shooting Method")] public ShooterType type;

    [Tooltip("AI Type")] public AIType aiType;

    public enum ShooterType
    {
        Projectile,
        Hitscan
    }

    public enum AIType
    {
        Enemy,
        NPC
    }

    public bool inShootingDistance;

    private bool attackedShooterHitscan;
    private bool attackedShooterProjectile;

    #endregion

    #region Melee Variables

    [Tooltip("The animator that the AI will use for melee")]
    public Animator meleeAnimator;

    [Tooltip("How long the AI will wait inbetween attacks")]
    public float waitBetweenAttack;

    public float waitBetweenSwingDelay;

    [Tooltip("How far the AI will stand from the player whilst attacking")]
    public float meleeDistance;

    private bool alreadyAttackedMelee;
    public bool inMeleeDistance;

    #endregion
    
    #region Hidden Variables

    private float waypointWaitTimer = 0f;

    private int currentWaypoint;

    private NavMeshAgent agent;

    private float searchTimer = 5;

    private float currentSearchTime;
    
    private bool searchTimerStarted = false;

    #endregion

    #region Debug

    #region Bools

    public bool shootingDistanceDebug;
    public bool meleeDistanceDebug;
    public bool canSeePlayerDebug;

    public bool searchRadiusDebug;
    public bool attackRadiusDebug;

    #endregion Bools

    #region Variables

    public EnemyType _enemyType;
    public enum EnemyType{Shooter, Melee}

    /*public bool melee;
    public bool shooter;*/

    #endregion Variables

    #endregion

    private void Start()
    {
        currentState = States.Idle;
        agent = gameObject.GetComponentInChildren<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FOVRoutine());

        if (moveMode == MoveMode.Idle)
        {
            idleStartPos = transform.position;
            idleStartRot = transform.rotation;
        }
    }

    #region FOV Manager

    IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, searchRadius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < searchAngle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    canSeePlayer = true;
                }
                else
                {
                    canSeePlayer = false;
                }
            }
            else
            {
                canSeePlayer = false;
            }
        }
        else if (canSeePlayer)
        {
            canSeePlayer = false;
        }
    }

    #endregion

    private void Update()
    {
        if (aiType == AIType.NPC)
        {
            IdlePassiveState();
        }
        
        if (currentState == States.Idle && aiType == AIType.Enemy)
        {
            IdleHostileState();
        }
        else if (currentState == States.Search && aiType == AIType.Enemy)
        {
            SearchHostileState();
        }
        else if (currentState == States.Attack && aiType == AIType.Enemy)
        {
            AttackState();
        }

        if (currentState == States.Attack && canSeePlayer == false)
        {
            currentState = States.Search;
        }
    }

    void IdlePassiveState()
    {
        if (moveMode == MoveMode.Waypoints)
        {
            WaypointsFunction();
        }
        else if (moveMode == MoveMode.Random)
        {
            RandomMove();
        }
        else if (moveMode == MoveMode.Idle)
        {
            Idle();
        }
        
        if (agent.velocity != Vector3.zero)
        {
            NPCAnimator.SetBool("isWalking", true);
        }
        else if (agent.velocity == Vector3.zero)
        {
            NPCAnimator.SetBool("isWalking", false);
        }
    }

    void Idle()
    {
        agent.destination = idleStartPos;
        transform.rotation = idleStartRot;
    }

    void IdleHostileState()
    {
        if (!dumbAI)
        {
            agent.isStopped = false;

            if (moveMode == MoveMode.Waypoints)
            {
                WaypointsFunction();
            }
            else if (moveMode == MoveMode.Random)
            {
                RandomMove();
            }
            
            FieldOfViewCheck();

            if (canSeePlayer)
            {
                currentState = States.Attack;
            }

            if (_enemyType == EnemyType.Shooter)
            {
                if (agent.velocity != Vector3.zero)
                {
                    shooterAnimator.SetBool("isWalking", true);
                }
                else if (agent.velocity == Vector3.zero)
                {
                    shooterAnimator.SetBool("isWalking", false);
                }
            }

            if (_enemyType == EnemyType.Melee)
            {
                if (agent.velocity != Vector3.zero)
                {
                    meleeAnimator.SetBool("isWalking", true);
                }
                else if (agent.velocity == Vector3.zero)
                {
                    meleeAnimator.SetBool("isWalking", false);
                }
            }
        }
    }

    void SearchHostileState()
    {
        agent.isStopped = false;

        RandomMove();

        if (!searchTimerStarted)
        {
            currentSearchTime = searchTimer;
            searchTimerStarted = true;
        }

        if(_enemyType == EnemyType.Shooter)
        {
            if (agent.velocity != Vector3.zero)
            {
                shooterAnimator.SetBool("combatWalk", true);

                if (shooterAnimator.GetBool("isWalking"))
                {
                    shooterAnimator.SetBool("isWalking", false);
                }

                shooterAnimator.SetBool("combatIdle", false);
            }
            else if (agent.velocity == Vector3.zero)
            {
                shooterAnimator.SetBool("combatWalk", false);
                shooterAnimator.SetBool("combatIdle", true);
            }

            currentSearchTime -= Time.deltaTime;

            if (currentSearchTime <= 0)
            {
                currentState = States.Idle;

                shooterAnimator.SetBool("combatIdle", false);
                shooterAnimator.SetBool("combatWalk", false);

                shooterAnimator.SetBool("isWalking", false);

                if (moveMode == MoveMode.Idle)
                {
                    agent.destination = idleStartPos;
                    transform.rotation = idleStartRot;
                }
            }
        }
        
        else if (_enemyType == EnemyType.Melee)
        {
            if (agent.velocity != Vector3.zero)
            {
                meleeAnimator.SetBool("isWalking", true);

                if (meleeAnimator.GetBool("isWalking"))
                {
                    meleeAnimator.SetBool("isWalking", false);
                }
                
                meleeAnimator.SetBool("isWalking", true);
            }
            else if (agent.velocity == Vector3.zero)
            {
                meleeAnimator.SetBool("isWalking", false);
            }

            currentSearchTime -= Time.deltaTime;

            if (currentSearchTime <= 0)
            {
                currentState = States.Idle;
                
                meleeAnimator.SetBool("isWalking", false);
            }
        }

        if (canSeePlayer)
        {
            currentState = States.Attack;
        }

        if (increaseSightOnAttack)
        {
            searchAngle = attackSearchAngle;
        }
    }

    void WaypointsFunction()
    {
        if (agent.remainingDistance < 0.5f)
        {
            waypointWaitTimer += Time.deltaTime;
            if (waypointWaitTimer >= waypointWaitTime)
            {
                currentWaypoint++;
                
                if (currentWaypoint >= _waypoints.waypoints.Count)
                {
                    currentWaypoint = 0;
                }
                
                agent.destination = _waypoints.waypoints[currentWaypoint].position;
                waypointWaitTimer = 0f;
            }
        }
    }

    #region Wander Mechanics

    void RandomMove()
    {
        if (!wandering)
        {
            wanderWaitTimer -= Time.deltaTime;

            if (wanderWaitTimer <= 0f)
            {
                destination = RandomNavSphere(transform.position, wanderRadius, -1);
                agent.SetDestination(destination);
                wandering = true;
                wanderWaitTimer = waitTimeBetweenWander;
            }
        }
        
        if (agent.remainingDistance <= minWanderDistance)
        {
            wandering = false;
        }

    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layerMask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layerMask);

        return navHit.position;
    }

    #endregion

    #region Attack States

    void AttackState()
    {
        if (_enemyType == EnemyType.Shooter)
        {
            ShooterAttack();
            CheckShooterBool();
        }
        
        else if (_enemyType == EnemyType.Melee)
        {
            MeleeAttack();
        }

        if (!canSeePlayer)
        {
            currentState = States.Search;
        }

        if (increaseSightOnAttack)
        {
            searchAngle = attackSearchAngle;
        }
    }
    
    // Shooter Functions

    void ResetAttack()
    {
        if (_enemyType == EnemyType.Shooter)
        {
            attackedShooterHitscan = false;

            shooterAnimator.SetBool("firing", false);
        }
        
        else if (_enemyType == EnemyType.Melee)
        {
            alreadyAttackedMelee = false;
            
            meleeAnimator.SetBool("attacking", false);
        }
    }

    void CheckShooterBool()
    {
        if (inShootingDistance)
        {
            agent.SetDestination(transform.position);
            transform.LookAt(
                new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));

            if (!attackedShooterHitscan && type == ShooterType.Hitscan)
            {
                Hitscan();
                attackedShooterHitscan = true;
                Invoke(nameof(ResetAttack), timeBetweenShots);
                shooterAnimator.SetBool("firing", true);
                shooterAnimator.SetBool("isWalking", false);
            }

            if (!attackedShooterProjectile && type == ShooterType.Projectile)
            {
                ProjectileShoot();
                attackedShooterProjectile = true;
                Invoke(nameof(ResetAttack), timeBetweenShots);
                shooterAnimator.SetBool("firing", true);
                shooterAnimator.SetBool("isWalking", false);
            }
        }
        else
        {
            agent.isStopped = false;
            shooterAnimator.SetBool("firing", false);
        }
    }
    
    void ShooterAttack()
    {
        if (!dumbAI)
        {
            if (agent.velocity != Vector3.zero)
            {
                shooterAnimator.SetBool("combatWalk", true);
                shooterAnimator.SetBool("isWalking", false);
                shooterAnimator.SetBool("combatIdle", false);
            }
            else if (agent.velocity == Vector3.zero)
            {
                shooterAnimator.SetBool("combatWalk", false);
                shooterAnimator.SetBool("combatIdle", true);
            }

            agent.destination = player.transform.position;
        }

        float distanceToPlayer = Vector3.Distance(player.transform.position, agent.transform.position);

        if (distanceToPlayer <= shootDistance)
        {
            inShootingDistance = true;
        }
        else if (distanceToPlayer >= shootDistance)
        {
            inShootingDistance = false;
        }
    }

    void Hitscan()
    {
        Ray ray = new Ray(firePoint.position, GetSpreadDirection(spreadAmount, transform.forward));

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, shootDistance, hitMask))
        {
            if (hit.transform.gameObject.CompareTag("Player"))
            {
                TrailRenderer trail = Instantiate(bulletTrail, firePoint.position, Quaternion.identity);

                StartCoroutine(SpawnTrail(trail, hit));

                hit.transform.gameObject.GetComponent<PlayerStats>().Damage(damageAmount);
                Debug.Log(hit.transform.name);
            }
        }
    }

    public static Vector3 GetSpreadDirection(float amount, Vector3 dir)
    {
        float horSpread = Random.Range(-amount, amount);
        float verSpread = Random.Range(-amount, amount);
        Vector3 finalDir = new Vector3(dir.x * horSpread, dir.y * verSpread, dir.z);

        return finalDir;
    }

    IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit)
    {
        float time = 0;
        Vector3 startPosition = trail.transform.position;

        while (time < 1)
        {
            trail.transform.position = Vector3.Lerp(startPosition, hit.point, time);
            time += Time.deltaTime / trail.time;

            yield return null;
        }

        trail.transform.position = hit.point;
        
        Destroy(trail.gameObject, trail.time);
    }

    void ProjectileShoot()
    {
        Rigidbody rb = Instantiate(projectile, firePoint.transform.position, Quaternion.identity)
            .GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
        rb.AddForce(transform.up * 2f, ForceMode.Impulse);
    }
    
    // Melee Functions

    void MeleeAttack()
    {
        if (agent.velocity != Vector3.zero)
        {
            meleeAnimator.SetBool("isWalking", true);
        }
        else if (agent.velocity == Vector3.zero)
        {
            meleeAnimator.SetBool("isWalking", false);
        }

        float distanceToPlayer = Vector3.Distance(player.transform.position, agent.transform.position);

        agent.destination = player.transform.position;

        if (distanceToPlayer <= meleeDistance)
        {
            inMeleeDistance = true;
            agent.isStopped = true;

            if (agent.velocity != Vector3.zero)
            {
                meleeAnimator.SetBool("isWalking", true);
            }
            else if (agent.velocity == Vector3.zero)
            {
                meleeAnimator.SetBool("isWalking", false);
            }

            if (!alreadyAttackedMelee)
            {
                alreadyAttackedMelee = true;
                
                meleeAnimator.SetBool("attacking", true);
                Invoke(nameof(ResetAttack), waitBetweenAttack);
            }
        }
        else if (distanceToPlayer >= meleeDistance)
        {
            inMeleeDistance = false;
            agent.isStopped = false;
            
            meleeAnimator.SetBool("attacking", false);
        }
    }

    public void MeleeDamage()
    {
        if (_enemyType == EnemyType.Melee && inMeleeDistance)
        {
            player.transform.gameObject.GetComponent<PlayerStats>().Damage(damageAmount);
            
            Debug.Log("Damaging " + player.name);
        }
    }

    #endregion
}