using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : CharacterManager
{
    InputHandler inputHandler;
    Animator anim;
    CameraHandler cameraHandler;
    PlayerLocomotion playerLocomotion;
    PlayerStats playerStats;

    InteractableUI interactableUI;
    public GameObject interactableUIGameObject;
    public GameObject interactableItemGameObject;
   
    [Header("Flags")]
    public bool isInteracting;
    public bool isSprinting;
    public bool isInAir;
    public bool isGrounded;
    public bool canDoCombo;

    private void Awake()
    {
        cameraHandler = FindObjectOfType<CameraHandler>();
    }
    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        inputHandler = GetComponent<InputHandler>();
        anim = GetComponentInChildren<Animator>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        interactableUI = FindObjectOfType<InteractableUI>();
    }

    // Update is called once per frame
    void Update()
    {
        isInteracting = anim.GetBool("isInteracting");        
        float delta = Time.deltaTime;
        inputHandler.TickInput(delta);       
        canDoCombo = anim.GetBool("canDoCombo");
        playerLocomotion.HandleRollingAndSprinting(delta);
        playerStats.RegenerateStamina();
        CheckForInteractable();
        
        
    }
    private void FixedUpdate()
    {
        float delta = Time.fixedDeltaTime;
        playerLocomotion.HandleMovement(delta);
        playerLocomotion.HandleFalling(delta, playerLocomotion.moveDirection);
        playerLocomotion.HandleRotation(delta);


    }

    private void LateUpdate()
    {
        inputHandler.rollFlag = false;
        inputHandler.rt_Input = false;
        inputHandler.rb_Input = false;
        inputHandler.d_Pad_Up = false;
        inputHandler.d_Pad_Down = false;
        inputHandler.d_Pad_Left = false;
        inputHandler.d_Pad_Right = false;
        inputHandler.a_Input = false;

        if (isInAir)
        {
            playerLocomotion.inAirTimer = playerLocomotion.inAirTimer + Time.deltaTime;
        }


        float delta = Time.deltaTime;
        if (cameraHandler != null)
        {
            cameraHandler.FollowTarget(delta);
            cameraHandler.HandleCameraRotation(delta, inputHandler.mouseX, inputHandler.mouseY);
        }
    }

    public void CheckForInteractable()
    {
        RaycastHit hit;

        if (Physics.SphereCast(transform.position, 0.3f, transform.forward, out hit, 1f))
        {
            if (hit.collider.tag == "Interactable")
            {
                Interactable interactableObject = hit.collider.GetComponent<Interactable>();

                if(interactableObject != null)
                {
                    string interactableText = interactableObject.interactableText;
                    interactableUI.interactableText.text = interactableText;
                    interactableUIGameObject.SetActive(true);

                    if (inputHandler.a_Input)
                    {
                        hit.collider.GetComponent<Interactable>().Interact(this);
                    }
                }
            }
        }
        else
        {
            if(interactableUIGameObject != null)
            {
                interactableUIGameObject.SetActive(false);
            }
            if(interactableItemGameObject != null && inputHandler.a_Input)
            {
                interactableItemGameObject.SetActive(false);
            }
        }
    }
}
