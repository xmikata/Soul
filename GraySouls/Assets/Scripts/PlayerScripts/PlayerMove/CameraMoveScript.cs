using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveScript : MonoBehaviour
{
    public GameObject cameraFollowTarget;
    public float rotateSpeed;
    public float currentPitch;


    private void Awake()
    {
        cameraFollowTarget = Camera.main.transform.parent.gameObject;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X"); // 鼠标横向移动（左右）
        float mouseY = Input.GetAxis("Mouse Y"); // 鼠标纵向移动（上下）
        cameraFollowTarget.transform.Rotate(Vector3.up, mouseX * rotateSpeed * Time.deltaTime);
        currentPitch -= mouseY*rotateSpeed*Time.deltaTime;
        currentPitch = Mathf.Clamp(currentPitch,-60,60);
        cameraFollowTarget.transform.localEulerAngles = new Vector3(currentPitch, cameraFollowTarget.transform.localEulerAngles.y, 0f);
    }
}
