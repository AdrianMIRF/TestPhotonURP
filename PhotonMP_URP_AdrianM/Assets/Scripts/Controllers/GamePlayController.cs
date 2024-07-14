using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayController : MonoBehaviour
{
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private Transform _spawnPoint;

    [SerializeField] private GamePlayView _gamePlayView;
    [SerializeField] private LeaderBoardView _leaderBoardView;

    private bool _playerInGamePlayRoom;
    private string _playerName;

    public void PlayInRoom(string playerName)
    {
        _playerInGamePlayRoom = true;
        _playerName = playerName;
        _gamePlayView.ShowView();
        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        GameObject player = PhotonNetwork.Instantiate(_playerPrefab.name, _spawnPoint.position, Quaternion.identity);
        player.GetComponent<PlayerController>().Setup(true);

        player.GetComponent<Weapon>().Setup(true);
        player.GetComponent<PhotonView>().RPC("SetupUIPropreties", RpcTarget.AllBuffered, _playerName);
        player.GetComponent<PlayerHealth>().OnPlayerDead += SpawnPlayer;
        PhotonNetwork.LocalPlayer.NickName = _playerName;
    }

    private void Update()
    {
        if (!_playerInGamePlayRoom)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            _leaderBoardView.Toggle();
        }
    }
}