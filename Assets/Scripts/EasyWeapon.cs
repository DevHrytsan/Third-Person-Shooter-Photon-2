using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.Audio;

public class EasyWeapon : MonoBehaviourPunCallbacks
{
    public enum FireMode { Semi = 0, FullAuto = 1 };
    [Header("SpinningBarrel")]
    public bool SpiningBarrelMode;
    public Transform Barrel;
    public float SpinUpTime = 2;
    public float MaxSpinRate = 360;
    [Header("Properties")]
    public GameObject cameraMain;
    public WeaponProperties weaponProperties;
    public Transform bulletSpawnPoint;
    public Transform targetLookCamera;
    public RectTransform Crosshair;
    public CameraController cameraHolder;
    public Transform leftHand;
    public float RecoilX = 0.5f;
    public float RecoilY = 0.5f;
    public float range = 500f;
    [Header("Weapon")]
    public int Damage = 10;
    //public string projectile;
    public float projectileSpeed;
    public float RateOfFireMinute = 400;
    public FireMode FireModeSwitch = FireMode.Semi;
    [Header("Muzzle Flash")]
    public GameObject flashHolder;
    public float flashTime;
    [Header("Hit")]
    public string hitParticle;
    [Header("Trail")]
    public GameObject bulletTrail;

    [Header("Audio")]
    public AudioMixerGroup audioMixer;
    public AudioClip fireClip;
    public float VolumeMultiplayer = 1f;
    public float minVolume = 0.5f;
    public float maxVolume = 0.8f;

    Vector3 posCross;
    float fireRate, lastFire;
    float SpinUpTimer;
    [HideInInspector]
    public bool shootInput;

    // Start is called before the first frame update
    void Start()
    {
        if (!photonView.IsMine) return;
        fireRate = RateOfFireMinute / 60;
        DeactivateMuzzle();
    }

    private void Control()
    {

        // Debug.Log(shootInput);
        if (!SpiningBarrelMode)
        {
            if (shootInput && FireModeSwitch == FireMode.FullAuto)
            {

                if (Time.time - lastFire > 1 / fireRate)
                {
                    lastFire = Time.time;
                    //InitializationShoot();
                    photonView.RPC("InitializationShoot", RpcTarget.All);

                }
            }
            else if (shootInput && FireModeSwitch == FireMode.Semi)
            {
                //InitializationShoot();

                photonView.RPC("InitializationShoot", RpcTarget.All);

            }
        }
        else
        {
            if (shootInput)
            {
                SpinUpTimer = Mathf.Clamp(
                    SpinUpTimer + Time.deltaTime,
                    0, SpinUpTime);

                if (SpinUpTimer >= SpinUpTime)
                {
                    if (Time.time - lastFire > 1 / fireRate)
                    {
                        lastFire = Time.time;
                        photonView.RPC("InitializationShoot", RpcTarget.All);

                    }
                }
            }
            else
            {
                SpinUpTimer = Mathf.Clamp(
                    SpinUpTimer - Time.deltaTime,
                    0, SpinUpTime);
            }
            GraphicsUpdate();
        }
    }

    void Update()
    {
        if (!photonView.IsMine) return;
        AlignTarget();
        Control();
    }
    [PunRPC]
    void InitializationShoot()
    {
        RaycastHit hit;
        Vector3 shootDirection = bulletSpawnPoint.transform.forward;

        photonView.RPC("ActivateMuzzle", RpcTarget.All);
        photonView.RPC("WeaponSound", RpcTarget.All);

        if (Physics.Raycast(bulletSpawnPoint.position, shootDirection, out hit, range))
        {

            if (hit.collider.gameObject.layer == 11)
            {
                Debug.Log("Hit Player");
                hit.collider.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, Damage);
            }
            else
            {
                //PhotonNetwork.Instantiate(hitParticle, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
            }
            photonView.RPC("SpawnBulletTrail", RpcTarget.All, hit.point);
            //SpawnBulletTrail(hit.point);
        }

        cameraHolder.AddRecoil(Random.Range(-RecoilY / 5, RecoilY / 5), RecoilX / 5f);

    }
    //[PunRPC]
    // void InitializationShoot()
    //{

    //GameObject newBullet = PhotonNetwork.Instantiate(projectile, bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation) as GameObject;
    // newBullet.GetComponent<Bullet>().currentVelocity = projectileSpeed * bulletSpawnPoint.forward;
    //newBullet.GetComponent<Bullet>().Damage = Damage;
    //cameraHolder.AddRecoil(Random.Range(-0.5f / 5, 0.5f / 5), 1f / 5f);

    // }
    [PunRPC]
    private void SpawnBulletTrail(Vector3 hitPoint)
    {
        GameObject bulletTrailEffect = Instantiate(bulletTrail, bulletSpawnPoint.position, Quaternion.identity);
        //LineRenderer lineR = bulletTrailEffect.GetComponent<LineRenderer>();
        bulletTrailEffect.GetComponent<LineRenderer>().SetPosition(0, bulletSpawnPoint.position);
        bulletTrailEffect.GetComponent<LineRenderer>().SetPosition(1, hitPoint);

    }
    [PunRPC]
    public void WeaponSound()
    {
        AudioSource audioRPC = gameObject.AddComponent<AudioSource>();
        //AudioEchoFilter audioEchoRPC = gameObject.AddComponent<AudioEchoFilter>();


        audioRPC.clip = fireClip;
        audioRPC.playOnAwake = false;
        audioRPC.volume = Random.Range(minVolume / VolumeMultiplayer, maxVolume / VolumeMultiplayer);
        audioRPC.outputAudioMixerGroup = audioMixer;
        audioRPC.spatialBlend = 1;
        audioRPC.minDistance = 5;
        audioRPC.maxDistance = 100;
        audioRPC.Play();
    }
    private void GraphicsUpdate()
    {
        float theta = (SpinUpTimer / SpinUpTime) *
                MaxSpinRate * Time.deltaTime;
        Barrel.RotateAroundLocal(Vector3.forward, theta);
    }
    
    void AlignTarget()
    {
        bulletSpawnPoint.LookAt(targetLookCamera);
        Vector3 origin = bulletSpawnPoint.position;
        Vector3 dir = targetLookCamera.position;

        RaycastHit hit;

        Debug.DrawLine(origin, dir, Color.red);
        Debug.DrawLine(cameraMain.transform.position, dir, Color.red);


        if (Physics.Linecast(origin, dir, out hit))
        {
            posCross = Camera.main.WorldToScreenPoint(hit.point);
        }
        else
        {
            Crosshair.transform.position = new Vector3(0, 0, 0);
        }
        Crosshair.transform.position = posCross;
    }

    [PunRPC]
    public void ActivateMuzzle()
    {
        if (photonView.IsMine)
        {
            flashHolder.SetActive(true);
            Invoke("DeactivateMuzzle", flashTime);
        }
    }
    void DeactivateMuzzle()
    {
        flashHolder.SetActive(false);
    }
}
