using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    AnimatorHandler animHandler;
    InputHandler inputHandler;
    public string lastAttack;
    WeaponSlotManager weaponSlotManager;
    PlayerStats playerStats;

    private void Awake()
    {
        inputHandler = GetComponent<InputHandler>();
        weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
        animHandler = GetComponentInChildren<AnimatorHandler>();
        playerStats = GetComponent<PlayerStats>();
        
    }

    public void HandleWeaponCombo(WeaponItem weapon)
    {

        if (playerStats.currentStamina <= 0)
            return;
        if (inputHandler.comboFlag)
        {
            animHandler.anim.SetBool("canDoCombo", false);

            if (lastAttack == weapon.Light_Attack)
            {
                animHandler.PlayTargetAnimation(weapon.Light_Attack_1, true);
            }
        }
     
    }

    public void HandleLightAttack(WeaponItem weapon)
    {

        if (playerStats.currentStamina <= 0)
            return;
        weaponSlotManager.attackingWeapon = weapon;
        animHandler.PlayTargetAnimation(weapon.Light_Attack, true);
        lastAttack = weapon.Light_Attack;
    }

    public void HandleHeavyAttack(WeaponItem weapon)
    {

        if (playerStats.currentStamina <= 0)
            return;
        weaponSlotManager.attackingWeapon = weapon;
        animHandler.PlayTargetAnimation(weapon.Heavy_Attack, true);
        lastAttack = weapon.Heavy_Attack;
    }
}
