using UnityEngine;
using System.Collections;
using Photon.Pun;

public class IntegrationMethods : MonoBehaviourPunCallbacks
{
    public static void BackwardEuler(
        float h,
        Vector3 currentPosition,
        Vector3 currentVelocity,
        out Vector3 newPosition,
        out Vector3 newVelocity)
    {

        Vector3 acceleartionFactor = Physics.gravity;

        newVelocity = currentVelocity + h * acceleartionFactor;

        newPosition = currentPosition + h * newVelocity;
    }
}