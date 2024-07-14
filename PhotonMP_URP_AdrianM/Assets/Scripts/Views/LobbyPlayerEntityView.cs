using Photon.Realtime;
using TMPro;
using UnityEngine;

public class LobbyPlayerEntityView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _playerLobbyName;

    public Player Player { get; private set; }

    public void Setup(Player player)
    {
        Player = player;
        _playerLobbyName.text = Player.NickName;
    }
}