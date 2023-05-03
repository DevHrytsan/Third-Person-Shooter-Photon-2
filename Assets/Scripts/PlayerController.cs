using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
using ExitGames.Client.Photon;

public class PlayerController : MonoBehaviourPunCallbacks
{
    public string Ragdoll;

    [Header("UI")]
    public int current_health;
    private int maxHealth = 100;
    private Transform ui_text;
    public TMP_Text ui_UserName;
    public bool IsPaused;

    [Header("PlayerStatus")]
    public bool IsAiming;
    public bool IsAimingMove;
    public bool IsOnAir;
    public bool IsSprint;
    public bool IsJump;
    public bool IsOnGround;
    public bool IsShooting;
    public bool IsHands;
    [Header("References")]
    public CharacterController characterController;

    public CharacterInventory characterInventory;
    public Animator animController;
    public EasyWeapon weapon;
    [Header("CameraProperties")]
    public Transform targetLook;
    public Transform cameraHolderTransform;
    public Transform MainCamera;
    [Header("Properties")]
    public float gravity = 9.81f;
    public float moveAmount;
    public float rotationSpeed = 0.4f;

    [Header("AirControl")]
    public float airTime;
    public float minAirTime = 2f;

    [Header("Debug")]
    public bool opportunityToAim;
    public bool debugAim;
    public bool LeftPivotMode;
    public Vector3 rotationDirection;
    public Vector3 moveDirection;

    [HideInInspector]
    public float verticalInput;
    [HideInInspector]
    public float horizontalInput;
    public bool DisableOnOwnObjects;

    float distance;
    Manager manager;
    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("Manager").GetComponent<Manager>();
        current_health = maxHealth;

        if (!photonView.IsMine)
        {
            gameObject.layer = 11;

        }

