using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerEffectsManager : MonoBehaviour
{
    PlayerStats playerStats;
    WeaponSlotManager weaponSlotManager;
    public GameObject currentParticalFX;
    public GameObject instantiatedFXModel;
    public int amountToBeHealed;

    private void Awake()
    {
        playerStats = GetComponentInParent<PlayerStats>();
        weaponSlotManager = GetComponent<WeaponSlotManager>();
    }
    public void HealPlayerFromEffect()
    {

        playerStats.HealPlayer(amountToBeHealed);
        GameObject healParticales = Instantiate(currentParticalFX, playerStats.transform);
        Destroy(instantiatedFXModel.gameObject,0.5f);
        Invoke("Delay", 1f);
    }

    public void Delay()
    {
        weaponSlotManager.LoadBothWeaponOnSlots();
    }
}
