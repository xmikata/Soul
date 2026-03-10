using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    public CharacterManager characterManager;
    public Collider damageCollider;
    public GameObject holder;

    [Header("Poise")]
    public float poiseBreak;
    public float offensivePoiseDefence;

    [Header("Damage")]
    public int currentWeaponDamage = 25;

    private void Awake()
    {
        damageCollider = GetComponent<Collider>();
        damageCollider.gameObject.SetActive(true);
        damageCollider.isTrigger = true;
        damageCollider.enabled = false;
    }

    private void Start()
    {
        if (transform.root != transform)
        {
            holder = transform.root.gameObject;
        }
    }

    public void EnableDamageCollider()
    {
        damageCollider.enabled = true;
    }

    public void DisableDamageCollider()
    {
        damageCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (holder.tag=="Player")
        {
            if (collision.tag == "Enemy")
            {
                EnemyStats enemyStats = collision.GetComponent<EnemyStats>();
                CharacterManager enemyCharacterManager = collision.GetComponent<CharacterManager>();
                BlockingCollider shield = collision.transform.GetComponentInChildren<BlockingCollider>();

                if (enemyCharacterManager!=null)
                {
                    if (enemyCharacterManager.isParrying && characterManager.canBeParried)
                    {
                        characterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Parried", true);
                        return;
                    }
                }

                if (enemyStats!=null)
                {
                    enemyStats.poiseResetTimer = enemyStats.totalPoiseResetTime;
                    enemyStats.totalPoiseDefence = enemyStats.totalPoiseDefence - poiseBreak;

                    if (enemyStats.isBoss)
                    {
                        if (enemyStats.totalPoiseDefence > poiseBreak)
                        {
                            enemyStats.TakeDamageNoAnimation(currentWeaponDamage);
                        }
                        else
                        {
                            enemyStats.TakeDamageNoAnimation(currentWeaponDamage);
                            enemyStats.BreakGuard();
                        }
                    }
                    else
                    {
                        if (enemyStats.totalPoiseDefence > poiseBreak)
                        {
                            enemyStats.TakeDamageNoAnimation(currentWeaponDamage);
                        }
                        else
                        {
                            enemyStats.TakeDamage(currentWeaponDamage);
                        }
                    }
                }
            }
        }
        else if(holder.tag=="Enemy")
        {
            if (collision.tag == "Player")
            {
                PlayerStats playerStats = collision.GetComponent<PlayerStats>();
                CharacterManager playerCharacterManager = collision.GetComponent<CharacterManager>();
                BlockingCollider shield = collision.transform.GetComponentInChildren<BlockingCollider>();

                if (characterManager != null)
                {
                    if (playerCharacterManager.isParrying && characterManager.canBeParried)
                    {
                        characterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Parried", true);
                        return;
                    }
                    else if (shield!=null&& playerCharacterManager.isBlocking)
                    {
                        int physicalDamageAfterBlock = 
                            Mathf.CeilToInt(currentWeaponDamage - (currentWeaponDamage * shield.blockingPhysicalDamageAbsorption) / 100);
                        if (playerStats!=null)
                        {
                            playerStats.TakeDamage(physicalDamageAfterBlock,"Block Guard Ping");
                            return;
                        }
                    }
                }


                if (playerStats != null)
                {
                    playerStats.poiseResetTimer = playerStats.totalPoiseResetTime;
                    playerStats.totalPoiseDefence = playerStats.totalPoiseDefence - poiseBreak;
                    
                    if (playerStats.totalPoiseDefence > poiseBreak)
                    {
                        playerStats.TakeDamageNoAnimation(currentWeaponDamage);
                        
                    }
                    else
                    {
                        playerStats.TakeDamage(currentWeaponDamage);
                    }
                }
            }
        }

    }
}
