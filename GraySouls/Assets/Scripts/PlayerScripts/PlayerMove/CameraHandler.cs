using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class CameraHandler : MonoBehaviour
{
    public Transform targetTransform;
    public Transform cameraTransform;
    public Transform cameraPivotTransform;
    private Transform myTransform;
    private Vector3 cameraTransformPosition;
    public LayerMask ignoredLayers;
    public LayerMask enviromentLayer;
    public Vector3 cameraFollowVelocity = Vector3.zero;

    public static CameraHandler singleton;

    public float lookSpeed;
    public float followSpeed;
    public float pivotSpeed;

    private float targetPosition;//检测碰撞用
    private float defaultPosition;
    private float lookAngle;
    private float pivotAngle;
    public float maxnumPivot = 35;
    public float minnumPivot = -35;

    public float cameraSphereRadius;
    public float cameraCollisionOffset;
    public float minimumCollisiontOffset;
    public float lockedPivotPosition = 2.25f;
    public float unlockedPivotPosition = 1.65f;

    public CharacterManager currentLockOnTarget;

    public List<CharacterManager> availableTargets = new List<CharacterManager>();
    public CharacterManager nearestLockOnTarget;
    public CharacterManager leftLockTarget;
    public CharacterManager rightLockTarget;
    public float maximunLockOnDistance;

    public InputHandler inputHandler;
    public PlayerManager playerManager;
    private void Awake()
    {
        singleton = this;
        cameraTransform = Camera.main.transform;
        cameraPivotTransform = cameraTransform.parent;
        myTransform = this.transform;
        defaultPosition = cameraTransform.localPosition.z;
        //ignoredLayers = ~(1 << 8 | 1 << 9 | 1 << 10);
        playerManager = FindObjectOfType<PlayerManager>();
        targetTransform = playerManager.transform.GetChild(1);
        inputHandler = FindObjectOfType<InputHandler>();
        
    }

    private void Start()
    {
        enviromentLayer = LayerMask.NameToLayer("Enviroment");
    }

    public void FollowTarget(float delta)
    {
        //Vector3 targetPosition = Vector3.Lerp(myTransform.position, targetTransform.position, delta / followSpeed);
        Vector3 targetPosition = Vector3.SmoothDamp(myTransform.position, targetTransform.position, ref cameraFollowVelocity, followSpeed);
        myTransform.position = targetPosition;

        HandleCameraCollisions(Time.deltaTime);
    }

    public void HandleCameraRotation(float delta,float mouseInputX,float mouseInputY)
    {
        if (inputHandler.lockOnFlag==false&& currentLockOnTarget==null)
        {
            lookAngle += (mouseInputX * lookSpeed) / delta;
            pivotAngle -= (mouseInputY * pivotSpeed) / delta;
            pivotAngle = Mathf.Clamp(pivotAngle, minnumPivot, maxnumPivot);

            Vector3 rotation = Vector3.zero;
            rotation.y = lookAngle;
            Quaternion targetRotation = Quaternion.Euler(rotation);
            myTransform.rotation = targetRotation;

            rotation = Vector3.zero;
            rotation.x = pivotAngle;

            targetRotation = Quaternion.Euler(rotation);
            cameraPivotTransform.localRotation = targetRotation;
        }
        else
        {
            //float velocity = 0;

            Vector3 dir = currentLockOnTarget.transform.position - transform.position;
            dir.Normalize();
            dir.y = 0;

            Quaternion targetRotation = Quaternion.LookRotation(dir);
            //transform.rotation = targetRotation;
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime*5) ;

            dir = currentLockOnTarget.transform.position - cameraPivotTransform.position;
            dir.Normalize();

            targetRotation = Quaternion.LookRotation(dir);
            Vector3 eulerAngle = targetRotation.eulerAngles;
            eulerAngle.y = 0;
            //cameraPivotTransform.localEulerAngles = eulerAngle;
            cameraPivotTransform.localEulerAngles = Vector3.Lerp(cameraPivotTransform.localEulerAngles, eulerAngle,Time.deltaTime*5);
        }
    }

    public void HandleCameraCollisions(float delta)
    {
        targetPosition = defaultPosition;
        RaycastHit hit;
        Vector3 direction = cameraTransform.position - cameraPivotTransform.position;//相机和跟随目标的距离
        direction.Normalize();
        if (Physics.SphereCast(cameraPivotTransform.position,cameraSphereRadius,direction,out hit,Mathf.Abs(targetPosition),ignoredLayers))//检测摄像机周围球型范围的空间，如果检测到碰撞器就会返回true
        {
            float dis = Vector3.Distance(cameraPivotTransform.position, hit.point);
            targetPosition = -(dis-cameraCollisionOffset);
        }
        if (Mathf.Abs(targetPosition) < minimumCollisiontOffset)
        {
            targetPosition = -minimumCollisiontOffset;
        }
        //cameraTransformPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, delta / 0.2f);
        cameraTransformPosition.z = targetPosition;
        cameraTransform.localPosition = cameraTransformPosition;
    }

    public void HandleLockOn()
    {
        float shortestDistance = Mathf.Infinity;
        float shortestDistanceOfLeftTarget = -Mathf.Infinity;
        float shortestDistanceOfRightTarget = Mathf.Infinity;

        Collider[] colliders = Physics.OverlapSphere(targetTransform.position, 26);

        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterManager character = colliders[i].GetComponent<CharacterManager>();
            
            if (character!=null)
            {
                Vector3 lockTargetDirection = character.transform.position - targetTransform.position;
                float distanceFromTarget = Vector3.Distance(targetTransform.position,character.transform.position);
                float viewableAngle = Vector3.Angle(lockTargetDirection, cameraTransform.forward);
                RaycastHit hit;
                
                if (character.transform.root!=targetTransform.transform.root
                    &&viewableAngle>-50&&viewableAngle<50
                    &&distanceFromTarget<=maximunLockOnDistance)
                {
                    if (Physics.Linecast(playerManager.lockOnTransform.position,character.transform.position,out hit))
                    {
                        Debug.DrawLine(playerManager.lockOnTransform.position, character.transform.position);

                        if (hit.transform.gameObject.layer==enviromentLayer)
                        {
                            //不能锁定目标了
                        }
                        else
                        {
                            availableTargets.Add(character);
                        }
                    }
                }
            }
        }
        for (int j = 0; j < availableTargets.Count; j++)
        {
            float distanceFromTarget = Vector3.Distance(targetTransform.position, availableTargets[j].transform.position);

            if (distanceFromTarget<shortestDistance)
            {
                shortestDistance = distanceFromTarget;
                nearestLockOnTarget = availableTargets[j];
            }

            if (inputHandler.lockOnFlag)
            {
                //Vector3 relativeEnemyPosition = currentLockOnTarget.transform.InverseTransformPoint(availableTargets[j].transform.position);
                //var distanceFromLeftTarget = currentLockOnTarget.transform.position.x - availableTargets[j].transform.position.x;
                //var distanceFromRightTarget = currentLockOnTarget.transform.position.x + availableTargets[j].transform.position.x;
                Vector3 relativeEnemyPosition = inputHandler.transform.InverseTransformPoint(availableTargets[j].transform.position);
                var distanceFromLeftTarget = relativeEnemyPosition.x;
                var distanceFromRightTarget = relativeEnemyPosition.x;

                if (relativeEnemyPosition.x <= 0.00 && distanceFromLeftTarget < shortestDistanceOfLeftTarget 
                    && availableTargets[j]!=currentLockOnTarget)
                {
                    shortestDistanceOfLeftTarget = distanceFromLeftTarget;
                    leftLockTarget = availableTargets[j];
                }
                else if (relativeEnemyPosition.x >= 0.00 && distanceFromRightTarget < shortestDistanceOfRightTarget
                    && availableTargets[j]!=currentLockOnTarget)
                {
                    shortestDistanceOfLeftTarget = distanceFromLeftTarget;
                    rightLockTarget = availableTargets[j];
                }
            }
        }
    }

    public void ClearLockOnTargets()
    {
        availableTargets.Clear();
        nearestLockOnTarget = null;
        currentLockOnTarget = null;

    }

    public void SetCameraHeight()
    {
        Vector3 velocity = Vector3.zero;
        Vector3 newLockedPosition = new Vector3(0, lockedPivotPosition);
        Vector3 newUnlockedPosition = new Vector3(0, unlockedPivotPosition);

        if (currentLockOnTarget!=null)
        {
            cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(cameraPivotTransform.transform.localPosition, newLockedPosition, ref velocity, Time.deltaTime);
        }
        else
        {
            cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(cameraPivotTransform.transform.localPosition, newUnlockedPosition, ref velocity, Time.deltaTime);
        }
    }
}
