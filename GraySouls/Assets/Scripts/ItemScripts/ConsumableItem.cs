using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableItem : Item
{
    [Header("道具数量")]
    public int maxItemAmout;
    public int currentIteamAmount;

    [Header("道具模型")]
    public GameObject itemModle;

    [Header("动画")]
    public string consumeAnimation;
    public bool isInteracting;

    public virtual void AttemptToConsumeItem(PlayerAnimatorManager playerAnimatorManager,WeaponSlotManager weaponSlotManager,PlayerEffectsManager playerEffectsManager)
    {
        if (currentIteamAmount>0)
        {
            playerAnimatorManager.PlayTargetAnimation(consumeAnimation, isInteracting,true);
        }
        else
        {
            playerAnimatorManager.PlayTargetAnimation("Shrug", true);
        }
    }
}
