using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character/Status")]
public class CharacterStatus : ScriptableObject
{
    public bool IsAiming;
    public bool IsAimingMove;
    public bool IsOnAir;
    public bool IsSprint;
    public bool IsGround;
}
