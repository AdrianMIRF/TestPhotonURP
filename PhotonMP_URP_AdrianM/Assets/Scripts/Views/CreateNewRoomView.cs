using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine;

public class CreateNewRoomView : MonoBehaviourPunCallbacks, IView
{
    private string _newRoomName = "default";
    public Action onCreatedRoom;

    public void Initialize()
    {
        ShowView();
    }

    public void ShowView()
    {
        gameObject.SetActive(true);
    }

    public void HideView()
    {
        gameObject.SetActive(false);
    }

    public void OnRoomNameUpdated(string name)
    {
        _newRoomName = name;
    }

    public void OnButtonCreateNewRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 6;

        PhotonNetwork.JoinOrCreateRoom(_newRoomName, roomOptions, TypedLobby.Default);
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        onCreatedRoom.Invoke();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        Debug.Log(Time.time + " Room creation failed ... "  + message);
    }
}