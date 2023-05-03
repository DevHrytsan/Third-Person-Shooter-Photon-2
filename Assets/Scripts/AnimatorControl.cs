using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AnimatorControl : MonoBehaviourPunCallbacks
{
    public Animator animController;
    public PlayerController playerController;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        animController = GetComponent<Animator>();
    }
    private void Update()
    {
        if (!photonView.IsMine) return;
        UpdateAnimation();
    }
    private void UpdateAnimation()
    {
        animController.SetBool("IsAiming", playerController.IsAiming);
        animController.SetBool("IsAimingMove", playerController.IsAimingMove);

        AirAnimation();
        if (!playerController.IsAimingMove)
        {
            AnimationNormal();
        }
        else
        {
            AnimationAiming();
        }
        if (playerController.IsOnGround && playerController.IsJump)
        {
            animController.SetTrigger("IsJump");
        }
    }
    private void AnimationNormal()
    {
        if (playerController.IsSprint)
        {
            animController.SetFloat("MoveAmount", playerController.moveAmount, 0.15f, Time.deltaTime);
        }
        else
        {
            animController.SetFloat("MoveAmount", playerController.moveAmount / 2, 0.15f, Time.deltaTime);

        }
    }
    private void AnimationAiming()
    {
        float v = playerController.verticalInput;
        float h = playerController.horizontalInput;
        animController.SetFloat("Vertical", v, 0.15f, Time.deltaTime);
        animController.SetFloat("Horizontal", h, 0.15f, Time.deltaTime);

    }
    private void AirAnimation()
    {
        animController.SetBool("IsGrounded", playerController.IsOnAir);

    }

}
