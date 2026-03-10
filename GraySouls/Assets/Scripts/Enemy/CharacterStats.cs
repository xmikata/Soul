using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public int healthLevel = 10;
    public int maxHealth;
    public int currentHealth;

    public int staminaLevel = 10;
    public float  maxStamina;
    public float currentStamina;

    public bool isDead;

    [Header("ШЭад")]
    public float totalPoiseDefence;
    public float basicPoiseDefence;
    public float offensivePiseBonus;
    public float totalPoiseResetTime = 15;
    public float poiseResetTimer = 0;

    protected virtual void Update()
    {
        HandlePoiseResetTimer();
    }

    private void Start()
    {
        totalPoiseDefence = basicPoiseDefence;
    }
    public virtual void HandlePoiseResetTimer()
    {
        if (poiseResetTimer>0)
        {
            poiseResetTimer -= Time.deltaTime;
        }
        else
        {
            totalPoiseDefence = basicPoiseDefence;
        }
    }
}
