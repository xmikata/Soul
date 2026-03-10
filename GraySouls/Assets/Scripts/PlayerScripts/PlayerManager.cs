using System.Collections;
using System.Collections.Generic;
using UnityEditor.Presets;
using UnityEngine;

public class PlayerManager : CharacterManager
{
    public static PlayerManager singleton;

    public InputHandler inputHandler;
    public CameraHandler cameraHandler;
    Animator anim;
    PlayerLocomotion playerLocomotion;
    PlayerStats playerStats;
    PlayerAnimatorManager playerAnimatorManager;

    InteractableUI interactableUI;
    public GameObject interactableUIGameObject;
    public GameObject itemInteractableGameObject;

    public bool isInteracting;

    public LayerMask interactableLayer;


    [Header("Player Flags")]
    public bool isSprinting;
    public bool isInAir;
    public bool isGrounded;
    public bool canDoCombo;
    public bool isUsingRightHand;
    public bool isUsingLeftHand;
    public bool isStabbing;

    private void Awake()
    {
        cameraHandler = FindObjectOfType<CameraHandler>();
        playerStats = GetComponent<PlayerStats>();
        backStabCollider = GetComponentInChildren<CriticalDamageCollider>();
        playerAnimatorManager = GetComponentInChildren<PlayerAnimatorManager>();
    }
    void Start()
    {
        inputHandler = GetComponent<InputHandler>();
        anim = GetComponentInChildren<Animator>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        singleton = this;
        interactableUI = FindObjectOfType<InteractableUI>();
    }

    void Update()
    {
        float delta = Time.deltaTime;

        isInteracting = anim.GetBool("isInteracting");
        canDoCombo = anim.GetBool("canDoCombo");
        isUsingRightHand = anim.GetBool("isUsingRightHand");
        isInvulnerable = anim.GetBool("isInvulnerable");
        isUsingLeftHand = anim.GetBool("isUsingLeftHand");
        isStabbing = anim.GetBool("isStabbing");
        anim.SetBool("isBlocking", isBlocking);
        anim.SetBool("isDead", playerStats.isDead);
        playerAnimatorManager.canRotate = anim.GetBool("canRotate");

        inputHandler.TickInput(delta);
        playerLocomotion.HandleRollingAndSprinting(delta);
        playerStats.RegenerateStamina();
        //playerLocomotion.HandleRotation(delta);
        //if (anim.GetBool("isInteracting") == false)
        //{
        //    playerLocomotion.HandleMovement(delta);
        //}
        //playerLocomotion.HandleRollingAndSprinting(delta);
        //playerLocomotion.HandleFalling(delta, playerLocomotion.moveDirection);

        CheckForInteractableObject();
    }

    private void FixedUpdate()
    {
        float delta = Time.fixedDeltaTime;
        //if (cameraHandler != null)
        //{
        //    cameraHandler.FollowTarget(delta);
        //    //cameraHandler.HandleCameraRotation(delta, mouseX, mouseY);
        //}
        if (anim.GetBool("isInteracting") == false)
        {
            playerLocomotion.HandleMovement(delta);
        } 
        playerLocomotion.HandleFalling(delta, playerLocomotion.moveDirection);
        playerLocomotion.HandleRotation(delta);
        inputHandler.sprintFlag = false;
    }

    private void LateUpdate()
    {
        ResetInputHandlerBools();

        if (isInAir)
        {
            playerLocomotion.inAirTimer += Time.deltaTime;
        }

        float delta = Time.fixedDeltaTime;
        if (cameraHandler != null)
        {
            cameraHandler.FollowTarget(delta);
            //cameraHandler.HandleCameraRotation(delta, mouseX, mouseY);
        }
    }

    public void CheckForInteractableObject()
    {
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, 0.3f, transform.forward, out hit, 1f, interactableLayer))
        {
            if (hit.collider.tag=="Interactable")
            {
                Interactable interactableObject = hit.collider.GetComponent<Interactable>();

                if (interactableObject!=null)
                {
                    string interactableText = interactableObject.interactbleText;
                    interactableUI.interactableText.text = interactableText;
                    interactableUIGameObject.SetActive(true);

                    if (inputHandler.pressF)
                    {
                        hit.collider.GetComponent<Interactable>().Interact(this);
                    }
                }
            }
        }
        else
        {
            if (interactableUIGameObject!=null)
            {
                interactableUIGameObject.SetActive(false);
            }

            if (itemInteractableGameObject!=null&&inputHandler.pressF)
            {
                itemInteractableGameObject.SetActive(false);
            }
        }
    }

    public void ResetInputHandlerBools() 
    {
        inputHandler.rollFlag = false;
        //inputHandler.sprintFlag = false;
        isSprinting = inputHandler.sprintFlag;
        inputHandler.leftMouseButtonDown = false;
        inputHandler.rightMouseButtonDown = false;
        inputHandler.holdingLeftShift = false;
        inputHandler.scrollWheelUp = false;
        inputHandler.scrollWheelDown = false;
        inputHandler.pressF = false;
        inputHandler.pressX = false;
        inputHandler.pressEsc = false;
        inputHandler.pressScrollWheel = false;
        inputHandler.pressQ = false;
        inputHandler.changeLockOnTargetLeft = false;
        inputHandler.changeLockOnTargetRight = false;
        inputHandler.pressControl = false;
        inputHandler.holdingLeftMouse = false;
        inputHandler.pressR = false;
    }

    public void PassThroughFogWallInteraction(Transform fogWallEntrance)
    {
        playerLocomotion.rigidbody.velocity = Vector3.zero;

        Vector3 rotationDirection = fogWallEntrance.transform.right;
        Quaternion turnRotation = Quaternion.LookRotation(rotationDirection);
        transform.rotation = turnRotation;

        playerAnimatorManager.PlayTargetAnimation("Pass Through Fog", true);

    }
}
