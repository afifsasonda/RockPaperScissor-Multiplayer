using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_InputField newRoomInputField;
    [SerializeField] TMP_Text feedbackText;
    [SerializeField] Button StartGameButton;
    [SerializeField] GameObject roomPanel;
    [SerializeField] TMP_Text roomNameText;
    [SerializeField] GameObject RoomListObject;
    [SerializeField] GameObject playerListObject;
    [SerializeField] RoomItem roomItemPrefab;
    [SerializeField] PlayerItem playerItemPrefab;

    //menyimpan nama2 room yg ada/ list dari room item pada game
    List<RoomItem> roomItemList = new List<RoomItem>();

    List<PlayerItem> playerItemList = new List<PlayerItem>();

    private void Start() {
        PhotonNetwork.JoinLobby();
        roomPanel.SetActive(false);
    }

    public void ClickCreateRoom()
    {
        feedbackText.text = "";
        if(newRoomInputField.text.Length < 3)
        {
            feedbackText.text = "Room Name min 3 characters";
            return;
        }

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 5;
        PhotonNetwork.CreateRoom(newRoomInputField.text, roomOptions);
    }

    public void ClickStartGame(string levelName)
    {
        if(PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(levelName);
        }
    }

    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    //callback
    public override void OnCreatedRoom()
    {
        Debug.Log("Created room : " + PhotonNetwork.CurrentRoom.Name);
        feedbackText.text = "Created room: " + PhotonNetwork.CurrentRoom.Name;
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room : " + PhotonNetwork.CurrentRoom.Name);
        feedbackText.text = "Create room: " + PhotonNetwork.CurrentRoom.Name;

        //menampilkan name room
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;

        //set aktif room panel
        roomPanel.SetActive(true);

        //update Player list
        UpdatePlayerList();

        //atur start game button
        SetStartGameButton();
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        //update Player list
        UpdatePlayerList();
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        //update Player list
        UpdatePlayerList();
    }

    public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
    {
        SetStartGameButton();
    }

    private void SetStartGameButton()
    {
        //tampilkan hanya di master client
        StartGameButton.gameObject.SetActive(PhotonNetwork.IsMasterClient);

        // bisa diklik/ interectable hanya ketika jumlah player >= 2
        StartGameButton.interactable = PhotonNetwork.CurrentRoom.PlayerCount >= 2;
    }

    private void UpdatePlayerList()
    {
        foreach (var item in playerItemList)
        {
            //destroy dulu semua player item yang sudah ada
            Destroy(item.gameObject);
        }

        playerItemList.Clear();
        //PhotonNetwork.PlayerList (alternative)
        //PhotonNetwork.CurrentRoom.Players

        //foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
        
        //bikin ulang player list
        foreach (var(id,player) in PhotonNetwork.CurrentRoom.Players)
        {
            PlayerItem newPlayerItem = Instantiate(playerItemPrefab, playerListObject.transform);
            newPlayerItem.Set(player);
            playerItemList.Add(newPlayerItem);

            if(player == PhotonNetwork.LocalPlayer)
            {
                newPlayerItem.transform.SetAsFirstSibling();
            }
        }

        //start game hanya bisa diklik ketika jumlah pemain tertentu
        //jadi atur disini
        SetStartGameButton();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
            Debug.Log(returnCode+ ","+ message);
            feedbackText.text = returnCode.ToString() +": " +message;
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (var item in this.roomItemList)
        {
            Destroy(item.gameObject);
        }

        this.roomItemList.Clear();

        foreach (var roomInfo in roomList)
        {
            RoomItem newRoomItem = Instantiate(roomItemPrefab,RoomListObject.transform);
            newRoomItem.Set(this,roomInfo.Name);
            this.roomItemList.Add(newRoomItem);
        }
    }

}
