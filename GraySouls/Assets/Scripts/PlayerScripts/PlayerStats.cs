using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerStats : CharacterStats
{
    //public int healthLevel = 10;
    //public int maxHealth;
    //public int currentHealth;

    //public int staminaLevel = 10;
    //public int maxStamina;
    //public int currentStamina;

    public HealthBar healthBar;
    public StaminaBar staminaBar;
    PlayerManager playerManager;

    public float staminaRegenerationAmount=30;
    public float staminaRegenTime=0;

    PlayerAnimatorManager animatorHandler;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
    }
    private void Start()
    {
        staminaBar = FindObjectOfType<StaminaBar>();

        maxHealth = SetMaxHealthFromHealthLevel();
        maxStamina = SetMaxStaminaFromHealthLevel();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        staminaBar.SetMaxStamina(maxStamina);
        animatorHandler = GetComponentInChildren<PlayerAnimatorManager>();

        
        currentStamina = maxStamina;

    }

    public override void HandlePoiseResetTimer()
    {
        if (poiseResetTimer>0)
        {
            poiseResetTimer = poiseResetTimer - Time.deltaTime;
        }
        else if (poiseResetTimer<=0&&!playerManager.isInteracting)
        {
            totalPoiseDefence = basicPoiseDefence;
        }
    }
    private int SetMaxHealthFromHealthLevel()
    {
        maxHealth = healthLevel * 10;
        return maxHealth;
    }

    private float SetMaxStaminaFromHealthLevel()
    {
        maxHealth = healthLevel * 10;
        return maxHealth;
    }
    public void TakeDamage(int damage)
    {
        if (playerManager.isInvulnerable)
        {
            return;
        }

        if (isDead)
            return;

        currentHealth = currentHealth - damage;

        healthBar.SetCurrentHealth(currentHealth);

        if (currentHealth > 0)
        {
            animatorHandler.PlayTargetAnimation("Damage_01", true);
        }
        else
        {
            currentHealth = 0;
            animatorHandler.PlayTargetAnimation("Dead_01", true);
            isDead = true;
        }
    }
    public void TakeDamage(int damage,string damageAnimation="Damage_01")
    {
        if (playerManager.isInvulnerable)
        {
            return;
        }

        if (isDead)
            return;

        currentHealth = currentHealth - damage;

        healthBar.SetCurrentHealth(currentHealth);

        if (currentHealth > 0)
        {
            animatorHandler.PlayTargetAnimation(damageAnimation, true);
        }
        else
        {
            currentHealth = 0;
            animatorHandler.PlayTargetAnimation("Dead_01", true);
            isDead = true;
        }
    }
    public void TakeDamageNoAnimation(int damage)
    {
        currentHealth = currentHealth - damage;

        if (currentHealth > 0)
        {

        }
        else
        {
            currentHealth = 0;
            isDead = true;
        }
    }
    public void TakeStaminaDamage(int damage)
    {
        currentStamina = currentStamina - damage;
        staminaBar.SetCurrentStamina(currentStamina);

    }
    public void RegenerateStamina()
    {
        if (playerManager.isInteracting)
        {
            staminaRegenTime = 0;
        }
        else
        {

            staminaRegenTime += Time.deltaTime;
            if (currentStamina <= maxStamina&&staminaRegenTime>0.4f)
            {
                currentStamina += staminaRegenerationAmount * Time.deltaTime;
                staminaBar.SetCurrentStamina(Mathf.RoundToInt(currentStamina));
            }
        } 
    }
    public void HealPlayer(int healAmount)
    {
        currentHealth = currentHealth + healAmount;

        if (currentHealth>maxHealth)
        {
            currentHealth = maxHealth;
        }

        healthBar.SetCurrentHealth(currentHealth);
    }
}
