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
        AnimatorHandler animatorHandler;

        playerInventory = playerManager.GetComponent<PlayerInventory>();
        playerLocomotion = playerManager.GetComponent<PlayerLocomotion>();
        animatorHandler = playerManager.GetComponentInChildren<AnimatorHandler>();

        playerLocomotion.rigidbody.velocity = Vector3.zero;
        animatorHandler.PlayTargetAnimation("Pickup", true);
        playerInventory.weaponsInventory.Add(weapon);
        playerManager.interactableItemGameObject.GetComponentInChildren<Text>().text = weapon.itemName;
        playerManager.interactableItemGameObject.GetComponentInChildren<RawImage>().texture = weapon.itemIcon.texture;
        playerManager.interactableItemGameObject.SetActive(true);
        Destroy(gameObject);
    }
}
