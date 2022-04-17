using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    PlayerLocomotion playerLocomotion;

    
    public float horizontal;
    public float vertical;
    public float moveAmount;
    public float mouseX;
    public float mouseY;                                               

    public bool rollFlag;
    public bool comboFlag;
    public bool sprintFlag;
    public bool b_Input;
    public bool rb_Input;
    public bool rt_Input;
    public bool lockOnInput;

    public bool lockOnFlag;
    public bool isInteracting;
    public float rollInputTimer;
    

    PlayerControls inputActions;
    PlayerAttacker playerAttacker;
    CameraHandler cameraHandler;
    PlayerInventory playerInventory;
    PlayerManager playerManager;
    

    Vector2 movementInput;
    Vector2 cameraInput;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
;       playerAttacker = GetComponent<PlayerAttacker>();
        playerInventory = GetComponent<PlayerInventory>();
        cameraHandler = CameraHandler.singleton;

    }

    private void FixedUpdate()
    {
        
    }

    public void OnEnable()
    {
        if (inputActions == null)
        {
            inputActions = new PlayerControls();
            inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
            inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
            inputActions.PlayerActions.RB.performed += i => rb_Input = true;
            inputActions.PlayerActions.RT.performed += i => rt_Input = true;
            inputActions.PlayerActions.Run.performed += i => b_Input = true;
            inputActions.PlayerActions.Run.canceled += i => b_Input = false;
            inputActions.PlayerActions.LockOn.performed += i => lockOnInput = true;

        }

        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    public void TickInput(float delta)
    {
        MoveInput(delta);
        AttackInput(delta);
        HandleRollInput(delta);
        HandleLockOnInput();
    }
    public void MoveInput(float delta)
    {
        horizontal = movementInput.x;
        vertical = movementInput.y;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
        mouseX = cameraInput.x;
        mouseY = cameraInput.y;
    }

    private void HandleRollInput(float delta)
    {
       

        b_Input = inputActions.PlayerActions.Run.phase == UnityEngine.InputSystem.InputActionPhase.Performed;

        if (b_Input)
        {
            rollInputTimer += delta;
            sprintFlag = true;
        }
        else
        {
            if (rollInputTimer > 0 && rollInputTimer < 0.5f)
            {
                sprintFlag = false;
                rollFlag = true;
            }

            rollInputTimer = 0;
        }
    }

    private void AttackInput(float delta)
    { 
        
        if (rb_Input)
        {
            
            if (playerManager.canDoCombo)
            {
                comboFlag = true;
                playerAttacker.HandleWeaponCombo(playerInventory.rightWeapon);
                comboFlag = false;
            }
            else
            {
                playerAttacker.HandleLightAttack(playerInventory.rightWeapon);
            }
            
            
        }

        if (rt_Input)
        {
            playerAttacker.HandleHeavyAttack(playerInventory.rightWeapon);
        }

    }

    private void HandleLockOnInput()
    {
        if(lockOnInput && lockOnFlag == false)
        {
            cameraHandler.ClearLockOnTargets();
            lockOnFlag = false;
            lockOnFlag = true;
            cameraHandler.HandleLockOn();
            if(cameraHandler.nearestLockOn != null)
            {
                cameraHandler.currentLockOnTarget = cameraHandler.nearestLockOn;
                lockOnFlag = true;
            }

        }
        else if(lockOnInput && lockOnFlag)
        {
            lockOnInput = false;
            lockOnFlag = false;
            cameraHandler.ClearLockOnTargets();

        }
    }
}
