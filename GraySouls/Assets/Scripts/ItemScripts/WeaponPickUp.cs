using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponPickUp : Interactable
{
    public WeaponItem weapon;

    public override void Interact(PlayerManager playerManager)
    {
        base.Interact(playerManager);

        PickUpItem(playerManager);
    }

    private void PickUpItem(PlayerManager playerManager)
    {
        PlayerInventory playerInventory;
        PlayerLocomotion playerLocomotion;
        PlayerAnimatorManager animatorHandler;
        playerInventory = playerManager.GetComponent<PlayerInventory>();
        playerLocomotion = playerManager.GetComponent < PlayerLocomotion>();
        animatorHandler = playerManager.GetComponentInChildren<PlayerAnimatorManager>();

        playerLocomotion.rigidbody.velocity = Vector3.zero;
        animatorHandler.PlayTargetAnimation("Pick Up Item", true);
        playerInventory.weaponsInventory.Add(weapon);
        playerManager.itemInteractableGameObject.GetComponentInChildren<Text>().text = weapon.itemName;
        playerManager.itemInteractableGameObject.transform.GetChild(1).GetComponent<Image>().preserveAspect = true;
        playerManager.itemInteractableGameObject.transform.GetChild(1).GetComponent<Image>().sprite = weapon.itemIcon;
        playerManager.itemInteractableGameObject.GetComponentInChildren<Image>().preserveAspect = true;
        playerManager.itemInteractableGameObject.SetActive(true);
        Destroy(gameObject);
    }
}
