using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Manager : MonoBehaviourPun
{
    public string player_prefab;
    public Transform[] spawnPoints;

    // Start is called before the first frame update
    void Start()
    {
        Spawn();
    }

    // Update is called once per frame
   public void Spawn()
    {
        Transform t_spawn = spawnPoints[Random.Range(0, spawnPoints.Length)];
        PhotonNetwork.Instantiate(player_prefab, t_spawn.position, t_spawn.rotation);

    }
}
