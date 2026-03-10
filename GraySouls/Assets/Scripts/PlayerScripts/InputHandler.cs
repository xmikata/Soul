using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class InputHandler : MonoBehaviour
{
    CameraHandler cameraHandler;
    
    float mouseX;
    float mouseY;
    public float mouseXDistance;

    public float horizontal;
    public float vertical;
    public float moveAmount;
    public float changeLockOnTargetMinDistance;

    public bool holdingSpace;
    public bool leftMouseButtonDown;
    public bool rightMouseButtonDown;
    public bool holdingLeftShift;
    public bool scrollWheelUp;
    public bool scrollWheelDown;
    public bool pressF;
    public bool pressE;
    public float scrollWheel;
    public bool pressX;
    public bool pressEsc;
    public bool pressQ;
    public bool pressScrollWheel;
    public bool changeLockOnTargetLeft;
    public bool changeLockOnTargetRight;
    public bool pressControl;
    public bool holdingLeftMouse;
    public bool pressR;

    public bool twoHandFlag;
    public bool rollFlag;
    public bool sprintFlag;
    public bool comboFlag;
    public bool lockOnFlag;
    public bool inventoruFlag;
    public bool critical_Attack;

    public Transform criticalAttackRayCastStartPoint;
    public BlockingCollider blockingCollider;

    public float rollInputTimer;

    PlayerAttacker playerAttacker;
    PlayerInventory playerInventory;
    PlayerManager playerManager;
    PlayerEffectsManager playerEffectsManager;
    UIManager uiManager;
    WeaponSlotManager weaponSlotManager;
    PlayerAnimatorManager playerAnimatorManager;
    PlayerStats playerStats;

    private void Awake()
    {
        playerAttacker = GetComponent<PlayerAttacker>();
        playerInventory = GetComponent<PlayerInventory>();
        playerManager = GetComponent<PlayerManager>();
        uiManager = FindObjectOfType<UIManager>();
        weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
        playerAnimatorManager = GetComponentInChildren<PlayerAnimatorManager>();
        playerStats = GetComponent<PlayerStats>();
        blockingCollider = GetComponentInChildren<BlockingCollider>();
        playerEffectsManager = GetComponentInChildren<PlayerEffectsManager>();
    }
    private void Start()
    {
        cameraHandler = CameraHandler.singleton;
    }

    private void Update()
    {
        MouseInput();
        float delta = Time.deltaTime;
        if (cameraHandler != null)
        {
            //cameraHandler.FollowTarget(delta);
            cameraHandler.HandleCameraRotation(delta, mouseX, mouseY);
        }
    }


    private void MouseInput()
    {
        mouseX = Input.GetAxis("Mouse X"); // 鼠标横向移动（左右）
        mouseY = Input.GetAxis("Mouse Y"); // 鼠标纵向移动（上下）
        scrollWheel = Input.GetAxis("Mouse ScrollWheel");
        if (scrollWheel>0)
        {
            scrollWheelUp = true;
        }
        else if(scrollWheel < 0)
        {
            scrollWheelDown = true;
        }

        if (Input.GetKeyDown(KeyCode.Mouse2))
        {
            pressScrollWheel = true;
        }

        if (Mathf.Abs(mouseX) >0.05&&lockOnFlag)
        {
            mouseXDistance += mouseX;
        }
        else
        {
            mouseXDistance = 0;
        }

        if (mouseXDistance>changeLockOnTargetMinDistance&& lockOnFlag)
        {
            changeLockOnTargetRight = true ;
            mouseXDistance = 0;
        }
        else if (mouseXDistance < -changeLockOnTargetMinDistance && lockOnFlag)
        {
            changeLockOnTargetLeft = true;
            mouseXDistance = 0;
        }
    }

    private void MoveInput()
    {
        if (Input.GetKey(KeyCode.W))
        {
            vertical = 1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            vertical = -1;
        }
        else
        {
            vertical = 0;
        }
        if (Input.GetKey(KeyCode.A))
        {
            horizontal = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            horizontal = 1;
        }
        else
        {
            horizontal = 0;
        }
    }
    public void TickInput(float delta)
    {
        KeyBoardInput();
        MoveInput();
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
        HandleRollInput(delta);       
        CombatInput(delta);
        HandleQuickSlots();
        HandleInventoryInput();
        HandleLockOnInput();
        HandleTwoHandInput();
        HandleUseComsumableInput();
    }

    private void HandleRollInput(float delta)
    {
        if (Input.GetKey(KeyCode.Space))
        {
            holdingSpace = true;
        }
        else
        {
            holdingSpace = false;
        }
        if (holdingSpace)
        {
            if (moveAmount>0)
            {
                rollInputTimer += delta;

                if (playerStats.currentStamina<=0)
                {
                    holdingSpace = false;
                    sprintFlag = false;
                    rollInputTimer = 0;
                }
                
            }
            if (rollInputTimer>=0.4f)
            {
                sprintFlag = true;
            }
        }
        else 
        {
            if (rollInputTimer>=0&&rollInputTimer<0.4f&& Input.GetKeyUp(KeyCode.Space))
            {
                rollFlag = true;
                sprintFlag = false;
            }

            rollInputTimer = 0;
        }
    }

    private void CombatInput(float delta)
    {
        #region Input
        if (Input.GetMouseButtonDown(0))
        {
            leftMouseButtonDown = true;
        }
        if (Input.GetMouseButtonDown(1))
        {
            rightMouseButtonDown = true;
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            holdingLeftShift = true;
        }

        if (Input.GetMouseButton(1))
        {
            holdingLeftMouse = true;
        }
        #endregion

        #region 战技
        if (pressControl)
        {
            if (twoHandFlag)
            {

            }
            else
            {
                playerAttacker.HandleLTAction();
            }
        }
        #endregion

            HandleCriticalAttackInput();
        if (!playerManager.isStabbing)
        {
            if (leftMouseButtonDown && playerInventory.rightWeapon != playerInventory.unarmedWeapon)
            {
                if (playerManager.canDoCombo)
                {
                    comboFlag = true;
                    playerAttacker.HandleWeaponCombo(playerInventory.rightWeapon);
                    comboFlag = false;
                }
                else if (playerManager.isInteracting == false)
                {
                    playerAttacker.HandleLightAttack(playerInventory.rightWeapon);
                    playerAnimatorManager.anim.SetBool("isUsingRightHand", true);
                }
            }

            if (leftMouseButtonDown && holdingLeftShift && playerInventory.rightWeapon != playerInventory.unarmedWeapon)
            {
                playerAttacker.HandleHeavyAttack(playerInventory.rightWeapon);
            }
        }
        critical_Attack = false;

        #region 格挡
        if (holdingLeftMouse)
        {
            playerAttacker.HandleLBAction();
        }
        else
        {
            playerManager.isBlocking = false;

            if (blockingCollider.blockingCollider.enabled)
            {
                blockingCollider.DisableBlockingCollider();
            }
        }
        #endregion
    }

    private void HandleQuickSlots()
    {
        if (holdingLeftShift&&scrollWheelUp)
        {
            playerInventory.ChangeRightWeapon();
        }
        else if(holdingLeftShift && scrollWheelDown)
        {
            playerInventory.ChangeLeftWeapon();
        }

    }

    private void KeyBoardInput()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            pressF = true;
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            pressX = true;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pressEsc = true;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            pressQ = true;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            pressE = true;
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            pressControl = true;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            pressR = true;
        }
    }

    private void HandleInventoryInput()
    {
        if (pressEsc)
        {
            inventoruFlag = !inventoruFlag;
            if (inventoruFlag)
            {
                uiManager.OpenSelectWindow();
                uiManager.UpdateUI();
                uiManager.hudWindow.SetActive(false);
            }
            else
            {
                uiManager.CloseSelectWindow();
                uiManager.CloseAllInventoryWindows();
                uiManager.hudWindow.SetActive(true);
            }
        }

        
    }

    private void HandleLockOnInput()
    {
        if ((pressQ||pressScrollWheel)&&lockOnFlag==false)
        {
            cameraHandler.ClearLockOnTargets();
            cameraHandler.HandleLockOn();
            if (cameraHandler.nearestLockOnTarget!=null)
            {
                cameraHandler.currentLockOnTarget = cameraHandler.nearestLockOnTarget;
                lockOnFlag = true;
            }
        }
        else if ((pressQ || pressScrollWheel) && lockOnFlag == true)
        {
            lockOnFlag = false;
            cameraHandler.ClearLockOnTargets();
        }

        if (changeLockOnTargetLeft)
        {
            Debug.Log("左");
            cameraHandler.HandleLockOn();
            if (cameraHandler.leftLockTarget!=null)
            {
                cameraHandler.currentLockOnTarget = cameraHandler.leftLockTarget;
            }
        }
        if (changeLockOnTargetRight)
        {
            Debug.Log("右");
            cameraHandler.HandleLockOn();
            if (cameraHandler.rightLockTarget != null)
            {
                cameraHandler.currentLockOnTarget = cameraHandler.rightLockTarget;
            }
        }

        cameraHandler.SetCameraHeight();
    }

    private void HandleTwoHandInput()
    {
        if (pressE)
        {
            pressE = false;
            twoHandFlag = !twoHandFlag;

            if (twoHandFlag)
            {
                weaponSlotManager.LoadWeaponOnSlot(playerInventory.rightWeapon, false);
            }
            else
            {
                weaponSlotManager.LoadWeaponOnSlot(playerInventory.rightWeapon, false);
                weaponSlotManager.LoadWeaponOnSlot(playerInventory.leftWeapon, true);
            }
        }
    }

    private void HandleCriticalAttackInput()
    {
        if (leftMouseButtonDown)
        {
            critical_Attack = true;
        }
        if (critical_Attack&&!playerManager.isStabbing)
        {
            playerAttacker.AttemptBackStabOrRiposte();
        }
    }

    private void HandleUseComsumableInput()
    {
        if (pressR)
        {
            playerInventory.currentConsumable.AttemptToConsumeItem(playerAnimatorManager,weaponSlotManager,playerEffectsManager);
        }
    }
}
