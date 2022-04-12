using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    AnimatorHandler animHandler;
    InputHandler inputHandler;
    public string lastAttack;
    

    private void Awake()
    {
        inputHandler = GetComponent<InputHandler>();
        animHandler = GetComponentInChildren<AnimatorHandler>();
    }

    public void HandleWeaponCombo(WeaponItem weapon)
    {
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
        animHandler.PlayTargetAnimation(weapon.Light_Attack, true);
        lastAttack = weapon.Light_Attack;
    }

    public void HandleHeavyAttack(WeaponItem weapon)
    {
        animHandler.PlayTargetAnimation(weapon.Heavy_Attack, true);
        lastAttack = weapon.Heavy_Attack;
    }
}
