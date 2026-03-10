using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    WeaponSlotManager weaponSlotManager;

    public WeaponItem rightWeapon;
    public WeaponItem leftWeapon;
    public ConsumableItem currentConsumable;

    public WeaponItem unarmedWeapon;

    public WeaponItem[] weaponsInRightHandSlots = new WeaponItem[1];
    public WeaponItem[] weaponsInLeftHandSlots = new WeaponItem[1];


    public int currentRightWeaponIndex;
    public int currentLeftWeaponIndex;

    public List<WeaponItem> weaponsInventory;
    private void Awake()
    {
        weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
        
    }

    private void Start()
    {
        //rightWeapon = weaponInRightHandSlots[currentRightWeaponIndex];
        //leftWeapon = weaponInLeftHandSlots[currentLeftWeaponIndex];
        //weaponSlotManager.LoadWeaponOnSlot(rightWeapon, false);
        //weaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);
        //weaponSlotManager.LoadLeftWeaponDamageCollider();
        //weaponSlotManager.LoadRightWeaponDamageCollider();
        rightWeapon = weaponsInRightHandSlots[0];
        leftWeapon = weaponsInLeftHandSlots[0];
        weaponSlotManager.LoadWeaponOnSlot(rightWeapon, false);
        weaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);
    }

    public void ChangeRightWeapon()
    {
        currentRightWeaponIndex = currentRightWeaponIndex + 1;   
        
        if (currentRightWeaponIndex == 0 && weaponsInRightHandSlots[0]!=null)
        {
            rightWeapon = weaponsInRightHandSlots[currentRightWeaponIndex];
            weaponSlotManager.LoadWeaponOnSlot(weaponsInRightHandSlots[currentRightWeaponIndex],false);
            weaponSlotManager.LoadRightWeaponDamageCollider();
        }
        else if(currentRightWeaponIndex==0&& weaponsInRightHandSlots[0] == null)
        {
            currentRightWeaponIndex = currentRightWeaponIndex + 1;
        }
        else if (currentRightWeaponIndex==1&&weaponsInRightHandSlots[1] != null)
        {
            rightWeapon = weaponsInRightHandSlots[currentRightWeaponIndex];
            weaponSlotManager.LoadWeaponOnSlot(weaponsInRightHandSlots[currentRightWeaponIndex], false);
            weaponSlotManager.LoadRightWeaponDamageCollider();
        }
        else
        {
            currentRightWeaponIndex = currentRightWeaponIndex + 1;
        }

        if (currentRightWeaponIndex > weaponsInRightHandSlots.Length - 1)
        {
            currentRightWeaponIndex = -1;
            rightWeapon = unarmedWeapon;
            weaponSlotManager.LoadWeaponOnSlot(unarmedWeapon, false);
            weaponSlotManager.LoadRightWeaponDamageCollider();
        }

    }

    public void ChangeLeftWeapon()
    {
        currentLeftWeaponIndex = currentLeftWeaponIndex + 1;

        if (currentLeftWeaponIndex == 0 && weaponsInLeftHandSlots[0] != null)
        {
            leftWeapon = weaponsInLeftHandSlots[currentLeftWeaponIndex];
            weaponSlotManager.LoadWeaponOnSlot(weaponsInLeftHandSlots[currentLeftWeaponIndex], true);
            weaponSlotManager.LoadLeftWeaponDamageCollider();
        }
        else if (currentLeftWeaponIndex == 0 && weaponsInLeftHandSlots[0] == null)
        {
            currentLeftWeaponIndex = currentLeftWeaponIndex + 1;
        }
        else if (currentLeftWeaponIndex == 1 && weaponsInLeftHandSlots[1] != null)
        {
            leftWeapon = weaponsInLeftHandSlots[currentLeftWeaponIndex];
            weaponSlotManager.LoadWeaponOnSlot(weaponsInLeftHandSlots[currentLeftWeaponIndex], true);
            weaponSlotManager.LoadLeftWeaponDamageCollider();
        }
        else
        {
            currentLeftWeaponIndex = currentLeftWeaponIndex + 1;
        }

        if (currentLeftWeaponIndex > weaponsInLeftHandSlots.Length - 1)
        {
            currentLeftWeaponIndex = -1;
            leftWeapon = unarmedWeapon;
            weaponSlotManager.LoadWeaponOnSlot(unarmedWeapon, true);
            weaponSlotManager.LoadLeftWeaponDamageCollider();
        }
    }
}
