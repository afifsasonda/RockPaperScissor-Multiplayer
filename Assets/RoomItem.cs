using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoomItem : MonoBehaviour
{
    [SerializeField] TMP_Text roomNameText;
    LobbyManager manager;

    public void Set(LobbyManager manager, string roomName)
    {
        this.manager = manager;
        roomNameText.text = roomName;
    }

    //ketika di klik mempunyai fungsi click room
    public void ClickRoomName()
    {
        manager.JoinRoom(roomNameText.text);
    }
}
