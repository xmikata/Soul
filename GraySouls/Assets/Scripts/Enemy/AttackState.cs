using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackState : State
{
    public CombatStanceState combatStanceState;
    public PursueTargetState pursueTarget;
    public RotateTowardsTargetState rotateTowardsTargetState;
    public DeadState deadState;


    public EnemyAttackAction currentAttack;

    bool willDoComboNextAttack=false;
    public bool hasPerformedAttack = false;
    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
    {
        // 根据攻击评分选择一种攻击方式
        // 如果因角度或距离不佳导致选中的攻击无法使用，则重新选择攻击
        // 如果攻击可行，停止移动并攻击目标
        // 将恢复计时器设为该攻击的恢复时间
        // 回到战斗姿态状态
        if (enemyStats.isDead)
        {
            return deadState;
        }

        float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
        
        RotateTowardsTargetWhilstAttacking(enemyManager);

        if (distanceFromTarget>enemyManager.maximumAggroRadius)
        {
            return pursueTarget;
        }

        if (willDoComboNextAttack&&enemyManager.canDoCombo)
        {
            AttackTargetWithCombo(enemyAnimatorManager,enemyManager);

        }

        if (!hasPerformedAttack)
        {
            enemyAnimatorManager.anim.SetFloat("Vertical", 0);
            enemyAnimatorManager.anim.SetFloat("Horizontal", 0);
            AttackTarget(enemyAnimatorManager, enemyManager);
            RollForComboChance(enemyManager);
        }

        if (willDoComboNextAttack&&hasPerformedAttack)
        {
            return this;
        }

        return rotateTowardsTargetState;
        #region 删掉的
        //if (enemyManager.isInteracting&&enemyManager.canDoCombo==false)
        //{
        //    return this;
        //}
        //else if (enemyManager.isInteracting&&enemyManager.canDoCombo)
        //{
        //    if (willDoComboNextAttack)
        //    {
        //        willDoComboNextAttack = false;
        //        enemyAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
        //    }
        //}

        //    Vector3 targetDirection = enemyManager.currentTarget.transform.position - transform.position;
        //float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
        //float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

        //HandleRotateTowardsTarget(enemyManager);

        //if (enemyManager.isPreformingAction)
        //    return combatStanceState;

        //if (currentAttack != null)
        //{
        //    if (distanceFromTarget < currentAttack.minimumDistanceNeededToAttack)
        //    {
        //        return this;
        //    }
        //    else if (distanceFromTarget < currentAttack.maximumDistanceNeededToAttack)
        //    {
        //        if (viewableAngle <= currentAttack.maximumAttackAngle &&
        //            viewableAngle >= currentAttack.minimumAttackAngle)
        //        {
        //            if (enemyManager.currentRecoveryTime <= 0 && enemyManager.isPreformingAction == false)
        //            {
        //                enemyAnimatorManager.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
        //                enemyAnimatorManager.anim.SetFloat("Horizontal", 0, 0.1f, Time.deltaTime);
        //                enemyAnimatorManager.anim.SetBool("isUsingRightHand", true);
        //                enemyAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
        //                enemyManager.isPreformingAction = true;
        //                RollForComboChance(enemyManager);

        //                if (currentAttack.canCombo&&willDoComboNextAttack)
        //                {
        //                    Debug.Log("???");
        //                    currentAttack = currentAttack.comboAction;
        //                    return this;
        //                }
        //                else
        //                {
        //                    enemyManager.currentRecoveryTime = currentAttack.recoveryTime;
        //                    currentAttack = null;
        //                    return combatStanceState;
        //                }
        //            }
        //        }
        //    }
        //}
        //else
        //{
        //    GetNewAttack(enemyManager);
        //}

        //return combatStanceState;
        #endregion
    }

    private void AttackTarget(EnemyAnimatorManager enemyAnimatorManager,EnemyManager enemyManager)
    {
        enemyAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
        enemyManager.currentRecoveryTime = currentAttack.recoveryTime;
        hasPerformedAttack = true;
    }
    private void AttackTargetWithCombo(EnemyAnimatorManager enemyAnimatorManager,EnemyManager enemyManager)
    {
        willDoComboNextAttack = false;
        enemyAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
        enemyManager.currentRecoveryTime = currentAttack.recoveryTime;
        currentAttack = null;
    }
    private void RotateTowardsTargetWhilstAttacking(EnemyManager enemyManager)
    {
        if (enemyManager.canRotate&&enemyManager.isInteracting)
        {
            Vector3 direction = enemyManager.currentTarget.transform.position - transform.position;
            direction.y = 0;
            direction.Normalize();

            if (direction == Vector3.zero)
            {
                direction = transform.forward;
            }

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            enemyManager.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, enemyManager.rotationSpeed / Time.deltaTime);
        }
        //else
        //{
        //    Vector3 relativeDirection = transform.InverseTransformDirection(enemyManager.navMeshAgent.desiredVelocity);
        //    Vector3 targetVelocity = enemyManager.enemyRigidBody.velocity;

        //    enemyManager.navMeshAgent.enabled = true;
        //    enemyManager.navMeshAgent.SetDestination(enemyManager.currentTarget.transform.position);
        //    enemyManager.enemyRigidBody.velocity = targetVelocity;
        //    enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, enemyManager.navMeshAgent.transform.rotation, enemyManager.rotationSpeed / Time.deltaTime);
        //}

        //enemyManager.navMeshAgent.transform.localPosition = Vector3.zero;
        //enemyManager.navMeshAgent.transform.localRotation = Quaternion.identity;
    }
    private void RollForComboChance(EnemyManager enemyManager)
    {
        float comboChance = Random.Range(0, 100);

        if (enemyManager.allowAIToPerformCombos&&comboChance<=enemyManager.comboLikelyHood)
        {
            if (currentAttack.comboAction!=null)
            {
                willDoComboNextAttack = true;
                currentAttack = currentAttack.comboAction;
            }
            else
            {
                willDoComboNextAttack = false;
                currentAttack = null;
            }
        }
    }
}
