using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class Launcher : MonoBehaviourPunCallbacks
{
    public InputField usernameField;
    public static string username;
    // Start is called before the first frame update

    public void ConnectionToLobby()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        Connect(); 
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected!");
        Join();
        base.OnConnectedToMaster();
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Create();
        base.OnJoinRandomFailed(returnCode, message);
    }
    public override void OnJoinedRoom()
    {
        StartGame();
        base.OnJoinedRoom();
    }
    public void Connect()
    {
        PhotonNetwork.GameVersion = "0.0.0";
        PhotonNetwork.ConnectUsingSettings();
    }
    public void Join()
    {
        PhotonNetwork.JoinRandomRoom();
    }
    public void Create()
    {
        PhotonNetwork.CreateRoom("Room");
    }
    public void StartGame()
    {
        PhotonNetwork.LocalPlayer.NickName = usernameField.text;
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            PhotonNetwork.LoadLevel(1);
        }
    }
   public void ConnectToRandomLobby()
    {
        Join();
    }
    private void OnApplicationQuit()
    {
        PhotonNetwork.Disconnect();

    }
}
