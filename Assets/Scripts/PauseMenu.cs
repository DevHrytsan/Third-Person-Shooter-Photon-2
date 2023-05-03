using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviourPunCallbacks
{
    public static bool paused = false;
    private bool disconnecting = false;

    private void Start()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }
    public void TogglePause()
    {
        if (disconnecting) return;
        paused = !paused;

        transform.GetChild(0).gameObject.SetActive(paused);
        Cursor.lockState = (paused) ? CursorLockMode.None : CursorLockMode.Confined;
        Cursor.visible = paused;
    }
    // Start is called before the first frame update
    public void Quit()
    {
        disconnecting = true;
        paused = false;

        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);

        base.OnLeftRoom();
    }
}
