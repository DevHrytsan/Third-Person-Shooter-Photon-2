using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasyAnimatorController : MonoBehaviour
{
    public Animator animController;
    EasyInputController easyInputController;
    public CharacterStatus characterStatus;

    // Start is called before the first frame update
    void Start()
    {
        easyInputController = GetComponent<EasyInputController>();
    }

    // Update is called once per frame
    public void UpdateAnimation()
    {
        animController.SetBool("IsAiming", characterStatus.IsAiming);
        animController.SetBool("IsAimingMove", characterStatus.IsAimingMove);

        AirAnimation();
        if (!characterStatus.IsAimingMove)
        {
            AnimationNormal();
        }
        else
        {
            AnimationAiming();
        }
    }
    void AnimationNormal()
    {
        if (characterStatus.IsSprint)
        {
            animController.SetFloat("MoveAmount", easyInputController.moveAmount, 0.15f, Time.deltaTime);
        }
        else
        {
            animController.SetFloat("MoveAmount", easyInputController.moveAmount / 2, 0.15f, Time.deltaTime);

        }
    }
    void AnimationAiming()
    {
        float v = easyInputController.verticalInput;
        float h = easyInputController.horizontalInput;
        animController.SetFloat("Vertical", v, 0.15f, Time.deltaTime);
        animController.SetFloat("Horizontal", h, 0.15f, Time.deltaTime);

    }
    void AirAnimation()
    {
        animController.SetBool("IsGrounded", characterStatus.IsOnAir);

    }
}
