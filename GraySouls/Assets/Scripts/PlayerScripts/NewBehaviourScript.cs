using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class NewBehaviourScript : MonoBehaviour
{
    #region 1
    //Transform cameraObject;
    //InputHandler inputHandler;
    //Vector3 moveDirection;

    //public Transform myTransform;

    //public new Rigidbody rigidbody;
    //public GameObject normalCamera;

    //float movementSpeed = 5;

    //float rotationSpeed = 10;
    //void Start()
    //{
    //    rigidbody = GetComponent<Rigidbody>();
    //    inputHandler = GetComponent<InputHandler>();
    //    cameraObject = Camera.main.transform;
    //    myTransform = transform;
    //}

    //private void Update()
    //{
    //    float delta=Time.deltaTime;

    //    inputHandler.TickInput(delta);

    //    moveDirection = cameraObject.forward*inputHandler.vertical;
    //    moveDirection+=cameraObject.right* inputHandler.vertical;
    //    moveDirection.Normalize();

    //    float speed = movementSpeed;
    //    moveDirection *= speed;

    //    Vector3 projectedVeclocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
    //    rigidbody.velocity = projectedVeclocity;
    //}
    //#region Movement
    //Vector3 normalVector;
    //Vector3 targetPosition;

    //private void HandleRotation(float delat)
    //{
    //    Vector3 targetDir = Vector3.zero;
    //    float moveOverride = inputHandler.moveAmount;

    //    targetDir = cameraObject.forward * inputHandler.vertical;
    //    targetDir = cameraObject.right * inputHandler.horizontal;

    //    targetDir.Normalize();
    //    targetDir.y = 0;

    //    if (targetDir==Vector3.zero)
    //    {
    //        targetDir = myTransform.forward;
    //    }
    //    float rs = rotationSpeed;

    //    Quaternion tr = Quaternion.LookRotation(targetDir);
    //    Quaternion targetRotation = Quaternion.Slerp(myTransform.rotation, tr, rs * delta);
    //    myTransform.rotation = targetRotation;
    //}
    //#endregion
    #endregion

    #region 2
    //public float horizontal;
    //public float vertical;
    //public float moveAmount;
    //public float mouseX;
    //public float mouseY;

    //PlayerControls inputActions;

    //Vector2 movementInput; 
    //Vector2 cameraInput;
    //public void OnEnable()
    //{
    //    if (inputActions == null)
    //    {
    //        inputActions = new PlayerControls(); 
    //        inputActions.Playerovement.Movement.performed += inputActions => movementInput =inputActions.Readvalue<Vector2>(); 
    //        inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
    //    }

    //    inputActions.Enable();
    //}

    //private void OnDisable()
    //{
    //    inputActions.Disable();
    //}

    //public void TickInput(float delta)
    //{
    //    horizontal = movementInput.x;
    //    vertical = movementInput.y;
    //    moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
    //    mouseX = cameraInput.x;
    //    mouseY = cameraInput.y;
    //}
    #endregion
}
