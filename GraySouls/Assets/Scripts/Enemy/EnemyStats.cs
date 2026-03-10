using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    EnemyBossManager enemyBossManager;
    EnemyAnimatorManager enemyAnimatorManager;
    public UIEnemyHealthBar enemyHealthBar;
    //public int healthLevel = 10;
    //public int maxHealth;
    //public int currentHealth;
    EnemyManager enemyManager;

    public bool isBoss;

    Animator animator;
    private void Awake()
    {
        enemyManager = GetComponent<EnemyManager>();
        enemyBossManager = GetComponent<EnemyBossManager>();
        animator = GetComponentInChildren<Animator>();
        maxHealth = SetMaxHealthFromHealthLevel();
        currentHealth = maxHealth;
    }
    private void Start()
    {
        if (!isBoss)
        {
            enemyHealthBar.SetMaxHealth(maxHealth);
        }
    }

    public override void HandlePoiseResetTimer()
    {
        base.HandlePoiseResetTimer();
    }
    private int SetMaxHealthFromHealthLevel()
    {
        maxHealth = healthLevel * 10;
        return maxHealth;
    }

    public void TakeDamageNoAnimation(int damage)
    {
        currentHealth = currentHealth - damage;

        if (!isBoss)
        {
            enemyHealthBar.SetHealth(currentHealth);
        }
        else if (isBoss && enemyBossManager != null)
        {
            enemyBossManager.UpdateBossHealthBar(currentHealth,maxHealth);
        }

        if (currentHealth > 0)
        {
            
        }
        else
        {
            currentHealth = 0;
            isDead = true;
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead)
        {
            return;
        }

        currentHealth = currentHealth - damage;

        if (!isBoss)
        {
            enemyHealthBar.SetHealth(currentHealth);
        }
        else if(isBoss&&enemyBossManager!=null)
        {
            enemyBossManager.UpdateBossHealthBar(currentHealth,maxHealth);
        }

        if (currentHealth > 0)
        {
            animator.CrossFade("Damage_01", 0.1f);
        }
        else
        {
            currentHealth = 0;
            animator.CrossFade("Dead_01", 0.1f);
            isDead = true;
        }
    }

    public void BreakGuard()
    {
        enemyAnimatorManager.PlayTargetAnimation("Break Guard", true);

    }
}
