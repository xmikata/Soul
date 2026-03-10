using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : CharacterManager
{
    EnemyLocomotionManager enemyLocomotionManager;
    public EnemyAnimatorManager enemyAnimatorManager;
    EnemyStats enemyStats;
    Animator animator;

    public State currentState;
    public CharacterStats currentTarget;
    public NavMeshAgent navMeshAgent;
    public Rigidbody enemyRigidBody;

    public EnemyStats deadState;

    public bool idle;
    public bool isPreformingAction;
    //public float distanceFromTarget;
    public float rotationSpeed = 15;
    public float maximumAggroRadius = 1.5f;

    public bool isUsingRightHand;
    public bool isUsingLeftHand;

    [Header("Combat Flag")]
    public bool canDoCombo;
    public bool isInteracting;

    //public EnemyAttackAction[] enemyAttacks;
    //public EnemyAttackAction currentAttack;

    [Header("AI设置")]
    public float detectionRadius=20;
    public float maximumDetectionAngle=50;
    public float minimumDetectiongAngle=-50;
    public int attackDamage;
    //public float viewableAngle;

    [Header("A.I战斗设置")]
    public bool allowAIToPerformCombos;
    public bool isPhaseShifting;
    public float comboLikelyHood;


    public float currentRecoveryTime = 0;
    private void Awake()
    {
        enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
        enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
        enemyStats = GetComponent<EnemyStats>();
        navMeshAgent = GetComponentInChildren<NavMeshAgent>();
        navMeshAgent.enabled = false;
        enemyRigidBody = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {

        enemyRigidBody.isKinematic = false;
    }

    private void Update()
    {
        HandleRecoveryTimer();
        HandleStateMachine();

        isRotatingWithRootMotion = enemyAnimatorManager.anim.GetBool("isRotatingWithRootMotion");
        isUsingRightHand = animator.GetBool("isUsingRightHand");
        enemyAnimatorManager.anim.SetBool("isDead", enemyStats.isDead);
        isInteracting = animator.GetBool("isInteracting");
        if (enemyStats.isBoss)
        {
            isPhaseShifting = animator.GetBool("isPhaseShifting");
        }
        isInvulnerable = animator.GetBool("isInvulnerable");
        canDoCombo = animator.GetBool("canDoCombo");
        canRotate = enemyAnimatorManager.anim.GetBool("canRotate");
        canBeRiposted = animator.GetBool("canBeRiposted");
    }

    private void FixedUpdate()
    {
        navMeshAgent.transform.localPosition = Vector3.zero;
        navMeshAgent.transform.localRotation = Quaternion.identity;
    }

    private void HandleStateMachine()
    {
        if (currentState!=null)
        {
            State nextState = currentState.Tick(this, enemyStats, enemyAnimatorManager);


            if (nextState!=null)
            {
                SwitchToNextState(nextState);
            }  
        }
    }

    private void SwitchToNextState(State state)
    {
        currentState = state;
    }

    private void HandleRecoveryTimer()
    {
        if (currentRecoveryTime > 0)
        {
            currentRecoveryTime -= Time.deltaTime;
        }

        //if (isPreformingAction)
        //{
        //    if (currentRecoveryTime <= 0)
        //    {
        //        isPreformingAction = false;
        //        currentRecoveryTime = currentAttack.recoveryTime;
        //        enemyAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
        //    }
        //}

        if (isPreformingAction)
        {
            if (currentRecoveryTime<=0)
            {
                isPreformingAction = false;
            }
        }
    }

    #region 攻击方法


    #endregion
}
