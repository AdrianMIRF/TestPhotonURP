using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GamePlayController : MonoBehaviour
{
    [SerializeField] private int _winMapScore = 5;

    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private Transform _spawnPoint;

    [SerializeField] private GamePlayView _gamePlayView;
    [SerializeField] private LeaderBoardView _leaderBoardView;

    private bool _playerInGamePlayRoom;
    private string _playerName;
    private GameObject _player;

    public void PlayInRoom(string playerName)
    {
        _playerInGamePlayRoom = true;
        _playerName = playerName;
        _gamePlayView.ShowView();
        SpawnPlayer();

        InvokeRepeating(nameof(CheckForPlayersScore), 1f, 1f);
    }

    public void SpawnPlayer()
    {
        if (!_playerInGamePlayRoom)
        {
            return;
        }

        _player = PhotonNetwork.Instantiate(_playerPrefab.name, _spawnPoint.position, Quaternion.identity);
        _player.GetComponent<PlayerController>().Setup(true);

        _player.GetComponent<Weapon>().Setup(true);
        _player.GetComponent<PhotonView>().RPC("SetupUIPropreties", RpcTarget.AllBuffered, _playerName);
        _player.GetComponent<PlayerHealth>().OnPlayerDead += SpawnPlayer;
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

    private void CheckForPlayersScore()
    {
        List<Player> playerList = (PhotonNetwork.PlayerList).ToList();
        for (int i = 0; i < playerList.Count; i++)
        {
            if (playerList[i] == PhotonNetwork.LocalPlayer)
            {
                _gamePlayView.UpdateLocalPlayerUIScore(playerList[i].GetScore(), _winMapScore);
            }

            if (playerList[i].GetScore() >= _winMapScore)
            {
                MapWinByPlayer(playerList[i]);
            }
        }
    }

    private void MapWinByPlayer(Player player)
    {
        CancelInvoke(nameof(CheckForPlayersScore));
        _gamePlayView.ShowMapWin(player, _winMapScore);
        _playerInGamePlayRoom = false;
		PhotonNetwork.LocalPlayer.SetScore(0);
        PhotonNetwork.Destroy(_player);
    }
}