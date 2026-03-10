using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Items/Consumables/Flask")]
public class FlaskItem : ConsumableItem
{
    [Header("圣杯瓶类型")]
    public bool estusFlask;
    public bool ashenFlask;

    [Header("回复量")]
    public int healthRecoverAmount;

    [Header("回复特效")]
    public GameObject recoveryFX;

    public override void AttemptToConsumeItem(PlayerAnimatorManager playerAnimatorManager, WeaponSlotManager weaponSlotManager, PlayerEffectsManager playerEffectsManager)
    {
        base.AttemptToConsumeItem(playerAnimatorManager, weaponSlotManager, playerEffectsManager);
        GameObject flask = Instantiate(itemModle, weaponSlotManager.rightHandSlot.transform);
        playerEffectsManager.currentParticalFX = recoveryFX;
        playerEffectsManager.amountToBeHealed = healthRecoverAmount;
        playerEffectsManager.instantiatedFXModel = flask;
        weaponSlotManager.rightHandSlot.UnloadWeapon();
    }
}
