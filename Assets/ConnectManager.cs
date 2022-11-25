using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.SceneManagement;

public class ConnectManager : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_InputField usernameInput;
    [SerializeField] TMP_Text feedbackText;
    public void ClickConnect()
    {
        feedbackText.text = " ";
        if(usernameInput.text.Length < 3)
        {
            feedbackText.text = "Username min 3 characters";
            return;
        }

        //simpan username
        PhotonNetwork.NickName = usernameInput.text;
        PhotonNetwork.AutomaticallySyncScene = true;

        // connect ke server
        PhotonNetwork.ConnectUsingSettings();
        feedbackText.text = "Connecting...";
    }

    //dijalankan setelah connect
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected To Master");
        feedbackText.text = "Connected To Master";
        SceneManager.LoadScene("Lobby");
    }
}
