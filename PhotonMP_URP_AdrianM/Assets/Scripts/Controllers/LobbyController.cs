using System;
using UnityEngine;

public class LobbyController : MonoBehaviour
{
    public Action PlayInRoom;

    [SerializeField] private GameObject _panelChatAndRoomList;
    [SerializeField] private RoomListView _roomListView;
    [SerializeField] private JoinedRoomView _joinedRoomView;
    [SerializeField] private ChatView _chatView;
    [SerializeField] private CreateNewRoomView _createNewRoomView;

    public void Initialize(string _playerName)
    {
        gameObject.SetActive(true);
        _panelChatAndRoomList.SetActive(true);

        _roomListView.Initialize();
        _chatView.Initialize(_playerName);

        _roomListView.OnCreateNewRoom += OnButtonCreateNewRoom;
        _roomListView.OnJoinedRoomFromRoomListView += OnJoinedRoom;
        _createNewRoomView.onCreatedRoom += OnJoinedRoom;
        _joinedRoomView.OnLeaveRoom += OnLeaveRoom;
        _joinedRoomView.OnPlayInRoom += OnPlayInRoom;
    }

    public void CloseThis()
    {
        gameObject.SetActive(false);
        _panelChatAndRoomList.SetActive(false);
        _createNewRoomView.HideView();
        _roomListView.HideView();
        _chatView.HideView();
        _joinedRoomView.HideView();

        _roomListView.OnCreateNewRoom -= OnButtonCreateNewRoom;
        _roomListView.OnJoinedRoomFromRoomListView -= OnJoinedRoom;
        _createNewRoomView.onCreatedRoom -= OnJoinedRoom;
        _joinedRoomView.OnLeaveRoom -= OnLeaveRoom;
        _joinedRoomView.OnPlayInRoom -= OnPlayInRoom;
    }

    private void OnButtonCreateNewRoom()
    {
        _panelChatAndRoomList.SetActive(false);
        _roomListView.HideView();
        _chatView.HideView();

        _createNewRoomView.ShowView();
    }

    private void OnJoinedRoom()
    {
        _panelChatAndRoomList.SetActive(true);
        _createNewRoomView.HideView();
        _roomListView.HideView();
        _chatView.ShowView();
        _joinedRoomView.Initialize();
    }

    private void OnLeaveRoom()
    {
        CloseThis();
    }

    private void OnPlayInRoom()
    {
        PlayInRoom.Invoke();
        CloseThis();
    }
}