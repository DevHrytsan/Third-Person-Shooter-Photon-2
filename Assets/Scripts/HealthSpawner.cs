using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HealthSpawner : MonoBehaviourPunCallbacks
{
    public string firstAid;
    public Transform[] spawnPoints;
    public float TimeToSpawnNew = 15f;
    private float startTime;
    private void Start()
    {
        Spawn();
    }
    public void Spawn()
    {
        Transform t_spawn = spawnPoints[Random.Range(0, spawnPoints.Length)];
        PhotonNetwork.Instantiate(firstAid, t_spawn.position, t_spawn.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        startTime += Time.deltaTime;
        if (startTime > TimeToSpawnNew)
        {
            Spawn();
            startTime = 0;
        }
    }
}
