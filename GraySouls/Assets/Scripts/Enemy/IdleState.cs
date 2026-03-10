using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class IdleState : State
{
    public PursueTargetState pursueTargetState;
    public DeadState deadState;

    public LayerMask detectionLayer;
    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
    {
        if (enemyStats.isDead)
        {
            return deadState;
        }

        if (enemyManager.isInteracting)
            return this;

        if (enemyManager.idle)
        {
            return this;
        }

        //寻找潜在目标，若发现目标则切换至追击目标状态
        //如果没找到目标，回到这个状态

        #region 侦察敌人
        Collider[] colliders = Physics.OverlapSphere(transform.position, enemyManager.detectionRadius, detectionLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterStats characterStats = colliders[i].transform.GetComponent<CharacterStats>();

            if (characterStats != null)
            {
                //检查TeamID

                Vector3 targetDirection = characterStats.transform.position - transform.position;
                float viewbleAngel = Vector3.Angle(targetDirection, transform.forward);

                if (viewbleAngel > enemyManager.minimumDetectiongAngle && viewbleAngel < enemyManager.maximumDetectionAngle)
                {
                    enemyManager.currentTarget = characterStats;
                }
            }
        }
        #endregion

        #region 切换状态
        if (enemyManager.currentTarget != null)
        {
            return pursueTargetState;
        }
        else
        {
            return this;
        }
        #endregion

    }
}
