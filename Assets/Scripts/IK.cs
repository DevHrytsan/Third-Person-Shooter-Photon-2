using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class IK : MonoBehaviourPunCallbacks, IPunObservable
{
    public Animator animController;
    public PlayerController playerController;
    public CharacterInventory characherInventory;
    [Header("LeftHandTarget")]
    public Transform l_Hand_Target;
    [Header("Weight")]
    public float rh_Weight;
    public float lh_Weight;
    [Header("Bones(AutoSet)")]
    public Transform shoulder;
    public Transform aimPivot;
    public Quaternion lh_rot;
    public Transform l_Hand;
    public Transform r_Hand;

    // Start is called before the first frame update
    void Start()
    {
        //if (!photonView.IsMine)
       // {
           // animController.SetLayerWeight(1, 0);
            //return;
       // }
            shoulder = animController.GetBoneTransform(HumanBodyBones.RightShoulder).transform;
          
            //aimPivot = new GameObject().transform;
            //aimPivot.name = "aim pivot";
            //aimPivot.transform.parent = transform;

           // r_Hand = new GameObject().transform;
           // r_Hand.name = "right hand";
           //r_Hand.transform.parent = aimPivot;

            //l_Hand = new GameObject().transform;
            //l_Hand.name = "left hand";
            //l_Hand.transform.parent = aimPivot;

            //r_Hand.localPosition = characherInventory.CurrentProperties().rHandPos;
            //Quaternion rotRight = Quaternion.Euler(characherInventory.CurrentProperties().rHandRot.x, characherInventory.CurrentProperties().rHandRot.y, characherInventory.firstWeapon.rHandRot.z);
            //r_Hand.localRotation = rotRight;     
    }
    [PunRPC]
    public void ChangeIK(Vector3 rHandPos,Vector3 rHandRot)
    {

        r_Hand.localPosition = rHandPos;
        Quaternion rotRight = Quaternion.Euler(rHandRot.x, rHandRot.y, rHandRot.z);
        r_Hand.localRotation = rotRight;
    }
    public void SetLeftHand(Transform leftHand)
    {
        l_Hand_Target = leftHand;

    }
    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine) return;

        if (!playerController.IsHands)
        {
            lh_rot = l_Hand_Target.rotation;

            l_Hand.position = l_Hand_Target.position;
        }
        if (playerController.IsAiming && !playerController.IsHands)
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

            if (!playerController.IsHands)
            {
                if (playerController.IsAiming)
                {
                    animController.SetLookAtWeight(1, 0, 1);

                    aimPivot.LookAt(playerController.targetLook.position);
                    animController.SetLookAtPosition(playerController.targetLook.position);

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
                    animController.SetLookAtPosition(playerController.targetLook.position);
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
    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(l_Hand_Target.position);
            stream.SendNext(l_Hand_Target.rotation);

            stream.SendNext(rh_Weight);
            stream.SendNext(lh_Weight);

            stream.SendNext(shoulder.position);
            stream.SendNext(shoulder.rotation);

            stream.SendNext(aimPivot.position);
            stream.SendNext(aimPivot.rotation);

            stream.SendNext(lh_rot);

            stream.SendNext(l_Hand.position);
            stream.SendNext(l_Hand.rotation);

            stream.SendNext(r_Hand.position);
            stream.SendNext(r_Hand.rotation);
            Debug.Log("IK_Local");

        }
        else
        {

            l_Hand_Target.position =  (Vector3)stream.ReceiveNext();
            l_Hand_Target.rotation = (Quaternion)stream.ReceiveNext();

            rh_Weight = (float)stream.ReceiveNext();
            lh_Weight = (float)stream.ReceiveNext();

            shoulder.position = (Vector3)stream.ReceiveNext();
            shoulder.rotation = (Quaternion)stream.ReceiveNext();

            aimPivot.position = (Vector3)stream.ReceiveNext();
            aimPivot.rotation = (Quaternion)stream.ReceiveNext();
            //////////////////////////////////////////////////////////
            lh_rot = (Quaternion)stream.ReceiveNext();
            /////////////////////////////////////////////////////////
            l_Hand.position = (Vector3)stream.ReceiveNext();
            l_Hand.rotation = (Quaternion)stream.ReceiveNext();
            ///////////////////////////////////////////////////////////
            r_Hand.position = (Vector3)stream.ReceiveNext();
            r_Hand.rotation = (Quaternion)stream.ReceiveNext();
            Debug.Log("IK_Remote" + animController.GetLayerWeight(1) + animController.GetLayerWeight(2));

        }


    }
}
