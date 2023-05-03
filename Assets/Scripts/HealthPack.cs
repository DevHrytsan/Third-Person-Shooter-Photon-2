using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviourPunCallbacks
{
    public int healthCount = 50;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            if (other.gameObject.GetComponent<PlayerController>().current_health != 100) {
                other.gameObject.GetComponent<PhotonView>().RPC("TakeHealth", RpcTarget.All, healthCount);
                photonView.RPC("DestroyPack", RpcTarget.All);
            }
        }
    }
    [PunRPC]
    void DestroyPack()
    {
        PhotonNetwork.Destroy(gameObject);
    }
}
