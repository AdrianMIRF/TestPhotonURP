using Photon.Pun;
using TMPro;
using UnityEngine;

public class RoomEntityView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _roomName;
    [SerializeField] private TextMeshProUGUI _roomPlayersCount;

    public string RoomName { get; private set; }

    public void Setup(string roomName, string roomPlayersCount)
    {
        _roomName.text = roomName;
        _roomPlayersCount.text = roomPlayersCount;

        RoomName = roomName;
    }

    public void OnButtonClickJoinRoomByName()
    {
        PhotonNetwork.JoinRoom(RoomName);
    }
}