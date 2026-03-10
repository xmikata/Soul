using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : State
{
    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
    {
        return this;
    }
}
