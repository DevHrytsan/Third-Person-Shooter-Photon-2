using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckIfGrounded : MonoBehaviour
{
    public Collider playerCollider;
    public Transform raycastPos;
    public bool isOnTerrain;
    public bool isInside;
    public bool isGrounded;

    RaycastHit hit;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    bool PlayerGrounded()
    {
        return Physics.Raycast(raycastPos.position, Vector3.down, out hit, playerCollider.bounds.extents.y + 1f);
        
    }
    bool CheckOnTerrain()
    {
        if(hit.collider != null  && hit.collider.tag == "Terrain")
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        isGrounded = PlayerGrounded();
        isOnTerrain = CheckOnTerrain();
    }
   // bool CheckIfInside()
    //{
    //Doen`t reliazed
    //}
}
