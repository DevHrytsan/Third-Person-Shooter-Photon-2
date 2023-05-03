using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInventory : MonoBehaviourPunCallbacks, IPunObservable
{
    public WeaponProperties firstWeapon;
    public GameObject firstWeaponPrefab;
    public Transform firstWeaponModel;

    public WeaponProperties secondWeapon;
    public GameObject secondWeaponPrefab;
    public Transform secondWeaponModel;

    public IK IkControl;
    public PlayerController playerController;

    public int selectedWeapon = 1;
    private void Start()
    {
        if (photonView.IsMine)
        {
            photonView.RPC("EmptySet", RpcTarget.All);
        }
    }
    void Update()
    {
        if (!photonView.IsMine) return;
        WeaponChange();
    }
    private void WeaponChange()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            playerController.IsHands = false;
            photonView.RPC("SetFirstWeapon", RpcTarget.All);
            //characterInventory.SetFirstWeapon();
            selectedWeapon = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            playerController.IsHands = false;
            //SetSecondWeapon();
            photonView.RPC("SetSecondWeapon", RpcTarget.All);
            selectedWeapon = 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            photonView.RPC("EmptySet", RpcTarget.All);
        }
    }

    [PunRPC]
    void EmptySet()
    {

        playerController.IsHands = true;
        photonView.RPC("SetEmptyWeapon", RpcTarget.All);
        selectedWeapon = 0;
        playerController.IsAiming = false;
        playerController.IsAimingMove = false;
    }

    [PunRPC]
    void SetFirstWeapon()
    {
        IkControl.l_Hand_Target = firstWeaponPrefab.GetComponent<EasyWeapon>().leftHand.transform;
       // photonView.RPC("SetLeftHand", RpcTarget.All, firstWeaponPrefab.GetComponent<EasyWeapon>().leftHand.position, firstWeaponPrefab.GetComponent<EasyWeapon>().leftHand.rotation);
        //photonView.RPC("ChangeIK", RpcTarget.All, firstWeapon);
        photonView.RPC("ChangeIK",RpcTarget.All,firstWeapon.rHandPos, firstWeapon.rHandRot);
        IkControl.animController.SetLayerWeight(1, 1);
        firstWeaponPrefab.SetActive(true);
        firstWeaponModel.gameObject.SetActive(false);
        secondWeaponPrefab.SetActive(false);
        secondWeaponModel.gameObject.SetActive(true);
    }
    [PunRPC]
    void SetSecondWeapon()
    {
      
        IkControl.l_Hand_Target = secondWeaponPrefab.GetComponent<EasyWeapon>().leftHand.transform;
       // photonView.RPC("SetLeftHand", RpcTarget.All, secondWeaponPrefab.GetComponent<EasyWeapon>().leftHand.position, secondWeaponPrefab.GetComponent<EasyWeapon>().leftHand.rotation);
        //photonView.RPC("ChangeIK", RpcTarget.All, secondWeapon);
        photonView.RPC("ChangeIK", RpcTarget.All, secondWeapon.rHandPos, secondWeapon.rHandRot);
        IkControl.animController.SetLayerWeight(1, 1);
        firstWeaponPrefab.SetActive(false);
        firstWeaponModel.gameObject.SetActive(true);
        secondWeaponPrefab.SetActive(true);
        secondWeaponModel.gameObject.SetActive(false);
    }
    [PunRPC]
    void SetEmptyWeapon()
    {
        IkControl.animController.SetLayerWeight(1, 0);
        firstWeaponPrefab.SetActive(false);
        firstWeaponModel.gameObject.SetActive(true);
        secondWeaponPrefab.SetActive(false);
        secondWeaponModel.gameObject.SetActive(true);
        //IkControl.l_Hand_Target = null;
        //IkControl.ChangeIK(null);

    }
    public WeaponProperties CurrentProperties()
    {
        if (selectedWeapon == 1)
        {
            return firstWeapon;
        }
        else if (selectedWeapon == 2)
        {
            return secondWeapon;
        }
        else
        {
            return firstWeapon;
        }
    }
    public void CurrentWeapon(int currentWeapon, bool shoot)
    {
        if (selectedWeapon == 1)
        {
            firstWeaponPrefab.GetComponent<EasyWeapon>().shootInput = shoot;
        }
        if (selectedWeapon == 2)
        {
            secondWeaponPrefab.GetComponent<EasyWeapon>().shootInput = shoot;
        }

    }
    void IPunObservable.OnPhotonSerializeView(PhotonStream stream,PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(firstWeaponPrefab.activeSelf);
            stream.SendNext(firstWeaponModel.gameObject.active);
            stream.SendNext(secondWeaponPrefab.active);
            stream.SendNext(secondWeaponModel.gameObject.active);
          //  stream.SendNext(IkControl.animController.GetLayerWeight(1));
            //stream.SendNext(IkControl.animController.GetLayerWeight(2));


        }
        else
        {
            firstWeaponPrefab.SetActive((bool)  stream.ReceiveNext());
            firstWeaponModel.gameObject.SetActive((bool)stream.ReceiveNext());
            secondWeaponPrefab.SetActive((bool) stream.ReceiveNext());
            secondWeaponModel.gameObject.SetActive((bool)stream.ReceiveNext());
            //IkControl.animController.SetLayerWeight(1, (float)stream.ReceiveNext());
            //IkControl.animController.SetLayerWeight(2, (float)stream.ReceiveNext());

        }
    }
}
