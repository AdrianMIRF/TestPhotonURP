using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;

public class LeaderBoardView : MonoBehaviour, IView
{
    [SerializeField] private Transform _content;
    [SerializeField] private PlayerScoreEntryView _prefabPlayerScoreEntry;

    private List<PlayerScoreEntryView> _playerScoreEntryView = new List<PlayerScoreEntryView>();
    private bool _enabledStatus;

    public void Toggle()
    {
        _enabledStatus = !_enabledStatus;

        if (_enabledStatus)
        {
            ShowView();
        }
        else
        {
            HideView();
        }
    }

    public void ShowView()
    {
        gameObject.SetActive(true);
        InvokeRepeating(nameof(RefreshLeaderboard), 1f, 1f);
    }

    public void HideView()
    {
        gameObject.SetActive(false);
        CancelInvoke(nameof(RefreshLeaderboard));
    }

    private void RefreshLeaderboard()
    {
        List<Player> sortedPlayerList = (from player in PhotonNetwork.PlayerList orderby player.GetScore() descending select player).ToList();

        for (int i = 0; i < sortedPlayerList.Count; i++)
        {
            int index = _playerScoreEntryView.FindIndex(r => r.PlayerName == sortedPlayerList[i].NickName);
            if (index == -1)
            {
                PlayerScoreEntryView newLeaderBoardEntry = Instantiate(_prefabPlayerScoreEntry, _content);
                newLeaderBoardEntry.Setup(sortedPlayerList[i].NickName, sortedPlayerList[i].GetScore().ToString());

                _playerScoreEntryView.Add(newLeaderBoardEntry);
            }
            else
            {
                _playerScoreEntryView[index].Setup(sortedPlayerList[i].NickName, sortedPlayerList[i].GetScore().ToString());
            }
        }
    }
}
