using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] WallRun wallRun;

    [SerializeField] private float sensX = 100f;
    [SerializeField] private float sensY = 100f;

    [SerializeField] Transform cam = null;
    [SerializeField] Transform orientation;

    float mouseX;
    float mouseY;

    float multiplier = 0.01f;

    float xRotation;
    float yRotation;

    private void Start()
    {    
        Cursor.lockState = CursorLockMode.Locked; //鎖定鼠標
        Cursor.visible = false; //不顯示鼠標
    }

    private void Update()
    {
        MyInput();

        cam.transform.rotation = Quaternion.Euler(xRotation, yRotation, wallRun.tilt); //Quaternion.Euler(float x, float y, float z)將歐拉角轉成四元數
        orientation.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    void MyInput()
    {
        mouseX = Input.GetAxisRaw("Mouse X");
        mouseY = Input.GetAxisRaw("Mouse Y");
         
        yRotation += mouseX * sensX * multiplier;
        xRotation -= mouseY * sensY * multiplier;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f); //Mathf.Clamp(value, mini, max) 限制值的大小
    }
}
