﻿using System;
using UnityEngine;

public class WallRun : MonoBehaviour
{
    [Header("Properties")]

    public bool isWallRunning = false; //If the player is currently WallRunning.
    public float wallRunDuration = 4; //The second before stop wallrun.
    public float upForce = 15; // The vertical force applied when the player enter in wallRun.
    public float constantUpForce = 10;// The vertical force to not let the player fall.
    public float WallForce = 4000;
    public float forwardForce = 18;
    public bool isWallLeft;
    public bool isWallRight;
    
    private int prevWallRunningExtraJump;
    public Transform orientation; 

    [HideInInspector]
    public bool extraExitWallrunJump = false;

    [HideInInspector]
    public int wallRunningExtraJump = 2;

    [Header("Camera")]
  
    public float rayDistance;
    public float camAngle = 20;
    public float curCamAngle;
    public Transform cam;

    private Vector3 wallDir; //The direction to the wall
    [Header("Controller")]
    public PlayerMovement controller; // Your Controller

    public Rigidbody rb; // The rigidbody

    public static WallRun instance;

    private bool isCancellingWallrunning;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        instance = this;
        wallDir = Vector3.up;
        prevWallRunningExtraJump = wallRunningExtraJump;
    }

    private void Update()
    {
        WallRunning();
        if(cam != null)
        {
           
            if (Physics.Raycast(transform.position, orientation.right, rayDistance) && isWallRunning)
            {
                curCamAngle = camAngle;
                isWallRight = true;
                isWallLeft = false;
          
            }
            else if (Physics.Raycast(transform.position, -orientation.right, rayDistance) && isWallRunning)
            {
                curCamAngle = -camAngle;
                isWallRight = false;
                isWallLeft = true;
            }
            else
            {
                curCamAngle = 0;
                isWallRight = false;
                isWallLeft = false;
            }
            

            
        }
    }


    private void EnterInWall()
    {
        if(controller.grounded == true && isWallRunning == true){
            wallRunningExtraJump = prevWallRunningExtraJump;
        }

        if (!isWallRunning)
        {
            rb.AddForce(Vector3.up * upForce, ForceMode.Impulse);
            rb.AddForce(orientation.forward * forwardForce, ForceMode.Impulse);
            Debug.Log("Start Wall Run");
        }

        isWallRunning = true;
    }

    private void WallRunning()
    {
        if (isWallRunning)
        {
            rb.AddForce(-wallDir * WallForce * Time.deltaTime); // Push the player on the wall.
            rb.AddForce(Vector3.up * constantUpForce * Time.deltaTime); // Apply a constant force to not let the player fall.
            if (wallRunningExtraJump > 0 && extraExitWallrunJump == true){
                controller.grounded = true;
            }
            // if (Input.GetKeyDown("space")){
            //     wallRunningExtraJump -= 1;
            // }
            if (wallRunningExtraJump <= 0){
                wallRunningExtraJump = 0;
                 controller.grounded = false;
            }
            
        }
        
    }

    //Check the angle of the surface.
    private bool CheckSurfaceAngle(Vector3 v, float angle)
    {
        return Math.Abs(angle - Vector3.Angle(Vector3.up, v)) < 0.1f;
    }


    private void ExitWallRunning()
    {
        if(controller.grounded == true && isWallRunning == false || controller.grounded == false && isWallRunning == false){
            wallRunningExtraJump = prevWallRunningExtraJump;
        }
        isWallRunning = false;
    }


    private void OnCollisionStay(Collision other)
    {
        Vector3 surface = other.contacts[0].normal;
        if (CheckSurfaceAngle(surface, 90))
        {
            EnterInWall();
            wallDir = surface;

            isCancellingWallrunning = false;
            CancelInvoke("ExitWallRunning");
        }
 
        if (!isCancellingWallrunning)
        {
            isCancellingWallrunning = true;
            Invoke("ExitWallRunning", wallRunDuration * Time.deltaTime);
        }

    }

    private void OnCollisionExit(Collision collision)
    {
        if (isWallRunning)
        {
            ExitWallRunning();
   
        }
        
    }

}