using Photon.Pun;
using System;
using System.Collections;
using UnityEngine;

public class MasterController : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject _previewRoomCamera;

    [SerializeField] private PlayerSetNameView _playerSetNameView;
    [SerializeField] private ConnectingToServerView _connectingToServerView;

    [SerializeField] private LobbyController _lobbyController;
    [SerializeField] private GamePlayController _gamePlayController;

    private string _playerName;

    private void Start()
    {
        _playerSetNameView.Initialize();
        _playerSetNameView.onNameChosen += NameChosen;

        _lobbyController.PlayInRoom += PlayInRoom;
    }

    private void NameChosen(string playerName)
    {
        _playerSetNameView.onNameChosen -= NameChosen;
        _playerName = playerName;
        PhotonNetwork.LocalPlayer.NickName = _playerName;

        _connectingToServerView.Initialize();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }
        else
        {
            _lobbyController.Initialize(_playerName);
        }

        Debug.Log(Time.time + " Connected To Master...");
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        _previewRoomCamera.SetActive(true);
        _lobbyController.Initialize(_playerName);
    }

    private void PlayInRoom()
    {
        _previewRoomCamera.SetActive(false);
        _gamePlayController.PlayInRoom(_playerName);
    }
}