using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasyInputController : MonoBehaviour
{
    public float verticalInput;
    public float horizontalInput;
    public float moveAmount;
    [HideInInspector]
    public bool IsAiming;
    [HideInInspector]
    public bool IsAimingMove;

    public bool debugAim;
    [HideInInspector]
    public bool opportunityToAim;
    [HideInInspector]
    public bool IsSprint;
    [HideInInspector]
    public bool IsShooting;


    public CharacterStatus characterStatus;
    public EasyWeapon weapon;
    public Transform TargetLook;

    [HideInInspector]
    public bool LeftPivotMode;
    float distance;

    private void Update()
    {
        InputUpdate();
        RayCastAiming();
        MouseControl();
    }

    private void InputUpdate()
    {
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal"); 
        IsSprint = Input.GetButton("Sprint");

        weapon.shootInput = IsShooting;
        moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + Math.Abs(horizontalInput));

        characterStatus.IsAiming = IsAiming;
        characterStatus.IsAimingMove = IsAimingMove;
    }
    void MouseControl()
    {

        if (Input.GetButton("Fire2") && opportunityToAim)
        {
            IsAiming = true;
            IsAimingMove = true;
        }
        if (Input.GetButton("Fire2") && !opportunityToAim)
        {
            IsAiming = false;
            IsAimingMove = true;
        }
        if (!Input.GetButton("Fire2"))
        {
            IsAiming = false;
            IsAimingMove = false;
        }

        if (Input.GetButton("Fire1") && Input.GetButton("Fire2") && opportunityToAim)
        {
            IsShooting = true;
        }
        else
        {
            IsShooting = false;
        }
    }

    private void RayCastAiming()
    {
        Debug.DrawLine(transform.position + transform.up * 1.4f, TargetLook.position, Color.green);

        distance = Vector3.Distance(transform.position + transform.up * 1.4f, TargetLook.position);
        if (distance > 1.3f)
            opportunityToAim = true;
        else opportunityToAim = false;
    }

 
}
