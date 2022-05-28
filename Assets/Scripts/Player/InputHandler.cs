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
    public bool paused;

    public bool b_Input;
    public bool a_Input;
    public bool rb_Input;
    public bool rt_Input;
    public bool lockOnInput;
    public bool right_Stick_Right_Input;
    public bool right_Stick_Left_Input;

    public bool d_Pad_Up;
    public bool d_Pad_Down;
    public bool d_Pad_Left;
    public bool d_Pad_Right;


    public bool lockOnFlag;
    public bool isInteracting;
    public float rollInputTimer;
    

    PlayerControls inputActions;
    PlayerAttacker playerAttacker;
    CameraHandler cameraHandler;
    PlayerInventory playerInventory;
    PlayerManager playerManager;

    public GameObject pause;
    public GameObject pauseMenu;
    public GameObject optionsMenu;
    

    Vector2 movementInput;
    Vector2 cameraInput;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
;       playerAttacker = GetComponent<PlayerAttacker>();
        playerInventory = GetComponent<PlayerInventory>();
        cameraHandler = FindObjectOfType<CameraHandler>();
    }

    private void Start()
    {
        pause.SetActive(false);
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
    }

    private void FixedUpdate()
    {
        float delta = Time.fixedDeltaTime;
        if (cameraHandler != null)
        {
            cameraHandler.FollowTarget(delta);
            cameraHandler.HandleCameraRotation(delta, mouseX, mouseY);
        }
     
    }

    public void OnEnable()
    {
        if (inputActions == null)
        {
            inputActions = new PlayerControls();
            inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
            inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
            inputActions.PlayerActions.Run.performed += i => b_Input = true;
            inputActions.PlayerActions.Run.canceled += i => b_Input = false;
            inputActions.PlayerActions.LockOn.performed += i => lockOnInput = true;
            inputActions.PlayerQuickSlots.DPadRight.performed += i => d_Pad_Right = true;
            inputActions.PlayerQuickSlots.DPadLeft.performed += i => d_Pad_Left = true;
            b_Input = inputActions.PlayerActions.Run.phase == UnityEngine.InputSystem.InputActionPhase.Performed;
            inputActions.PlayerActions.RB.performed += i => rb_Input = true;
            inputActions.PlayerActions.RT.performed += i => rt_Input = true;
            inputActions.PlayerMovement.LockOnTargetRight.performed += i => right_Stick_Right_Input = true;
            inputActions.PlayerMovement.LockOnTargetLeft.performed += i => right_Stick_Left_Input = true;
            inputActions.PlayerActions.Pause.started += i => PauseGame();

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
        HandleQuickSlotsInput();
        HandleInteractingButtonInput();
        PausedGame();
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
        
        sprintFlag = b_Input;

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
            lockOnInput = false;
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

        if(lockOnFlag && right_Stick_Left_Input)
        {
            right_Stick_Left_Input = false;
            cameraHandler.HandleLockOn();
            if(cameraHandler.leftLockTarget != null)
            {
                cameraHandler.currentLockOnTarget = cameraHandler.leftLockTarget;
            }
        }

        if (lockOnFlag && right_Stick_Left_Input)
        {
            right_Stick_Right_Input = false;
            cameraHandler.HandleLockOn();
            if (cameraHandler.rightLockTarget != null)
            {
                cameraHandler.currentLockOnTarget = cameraHandler.rightLockTarget;
            }
        }
    }
    private void HandleQuickSlotsInput()
    {
       

        if (d_Pad_Right)
        {
            playerInventory.ChangeRightWeapon();
        }

        else if (d_Pad_Left)
        {
            playerInventory.ChangeLeftWeapon();
        }
    }

    private void HandleInteractingButtonInput()
    {
        inputActions.PlayerActions.Interact.performed += i => a_Input = true;
    }
    public void PausedGame()
    {
       
       
    }
    public void OptionsMenu()
    {
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(true);
        
    }
    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        optionsMenu.SetActive(false);
        pause.SetActive(false);
        pauseMenu.SetActive(false);
        paused = false;
    }

    public void PauseGame()
    {
        Cursor.lockState = CursorLockMode.None;
        pause.SetActive(true);
        pauseMenu.SetActive(true);
    }

    public void PauseMenu()
    {
        pauseMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }
}
