using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    public int healthLevel = 10;
    public int maxHealth;
    public int currentHealth;

    public int staminaLevel = 10;
    public float maxStamina;
    public float currentStamina;
    public float staminaRegenerationTimer = 0;
    public float staminaRegenerationAmount = 5;

    public StaminaBar staminaBar;
    AnimatorHandler animatorHandler;
    public HealthBar healthbar;
    PlayerManager playerManager;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        staminaBar = FindObjectOfType<StaminaBar>();
        healthbar = FindObjectOfType<HealthBar>();
        animatorHandler = GetComponentInChildren<AnimatorHandler>();
    }

    void Start() 
    {
        maxHealth = SetMaxHealthFromHealthLevel();
        currentHealth = maxHealth;
        healthbar.SetMaxHealth(maxHealth);
        healthbar.SetCurrentHealth(currentHealth);

        maxStamina = SetMaxStaminaFromStaminaLevel();
        currentStamina = maxStamina;
        staminaBar.SetMaxStamina(maxStamina);
        staminaBar.SetCurrentStamina(currentStamina);
    }

    private int SetMaxHealthFromHealthLevel()
    {
        maxHealth = healthLevel * 10;
        return maxHealth;
    }

    private float SetMaxStaminaFromStaminaLevel()
    {
        maxStamina = staminaLevel * 10;
        return maxStamina;
    }
    public void TakeDamage(int damage)
    {
        if (isDead)
            return; 

        currentHealth = currentHealth - damage;

        healthbar.SetCurrentHealth(currentHealth);

        animatorHandler.PlayTargetAnimation("Damage", true);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            animatorHandler.PlayTargetAnimation("Death", true);
            isDead = true;
        }
    }

    public void TakeStaminaDamage(int damage)
    {
        currentStamina = currentStamina - damage;
        staminaBar.SetCurrentStamina(currentStamina);
    }

    public void RegenerateStamina()
    {
        if (playerManager.isInteracting)
        {
            staminaRegenerationTimer = 0;
        }
        else
        {
            staminaRegenerationTimer += Time.deltaTime;

            if (currentStamina < maxStamina && staminaRegenerationTimer > 1f)
            {
                currentStamina += staminaRegenerationAmount * Time.deltaTime;
                staminaBar.SetCurrentStamina(Mathf.RoundToInt(currentStamina));
            }
        }
        
    }
}
