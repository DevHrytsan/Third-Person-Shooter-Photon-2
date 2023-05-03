using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKCharacter : MonoBehaviour
{
    public Animator animController;
    public EasyController characterMovement;
    public CharacterInventory characherInventory;
    public CharacterStatus characterStatus;
    public Transform targetLook;

    public Quaternion lh_rot;
    public Transform l_Hand;
    public Transform l_Hand_Target;
    public Transform r_Hand;


    public float rh_Weight;
    public float lh_Weight;

    public Transform shoulder;
    public Transform aimPivot;

    // Start is called before the first frame update
    void Start()
    {
        shoulder = animController.GetBoneTransform(HumanBodyBones.RightShoulder).transform;

        aimPivot = new GameObject().transform;
        aimPivot.name = "aim pivot";
        aimPivot.transform.parent = transform;

        r_Hand = new GameObject().transform;
        r_Hand.name = "right hand";
        r_Hand.transform.parent = aimPivot;

        l_Hand = new GameObject().transform;
        l_Hand.name = "left hand";
        l_Hand.transform.parent = aimPivot;

        r_Hand.localPosition = characherInventory.firstWeapon.rHandPos;
        Quaternion rotRight = Quaternion.Euler(characherInventory.firstWeapon.rHandRot.x, characherInventory.firstWeapon.rHandRot.y, characherInventory.firstWeapon.rHandRot.z);
        r_Hand.localRotation = rotRight;

    }

    // Update is called once per frame
    void Update()
    {
        lh_rot = l_Hand_Target.rotation;

        l_Hand.position = l_Hand_Target.position;

        if (characterStatus.IsAiming)
        {
            rh_Weight += Time.deltaTime * 1.2f;
            lh_Weight += Time.deltaTime * 1.2f;

        }
        else
        {
            rh_Weight -= Time.deltaTime * 1.2f;
           lh_Weight -= Time.deltaTime * 1.2f;

        }
        rh_Weight = Mathf.Clamp(rh_Weight, 0, 1);
        lh_Weight = Mathf.Clamp(lh_Weight, 0, 1);

    }
    void OnAnimatorIK()
    {
        aimPivot.position = shoulder.position;

        if (characterStatus.IsAiming)
        {
            animController.SetLookAtWeight(1, 0, 1);

            aimPivot.LookAt(targetLook.position);
            animController.SetLookAtPosition(targetLook.position);

            animController.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
            animController.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
            animController.SetIKPosition(AvatarIKGoal.LeftHand, l_Hand.position);
            animController.SetIKRotation(AvatarIKGoal.LeftHand, lh_rot);


            animController.SetIKPositionWeight(AvatarIKGoal.RightHand, rh_Weight);
            animController.SetIKRotationWeight(AvatarIKGoal.RightHand, rh_Weight);
            animController.SetIKPosition(AvatarIKGoal.RightHand, r_Hand.position);
            animController.SetIKRotation(AvatarIKGoal.RightHand, r_Hand.rotation);

        }
        else
        {
            animController.SetLookAtWeight(0.3f, 0.3f, 1f);
            animController.SetLookAtPosition(targetLook.position);
            animController.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
            animController.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
            animController.SetIKPosition(AvatarIKGoal.LeftHand, l_Hand.position);
            animController.SetIKRotation(AvatarIKGoal.LeftHand, lh_rot);

         
            animController.SetIKPositionWeight(AvatarIKGoal.RightHand, rh_Weight);
            animController.SetIKRotationWeight(AvatarIKGoal.RightHand, rh_Weight);
            animController.SetIKPosition(AvatarIKGoal.RightHand, r_Hand.position);
            animController.SetIKRotation(AvatarIKGoal.RightHand, r_Hand.rotation);

        }
    }
}
