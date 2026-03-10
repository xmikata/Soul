using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorManager : AnimatorManager
{
    EnemyManager enemyManager;
    EnemyBossManager enemyBossManager;
    EnemyStats enemyStats;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyManager = GetComponentInParent<EnemyManager>();
        enemyBossManager = GetComponentInParent<EnemyBossManager>();
        enemyStats = GetComponentInParent<EnemyStats>();
    }

    public override void TakeCriticalDamageAnimationEvent()
    {
        enemyStats.TakeDamageNoAnimation(enemyManager.pendingCriticalDamage);
        enemyManager.pendingCriticalDamage = 0;
    }

    public void CanRotate()
    {
        anim.SetBool("canRotate", true);
    }

    public void StopRotation()
    {
        anim.SetBool("canRotate", false);
    }

    public void EnableCombo()
    {
        anim.SetBool("canDoCombo", true);
    }

    public void DisableCombo()
    {
        anim.SetBool("canDoCombo", false);
    }

    public void EnableIsInvulnerable()
    {
        anim.SetBool("isInvulnerable", true);
    }

    public void DisableIsInvulnerable()
    {
        anim.SetBool("isInvulnerable", false);
    }
    public void EnableIsParrying()
    {
        enemyManager.isParrying = true;
    }
    public void EnableCanBeRiposted()
    {
        anim.SetBool("canBeRiposted",true);
    }

    public void DisableCanBeRiposted()
    {
        anim.SetBool("canBeRiposted", false);
    }
    public void DisableIsParrying()
    {
        enemyManager.isParrying = false;
    }

    public void CanParry()
    {
        enemyManager.canBeParried = true;
    }

    public void CantParry()
    {
        enemyManager.canBeParried = false;
    }

    public void InstantiateBossParticleFX()
    {
        BossFXTransform bossFXTransform = GetComponentInChildren<BossFXTransform>();
        GameObject phaseFX = Instantiate(enemyBossManager.particalFx, bossFXTransform.transform);
    }

    private void OnAnimatorMove()
    {
        float delta = Time.deltaTime;
        enemyManager.enemyRigidBody.drag = 0;
        Vector3 deltaPosition = anim.deltaPosition;
        deltaPosition.y = 0;
        Vector3 velocity = deltaPosition / delta;
        enemyManager.enemyRigidBody.velocity = velocity;

        if (enemyManager.isRotatingWithRootMotion)
        {
            enemyManager.transform.rotation *= anim.deltaRotation;
        }
    }
}
