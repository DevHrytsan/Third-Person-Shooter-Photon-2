using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasyPlayerManager : MonoBehaviour
{
    public EasyInputController easyInputController;
    public EasyAnimatorController easyAnimatorController;
    public EasyController easyController;
    public CharacterStatus characterStatus;
    public EasyCameraController easyCameraController;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        easyController.MoveUpdate();
        easyAnimatorController.UpdateAnimation();

        characterStatus.IsAiming = easyInputController.IsAiming;
        characterStatus.IsAimingMove = easyInputController.IsAimingMove;
        characterStatus.IsSprint = easyInputController.IsSprint;
    }
}