        if (photonView.IsMine)
        {
            ui_text = GameObject.Find("Canvas/HUD/Health").transform;
            //photonView.RPC("SyncProfile", RpcTarget.All);
            RefreshHealth();
            //ui_UserName.gameObject.SetActive(false);
        }

    }   
    private void SyncNickName()
    {
        bool showInfo = !this.DisableOnOwnObjects || this.photonView.IsMine;

        if (ui_UserName.gameObject != null)
        {
            ui_UserName.gameObject.SetActive(showInfo);
        }
        if (!showInfo)
        {
            return;
        }
        if (this.photonView.Owner != null)
        {
            ui_UserName.text = (string.IsNullOrEmpty(this.photonView.Owner.NickName)) ? "player" : this.photonView.Owner.NickName;
        }
        else if (this.photonView.IsSceneView)
        {
            ui_UserName.text = "USER" + Random.Range(0,999);
            
        }
        else
        {
            ui_UserName.text = "n/a";
        }

    }

    // Update is called once per frame
    void Update()
    {
        SyncNickName();

        if (!photonView.IsMine)
        {
            return;
        }
       
        MoveUpdate();
        BoolUpdate();
        InputUpdate();
        CheckHP();
        //GravityState();
        //SyncNickName();
        //photonView.RPC("SyncProfile", RpcTarget.All);

    }
     
    private void InputUpdate()
    {
        KeyBoardInput();
        FireInput();
        CameraChange();
        //WeaponChange();
        RayCastAiming();
    }
    private void BoolUpdate()
    {
        IsOnGround = OnGround();
        IsOnAir = OnAir();

    }
    private void MoveUpdate()
    {
        Vector3 moveDir = cameraHolderTransform.forward * verticalInput;
        moveDir += cameraHolderTransform.right * horizontalInput;
        moveDir.Normalize();
        moveDirection = moveDir;
        rotationDirection = cameraHolderTransform.forward;
        FollowCameraRotation();
    }

    void KeyBoardInput()
    {
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");
        IsSprint = Input.GetButton("Sprint");
        IsJump = Input.GetButtonDown("Jump");
        IsPaused = Input.GetKeyDown(KeyCode.Escape);

        if (IsPaused)
        {
            GameObject.Find("Pause").GetComponent<PauseMenu>().TogglePause();
        }

        if (PauseMenu.paused)
        {
            IsAiming = false;
            IsAimingMove = false;
            IsShooting = false;
            horizontalInput = 0;
            verticalInput = 0;
        }

        FireInput();
        characterInventory.CurrentWeapon(characterInventory.selectedWeapon, IsShooting);
        moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));

        if (Input.GetKeyDown(KeyCode.U)) TakeDamage(20);
    }
    void FireInput()
    {

        if (Input.GetButton("Fire2") && opportunityToAim && !IsHands)
        {
            IsAiming = true;
            IsAimingMove = true;
        }
        if (Input.GetButton("Fire2") && !opportunityToAim && !IsHands)
        {
            IsAiming = false;
            IsAimingMove = true;
        }
        if (!Input.GetButton("Fire2"))
        {
            IsAiming = false;
            IsAimingMove = false;
        }

        if (Input.GetButton("Fire1") && Input.GetButton("Fire2") && opportunityToAim && !IsHands)
        {
            IsShooting = true;
        }
        else
        {
            IsShooting = false;
        }
    }

    public void FollowCameraRotation()
    {
        if (!IsAimingMove)
        {
            rotationDirection = moveDirection;
        }
        Vector3 targetDir = rotationDirection;
        targetDir.y = 0;

        if (targetDir == Vector3.zero)
        {
            targetDir = transform.forward;
        }

        Quaternion lookDir = Quaternion.LookRotation(targetDir);
        Quaternion targetRot = Quaternion.Slerp(transform.rotation, lookDir, rotationSpeed);
        transform.rotation = targetRot;
    }
    bool OnAir()
    {
        if (!IsOnGround)
        {
            airTime += Time.deltaTime;
        }
        else
        {
            airTime = 0;
        }
        if (airTime > minAirTime)
        {
            return false;
        }
        else
        {
            return true;

        }
    }
    private void RayCastAiming()
    {
        Debug.DrawLine(transform.position + transform.up * 1.4f, targetLook.position, Color.green);

        distance = Vector3.Distance(transform.position + transform.up * 1.4f, targetLook.position);
        if (distance > 1.3f)
            opportunityToAim = true;
        else opportunityToAim = false;

    }

    bool OnGround()
    {
        return characterController.isGrounded;
    }
    private void CameraChange()
    {
        if (Input.GetAxis("Change") < 0)
        {
            LeftPivotMode = true;

        }
        else if (Input.GetAxis("Change") > 0)
        {
            LeftPivotMode = false;

        }
    }

    private void GravityState()
    {
        moveDirection.y -= Time.deltaTime * gravity;

        characterController.Move(moveDirection * Time.deltaTime);
    }
    void RefreshHealth()
    {
        string Health = "+ " + current_health;
        ui_text.GetComponent<TMP_Text>().text = Health;
    }

    private void CheckHP()
    {
        if (photonView.IsMine)
        {
            if (current_health <= 0)
            {
                manager.Spawn();
                PhotonNetwork.Instantiate(Ragdoll, transform.position, transform.rotation);
                PhotonNetwork.Destroy(gameObject);
            }
            if(current_health >= 100)
            {
                current_health = maxHealth;
            }

        }
    }


    [PunRPC]
    public void TakeDamage(int p_damage)
    {
        if (photonView.IsMine)
        {
            current_health -= p_damage;
            RefreshHealth();
            Debug.Log(current_health);

        }
    }
    [PunRPC]
    public void TakeHealth(int p_health)
    {
        if (photonView.IsMine)
        {
           
            current_health += p_health;
            if (current_health >= 100)
            {
                current_health = maxHealth;
            }
            RefreshHealth();
            Debug.Log(current_health);

        }
    }
}

