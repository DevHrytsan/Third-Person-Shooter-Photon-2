using UnityEngine;
using System.Collections;
using Photon.Pun;

public class Bullet : MonoBehaviourPunCallbacks
{
    public float Damage = 30f;

    [Tooltip("BulletHoles")]
    public string[] BulletHoles;
    public float BulletlifeTime = 10;
    public Vector3 currentPosition;
    public Vector3 currentVelocity;

    Vector3 newPosition = Vector3.zero;
    Vector3 newVelocity = Vector3.zero;

    void Awake()
    {

        currentPosition = transform.position;
    }

    void Update()
    {
        DestroyBullet();
    }

    void FixedUpdate()
    {
        MoveBullet();
    }

    //Did we hit a target
    void CheckHit()
    {

        Vector3 fireDirection = (newPosition - currentPosition).normalized;
        float fireDistance = Vector3.Distance(newPosition, currentPosition);

        RaycastHit hit;

        if (Physics.Raycast(currentPosition, fireDirection, out hit, fireDistance))
        {
            if (hit.collider)
            {
                //Debug.Log("Hit target!");
                   if (photonView.IsMine && hit.collider.gameObject.layer == 11)
                    {
                        Debug.Log("Hit Player");
                        hit.collider.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, Damage);
                        PhotonNetwork.Destroy(gameObject);
                    }             
                photonView.RPC("SpawnBulletHole", RpcTarget.All, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));

                // SpawnBulletHole(hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }
    [PunRPC]
    void SpawnBulletHole(Vector3 positon, Quaternion rotation)
    {
        int i = Random.Range(0, BulletHoles.Length);
        GameObject temp_bulletHole = PhotonNetwork.Instantiate(BulletHoles[i], positon, rotation) as GameObject;
        Destroy(temp_bulletHole, BulletlifeTime);
    }
    void MoveBullet()
    {
        float h = Time.fixedDeltaTime;
        Ballistic.CurrentIntegrationMethod(h, currentPosition, currentVelocity, out newPosition, out newVelocity);

        CheckHit();

        currentPosition = newPosition;
        currentVelocity = newVelocity;

        transform.position = currentPosition;
    }

    void DestroyBullet()
    {
        if (photonView.IsMine)
        {
            if (transform.position.x > 500 || transform.position.x < -500 || transform.position.z > 500 || transform.position.z < -500)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }
    //[PunRPC]
   // private void TakeDamage(int p_damage)
   // {
      //  GetComponent<PlayerController>().TakeDamage(p_damage);
    //}
}