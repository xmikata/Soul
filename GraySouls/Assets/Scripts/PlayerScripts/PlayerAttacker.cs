using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    PlayerManager playerManager;
    PlayerStats playerStats;
    PlayerAnimatorManager animatorHandler;
    InputHandler inputHandler;
    WeaponSlotManager weaponSlotManager;
    PlayerInventory playerInventory;
    PlayerEquipmentManager playerEquipmentManager;

    public string lastAttack;

    LayerMask backStabLayer=1<<12;
    LayerMask ripostLayer = 1 << 13;

    private void Awake()
    {
        playerInventory = GetComponent<PlayerInventory>();
        playerManager = GetComponent<PlayerManager>();
        animatorHandler = GetComponentInChildren<PlayerAnimatorManager>();
        playerEquipmentManager = GetComponentInChildren<PlayerEquipmentManager>();
        inputHandler = GetComponent<InputHandler>();
        weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
        playerStats = GetComponent<PlayerStats>();
    }

    public void HandleWeaponCombo(WeaponItem weapon)
    {
        if (playerStats.currentStamina <= 0)
        {
            return;
        }

        if (inputHandler.comboFlag)
        {
            animatorHandler.anim.SetBool("canDoCombo", false);

           
            if (lastAttack == weapon.One_Light_Attack_1)
            {
                animatorHandler.PlayTargetAnimation(weapon.One_Light_Attack_2, true);
                lastAttack = weapon.One_Light_Attack_2;
            }
            else if (lastAttack == weapon.One_Light_Attack_2)
            {
                animatorHandler.PlayTargetAnimation(weapon.One_Light_Attack_1, true);
                lastAttack = weapon.One_Light_Attack_1;
            }
            else if(lastAttack==weapon.Two_Light_Attack_1)
            {
                animatorHandler.PlayTargetAnimation(weapon.Two_Light_Attack_2, true);
                lastAttack = weapon.Two_Light_Attack_2;
            }
            else if (lastAttack == weapon.Two_Light_Attack_2)
            {
                animatorHandler.PlayTargetAnimation(weapon.Two_Light_Attack_3, true);
                lastAttack = weapon.Two_Light_Attack_3;
            }
            else if (lastAttack == weapon.Two_Light_Attack_3)
            {
                animatorHandler.PlayTargetAnimation(weapon.Two_Light_Attack_1, true);
                lastAttack = weapon.Two_Light_Attack_1;
            }
        }   
    }
    public void HandleLightAttack(WeaponItem weapon)
    {
        if (playerStats.currentStamina <= 0)
        {
            return;
        }

        weaponSlotManager.attackingWeapon = weapon;

        if (inputHandler.twoHandFlag)
        {
            animatorHandler.PlayTargetAnimation(weapon.Two_Light_Attack_1, true);
            lastAttack = weapon.Two_Light_Attack_1;
        }
        else
        {
            
            animatorHandler.PlayTargetAnimation(weapon.One_Light_Attack_1, true);
            lastAttack = weapon.One_Light_Attack_1;
        }
       
    }

    public void HandleHeavyAttack(WeaponItem weapon)
    {
        if (playerStats.currentStamina <= 0)
        {
            return;
        }

        weaponSlotManager.attackingWeapon = weapon;

        if (inputHandler.twoHandFlag)
        {
            animatorHandler.PlayTargetAnimation(weapon.Two_Heavy_Attack_1, true);
            lastAttack = weapon.Two_Heavy_Attack_1;
        }
        else
        {
            
            animatorHandler.PlayTargetAnimation(weapon.One_Heavy_Attack_1, true);
            lastAttack = weapon.One_Heavy_Attack_1;
        }
        
    }

    public void AttemptBackStabOrRiposte()
    {
        if (playerStats.currentStamina <= 0)
        {
            return;
        }

        RaycastHit hit;

        if (Physics.Raycast(inputHandler.criticalAttackRayCastStartPoint.position, 
            transform.TransformDirection(Vector3.forward), out hit, 0.5f, backStabLayer))
        {
            CharacterManager enemyCharacterManager=null;
            DamageCollider rightWeapon=null;
            if (hit.transform!=null)
            {
                enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
                rightWeapon = weaponSlotManager.rightDamageCollider;
            }

            if (enemyCharacterManager!=null)
            {
                playerManager.transform.position = enemyCharacterManager.backStabCollider.criticalDamagerStandPosition.position;
                Vector3 rotationDirection = playerManager.transform.root.eulerAngles;
                rotationDirection = hit.transform.position - playerManager.transform.position;
                rotationDirection.y = 0;
                rotationDirection.Normalize();
                Quaternion tr = Quaternion.LookRotation(rotationDirection);
                Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 500 * Time.deltaTime);
                playerManager.transform.rotation = targetRotation;

                int criticalDamage = playerInventory.rightWeapon.criticalDamageMuiltiplier * rightWeapon.currentWeaponDamage;
                enemyCharacterManager.pendingCriticalDamage = criticalDamage;

                playerManager.isStabbing = true;
                animatorHandler.anim.SetBool("isStabbing", true);
                animatorHandler.PlayTargetAnimation("Back Stab", true);
                enemyCharacterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Back Stabbed", true);
            }
        }
        else if (Physics.Raycast(inputHandler.criticalAttackRayCastStartPoint.position,
                transform.TransformDirection(Vector3.forward), out hit, 0.7f, ripostLayer))
        {
            CharacterManager enemyCharacterManager = null;
            DamageCollider rightWeapon = null;
            if (hit.transform != null)
            {
                enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
                rightWeapon = weaponSlotManager.rightDamageCollider;
            }

            if (enemyCharacterManager!=null&&enemyCharacterManager.canBeRiposted)
            {
                playerManager.transform.position = enemyCharacterManager.riposteCollider.criticalDamagerStandPosition.position;

                Vector3 rotationDirection = playerManager.transform.root.eulerAngles;
                rotationDirection = hit.transform.position - playerManager.transform.position;
                rotationDirection.y = 0;
                rotationDirection.Normalize();
                Quaternion tr = Quaternion.LookRotation(rotationDirection);
                Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 500 * Time.deltaTime);
                playerManager.transform.rotation = targetRotation;

                int criticalDamage = playerInventory.rightWeapon.criticalDamageMuiltiplier * rightWeapon.currentWeaponDamage;
                enemyCharacterManager.pendingCriticalDamage = criticalDamage;

                playerManager.isStabbing = true;
                animatorHandler.anim.SetBool("isStabbing", true);
                animatorHandler.PlayTargetAnimation("Riposte", true);
                enemyCharacterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Riposted", true);
                enemyCharacterManager.GetComponentInChildren<AnimatorManager>().anim.SetBool("canBeRiposted", false);
                enemyCharacterManager.GetComponentInChildren<EnemyWeaponSlotManager>().CloseDamageCollider();
            }
        }
    }

    public void HandleLBAction()
    {
        PerformLBBlockingAction();
    }
    public void HandleLTAction()
    {
        if (playerInventory.leftWeapon.isShieldWeapon)
        {
            PerformLTWeaponArt(inputHandler.twoHandFlag);
        }
        else if(playerInventory.leftWeapon.isMeleeWeapon)
        {

        }
    }
    public void PerformLTWeaponArt(bool isTwoHanding)
    {
        if (playerManager.isInteracting)
            return;

        if (isTwoHanding)
        {
            
        }
        else
        {
            animatorHandler.PlayTargetAnimation(playerInventory.leftWeapon.weapon_art, true);
        }
    }

    private void PerformLBBlockingAction()
    {
        if (playerManager.isInteracting)
            return;

        if (playerManager.isBlocking)
            return;

        animatorHandler.PlayTargetAnimation("Block_Start", false,true);
        playerEquipmentManager.OpenBlockingCollider();
        playerManager.isBlocking = true;

    }
}
