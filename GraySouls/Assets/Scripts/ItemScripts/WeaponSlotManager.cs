using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlotManager : MonoBehaviour
{
    WeaponHolderSlot leftHandSlot;
    public WeaponHolderSlot rightHandSlot;
    WeaponHolderSlot backSlot;

    public DamageCollider leftDamageCollider;
    public DamageCollider rightDamageCollider;

    public WeaponItem attackingWeapon;

    Animator animator;

    QuickSlotsUI quickSlotsUI;

    PlayerStats playerStats;
    InputHandler inputHandler;
    PlayerManager playerManager;
    PlayerInventory playerInventory;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        quickSlotsUI = FindObjectOfType<QuickSlotsUI>();
        playerStats = GetComponentInParent<PlayerStats>();
        inputHandler = GetComponentInParent<InputHandler>();
        playerInventory = GetComponentInParent<PlayerInventory>();
        playerManager = GetComponentInParent<PlayerManager>();

        WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
        foreach (WeaponHolderSlot weaponSlot in weaponHolderSlots)
        {
            if (weaponSlot.isLeftHandSlot)
            {
                leftHandSlot = weaponSlot;
                //LoadLeftWeaponDamageCollider();
            }
            else if(weaponSlot.isRightHandSlot)
            {
                rightHandSlot = weaponSlot;
                //LoadRightWeaponDamageCollider();
            }
            else if (weaponSlot.isBackSlot)
            {
                backSlot = weaponSlot;
            }
        }
    }

    public void LoadBothWeaponOnSlots()
    {
        LoadWeaponOnSlot(playerInventory.rightWeapon, false);
        LoadWeaponOnSlot(playerInventory.leftWeapon, true);
    }

    #region ÎäÆ÷Åö×²Ìå²Ù×÷
    public void LoadLeftWeaponDamageCollider()
    {
        if (leftHandSlot.currentWeaponModel!=null&& leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>()!=null)
        {
            leftDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            leftDamageCollider.currentWeaponDamage = playerInventory.leftWeapon.baseDamage;
            leftDamageCollider.characterManager = GetComponentInParent<CharacterManager>();
            leftDamageCollider.poiseBreak = playerInventory.leftWeapon.poiseBreak;
        }
    }

    public void LoadRightWeaponDamageCollider()
    {
        if (rightHandSlot.currentWeaponModel != null && rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>() != null)
        {
            rightDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            rightDamageCollider.characterManager = GetComponentInParent<CharacterManager>();
            rightDamageCollider.poiseBreak = playerInventory.rightWeapon.poiseBreak;
        }
    }

    public void LoadWeaponsDamageCollider(bool isLeft)
    {

    }

    public void OpenDamageCollider()
    {
        if (playerManager.isUsingRightHand)
        {
            rightDamageCollider.EnableDamageCollider();
        }
        else if(playerManager.isUsingLeftHand)
        {
            leftDamageCollider.EnableDamageCollider();
        }
    }

    //public void OpenLeftDamageCollider()
    //{
    //    leftDamageCollider.EnableDamageCollider();
    //}

    public void CloseDamageCollider()
    {
        
        if (rightDamageCollider!=null)
        {
            rightDamageCollider.DisableDamageCollider();
        }
        if (leftDamageCollider != null)
        {
            leftDamageCollider.DisableDamageCollider();
        }
    }

    //public void CloseLeftDamageCollider()
    //{
    //    leftDamageCollider.DisableDamageCollider();
    //}
    #endregion
    public void LoadWeaponOnSlot(WeaponItem weaponItem,bool isLeft)
    {
        if (isLeft)
        {
            leftHandSlot.currentWeapon = weaponItem;
            leftHandSlot.LoadWeaponModel(weaponItem);

            quickSlotsUI.UpdateWeaponQuickSlotsUI(true, weaponItem);
            #region ÎäÆ÷ÏÐÖÃ¶¯»­
            if (weaponItem != null)
            {
                animator.CrossFade(weaponItem.left_hand_idle, 0.2f);
            }
            else
            {
                animator.CrossFade("Left Arm Empty", 0.2f);
            }
            #endregion
            LoadLeftWeaponDamageCollider();
        }
        else
        {
            if (inputHandler.twoHandFlag)
            {
                backSlot.LoadWeaponModel(leftHandSlot.currentWeapon);
                leftHandSlot.UnloadWeaponAndDestroy();
                animator.CrossFade(weaponItem.th_idle, 0.2f);
            }
            else
            {

                #region ÎäÆ÷ÏÐÖÃ¶¯»­

                animator.CrossFade("Both Arms Empty", 0.2f);

                backSlot.UnloadWeaponAndDestroy();

                if (weaponItem != null)
                {
                    
                    animator.CrossFade(weaponItem.right_hand_idle, 0.2f);
                }
                else
                {
                    animator.CrossFade("Right Arm Empty", 0.2f);
                }
                #endregion
            }
            rightHandSlot.currentWeapon = weaponItem;
            rightHandSlot.LoadWeaponModel(weaponItem);

            quickSlotsUI.UpdateWeaponQuickSlotsUI(false, weaponItem);
            LoadRightWeaponDamageCollider();
        }
    }

    #region ÎäÆ÷ÄÍÁ¦ÏûºÄ²Ù×÷
    public void DrainStaminaLightAttack()
    {
        playerStats.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.lightAttackMultiplier));
    }

    public void DrainStaminaHeavyAttack()
    {
        playerStats.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.heavyAttackMultiplier));
    }
    #endregion

    #region ÎäÆ÷¹¥»÷ÈÍÐÔ
    
    public void GrantWeaponAttackingPoiseBonus()
    {
        playerStats.totalPoiseDefence = playerStats.totalPoiseDefence + attackingWeapon.offensivePoiseBonus;
    }

    public void ResetWeaponPoiseBonus()
    {
        playerStats.totalPoiseDefence = playerStats.totalPoiseDefence - attackingWeapon.offensivePoiseBonus;
    }

    #endregion
}
