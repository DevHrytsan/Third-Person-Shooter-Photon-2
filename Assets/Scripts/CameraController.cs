using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Runtime.Remoting.Lifetime;
using UnityEngine;

public class CameraController : MonoBehaviourPunCallbacks
{
    [Header("Reference")]
    public Transform cameraTransform;
    public Transform pivotTransform;
    public Transform characterTransform;
    public Transform m_Transform;
    public Transform targetLook;
    public CharacterStatus characterStatus;
    public CameraConfig cameraConfig;
    [Header("Properties")]
    public bool leftPivot;
    public float delta;
    public float mouseX;
    public float mouseY;
    public float smoothX;
    public float smoothY;
    [Header("Smooth")]
    public float smoothXVelocity;
    public float smoothYVelocity;
    public float lookAngle;
    public float titlAngle;
    [Header("Recover")]
    public float recoilRecoverX = 2f;
    public float recoilRecoverY = 2f;

    float recoilX;
    float recoilY;
    PlayerController playerController;
    private void Start()
    {
        cameraTransform.gameObject.SetActive(characterTransform.GetComponent<PhotonView>().IsMine);
        playerController = characterTransform.gameObject.GetComponent<PlayerController>();
    }
    void FixedTick()
    {
        delta = Time.deltaTime;
        HandlePosition();
        HandleRotation();
        
        Vector3 targetPosition = Vector3.Lerp(m_Transform.position, characterTransform.position, 0.1f);
        m_Transform.position = targetPosition;

    }
    private void Update()
    {
        if (!characterTransform.GetComponent<PhotonView>().IsMine) return;
        if (!PauseMenu.paused)
        {
            FixedTick();
        }
        TargetLook();
    }

    void HandlePosition()
    {
        Vector3 target = cameraConfig.normal;

        if (playerController.IsAiming)
        {
            target.x = cameraConfig.aimX;
            target.z = cameraConfig.aimZ;
        }
        if (leftPivot)
        {
            target.x = -target.x;
        }

        Vector3 newPivotPosition = pivotTransform.localPosition;
        newPivotPosition.x = target.x;
        newPivotPosition.y = target.y;

        Vector3 newCameraPosition = cameraTransform.localPosition;
        newCameraPosition.z = target.z;

        float t = delta * cameraConfig.pivotSpeed;
        pivotTransform.localPosition = Vector3.Lerp(pivotTransform.localPosition, newPivotPosition, t);
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newCameraPosition, t);
    }
    void HandleRotation()
    {
        mouseX = recoilX + Input.GetAxis("Mouse X");
        mouseY = recoilY + Input.GetAxis("Mouse Y");
        recoverRecoil();
        if (cameraConfig.turnSmooth > 0)
        {
            smoothX = Mathf.SmoothDamp(smoothX, mouseX, ref smoothXVelocity, cameraConfig.turnSmooth);
            smoothY = Mathf.SmoothDamp(smoothY, mouseY, ref smoothYVelocity, cameraConfig.turnSmooth);

        }
        else
        {
            smoothX = mouseX;
            smoothY = mouseY;

        }
        lookAngle += smoothX * cameraConfig.Y_rot_speed;
        Quaternion targetRot = Quaternion.Euler(0, lookAngle, 0);
        m_Transform.rotation = targetRot;

        titlAngle -= smoothY * cameraConfig.Y_rot_speed;
        titlAngle = Mathf.Clamp(titlAngle, cameraConfig.minAngle, cameraConfig.maxAngle);
        pivotTransform.localRotation = Quaternion.Euler(titlAngle, 0, 0);
    }
    void TargetLook()
    {
        RaycastHit hit;
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward * 1000, out hit))
        {
            targetLook.position = Vector3.Lerp(targetLook.position, hit.point, Time.deltaTime * 40);
        }
        else
        {
            targetLook.position = Vector3.Lerp(targetLook.position, targetLook.transform.forward * 1000, Time.deltaTime * 40);

        }
    }
    public void AddRecoil(float x, float y)
    {
        recoilX = x;
        recoilY = y;
    }
    private void recoverRecoil()
    {
        recoilX -= recoilRecoverX * Time.deltaTime;
        recoilY -= recoilRecoverY * Time.deltaTime;
        if (recoilX < 0)
        {
            recoilX = 0;
        }
        if (recoilY < 0)
        {
            recoilY = 0;
        }
    }
}
