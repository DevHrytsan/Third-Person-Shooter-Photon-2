using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasyController : MonoBehaviour
{
    public Transform cameraTransform;
    public Animator animController;

    public float moveAmount = 0.4f;
    public float rotationSpeed = 0.4f;

    public Vector3 rotationDirection;
    public Vector3 moveDirection;

    [Header("AirControl")]
    public float airTime;
    public float minAirTime = 0.25f;

    EasyInputController easyInputController;
    public CharacterStatus characterStatus;

    // Start is called before the first frame update
    void Start()
    {
        easyInputController = GetComponent<EasyInputController>();
    }

    // Update is called once per frame

    public void MoveUpdate()
    {
        Vector3 moveDir = cameraTransform.forward * easyInputController.verticalInput;
        moveDir += cameraTransform.right * easyInputController.horizontalInput;
        moveDir.Normalize();
        moveDirection = moveDir;
        rotationDirection = cameraTransform.forward;
        Rotation();
        characterStatus.IsGround = Ground();
        OnAir();
    }
    public void Rotation()
    {
        if (!characterStatus.IsAimingMove)
        {
            rotationDirection = moveDirection;
        }
        Vector3 targetDir = rotationDirection;
        targetDir.y = 0;

        if (targetDir == Vector3.zero)
        {
            targetDir = transform.forward;
        }

        Quaternion lookDir = Quaternion.LookRotation(targetDir);
        Quaternion targetRot = Quaternion.Slerp(transform.rotation, lookDir, rotationSpeed);
        transform.rotation = targetRot;

    }

    public bool Ground()
    {
        Vector3 origin = transform.position;
        origin.y += 0.6f;
        Vector3 dir = -Vector3.up;
        float dis = 0.7f;
        RaycastHit hit;
        if (Physics.Raycast(origin, dir, out hit, dis))
        {
            Vector3 tp = hit.point;
            transform.position = tp;
            return true;
        }
        return false;
    }
    public void OnAir()
    {
        if (!characterStatus.IsGround)
        {
            airTime += Time.deltaTime;
        }
        else
        {
            airTime = 0;
        }
        if (airTime > minAirTime)
        {
            characterStatus.IsOnAir = false;
        }
        else
        {
            characterStatus.IsOnAir = true;

        }
    }
}
