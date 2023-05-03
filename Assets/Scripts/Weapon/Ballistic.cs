using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Ballistic : MonoBehaviourPunCallbacks
{

    public Transform targetObj;
    public Transform gunObj;


    public static float bulletSpeed = 20f;

    static float h;

    void Awake()
    {

        h = Time.fixedDeltaTime * 1f;
    }

    public static void CurrentIntegrationMethod(
    float h,
    Vector3 currentPosition,
    Vector3 currentVelocity,
    out Vector3 newPosition,
    out Vector3 newVelocity)
    {
        IntegrationMethods.BackwardEuler(h, currentPosition, currentVelocity, out newPosition, out newVelocity);
    }
}