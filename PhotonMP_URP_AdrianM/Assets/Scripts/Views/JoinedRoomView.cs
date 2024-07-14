using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JoinedRoomView : MonoBehaviourPunCallbacks, IView
{
    private const string ROOM_NAME_PREFIX = "Room name: ";

    public Action OnLeaveRoom;
    public Action OnPlayInRoom;

    [SerializeField] private Transform _content;
    [SerializeField] private LobbyPlayerEntityView _lobbyPlayerEntityPrefab;
    [SerializeField] private TextMeshProUGUI _roomNameTitle;

    private List<LobbyPlayerEntityView> _lobbyPlayerEntityList = new List<LobbyPlayerEntityView>();

    public void Initialize()
    {
        ShowView();
        SetCurrentRoomTitle();
        ClearLobbyPlayerEntityList();
        GetCurrentRoomPlayers();
    }

    private void SetCurrentRoomTitle()
    {
        _roomNameTitle.text = ROOM_NAME_PREFIX + PhotonNetwork.CurrentRoom.Name;
    }

    public void ShowView()
    {
        gameObject.SetActive(true);
    }

    public void HideView()
    {
        gameObject.SetActive(false);
    }

    private void GetCurrentRoomPlayers()
    {
        foreach (KeyValuePair<int, Player> playerInfo in PhotonNetwork.CurrentRoom.Players)
        {
            AddToLobbyPlayerEntityList(playerInfo.Value);
        }
    }

    private void AddToLobbyPlayerEntityList(Player player)
    {
        LobbyPlayerEntityView newPlayerInRoom = Instantiate(_lobbyPlayerEntityPrefab, _content);
        newPlayerInRoom.GetComponent<LobbyPlayerEntityView>().Setup(player);
        _lobbyPlayerEntityList.Add(newPlayerInRoom);
    }

    public override void OnPlayerEnteredRoom(Player player)
    {
        base.OnPlayerEnteredRoom(player);
        AddToLobbyPlayerEntityList(player);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);

        int index = _lobbyPlayerEntityList.FindIndex(r => r.Player == otherPlayer);
        if (index != -1)
        {
            Destroy(_lobbyPlayerEntityList[index].gameObject);
            _lobbyPlayerEntityList.RemoveAt(index);
        }
    }

    public void OnButtonLeaveRoom()
    {
        PhotonNetwork.LeaveRoom(true);
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        OnLeaveRoom.Invoke();
    }

    private void ClearLobbyPlayerEntityList()
    {
        for (int i = _lobbyPlayerEntityList.Count - 1; i >= 0 ; i--)
        {
            Destroy(_lobbyPlayerEntityList[i].gameObject);
        }

        _lobbyPlayerEntityList.Clear();
    }

    public void OnButtonPlayInRoom()
    {
        OnPlayInRoom.Invoke();
    }
}