using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Camera/Config")]
public class CameraConfig : ScriptableObject
{
    public float turnSmooth;
    public float pivotSpeed;
    public float Y_rot_speed;
    public float X_rot_speed;
    public float minAngle;
    public float maxAngle;
    public Vector3 normal;
    public float aimX;
    public float aimZ;
   
}
