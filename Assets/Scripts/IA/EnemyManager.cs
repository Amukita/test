using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : CharacterManager
{
    EnemyLocomotionManager enemyLocomotionManager;
    EnemyAnimatorManager enemyAnimatorManager;
    EnemyStats enemyStats;
    public NavMeshAgent navMeshAgent;
    public Rigidbody enemyRigidbody;

    public State currentState;
    public CharacterStats currentTarget;
    

    public bool isPerformingAction;

    public float rotationSpeed = 15;
    public float maximumAttackRange = 1.5f;


    [Header("Configs IA")]
    public float detectionRadius = 20;
    public float minDetectionAngle = 50;
    public float maxDetectionAngle = -50;
    public float currentRecoveryTime = 0;

    private void Awake()
    {
        enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
        enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
        enemyStats = GetComponent<EnemyStats>();
        enemyRigidbody = GetComponent<Rigidbody>();
        navMeshAgent = GetComponentInChildren<NavMeshAgent>();
        navMeshAgent.enabled = false;
    }

    private void Start()
    {
        enemyRigidbody.isKinematic = false;
    }
    private void Update()
    {
        HandleRecoveryTime();
    }

    private void FixedUpdate()
    {
        HandleStates();        
    }

    private void HandleStates()
    {
        if (currentState != null)
        {
            State nextState = currentState.Tick(this, enemyStats, enemyAnimatorManager);

            if(nextState != null)
            {
                SwitchToNextState(nextState);
            }
        }
    }

    private void HandleRecoveryTime()
    {
        if(currentRecoveryTime > 0)
        {
            currentRecoveryTime -= Time.deltaTime;
        }

        if (isPerformingAction)
        {
            if(currentRecoveryTime <= 0)
            {
                isPerformingAction = false;
            }
        }
    }

    private void SwitchToNextState(State state)
    {
        if (enemyStats.isDead == false)
        {
            currentState = state;
        }
        
    }
}
