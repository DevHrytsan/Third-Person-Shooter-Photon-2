using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PhotonDestroy : MonoBehaviourPun
{
    [SerializeField]
    private float destroyTime = 2f;
    private float counter;
    private void Update()
    {
        if (photonView.IsMine)
        {
            Destroyed();
        }
    }

    private void Destroyed()
    {
        counter += Time.deltaTime;
        if (counter >= destroyTime)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}

