using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbushState : State
{
    public bool isSleeping;
    public float detectionRadius = 2;
    public string sleepAnimation;
    public string wakeAnimation;
    LayerMask detectionLayer;

    public PursueTargetState pursueTargetState;
    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
    {
        if (isSleeping&&enemyManager.isPreformingAction==false)
        {
            enemyAnimatorManager.PlayTargetAnimation(sleepAnimation, true);
        }

        #region ¼ì²âÄ¿±ê

        Collider[] colliders = Physics.OverlapSphere(enemyManager.transform.position, detectionRadius, detectionLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterStats characterstats = colliders[i].transform.GetComponent<CharacterStats>();

            if (characterstats!=null)
            {
                Vector3 targetDirection = characterstats.transform.position - enemyManager.transform.position;
                float viewableAngle = Vector3.Angle(targetDirection, enemyManager.transform.forward);

                if (viewableAngle >enemyManager.minimumDetectiongAngle
                    &&viewableAngle <enemyManager.maximumDetectionAngle)
                {
                    enemyManager.currentTarget = characterstats;
                    isSleeping = false;
                    enemyAnimatorManager.PlayTargetAnimation(wakeAnimation, true);
                }
            }
        }


        #endregion

        #region ×´Ì¬ÇÐ»»
        if (enemyManager.currentTarget!=null)
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
