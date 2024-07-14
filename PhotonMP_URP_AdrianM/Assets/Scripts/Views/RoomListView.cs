using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomListView : MonoBehaviourPunCallbacks, IView
{
    public Action OnCreateNewRoom;
    public Action OnJoinedRoomFromRoomListView;

    [SerializeField] private Transform _content;
    [SerializeField] private RoomEntityView _roomEntityPrefab;

    private List<RoomEntityView> _roomEntityList = new List<RoomEntityView>();

    public void Initialize()
    {
        ShowView();
        ClearRoomEntityList();
    }

    public void ShowView()
    {
        gameObject.SetActive(true);
    }

    public void HideView()
    {
        gameObject.SetActive(false);
    }

    public void OnButtonCreateNewRoom()
    {
        OnCreateNewRoom.Invoke();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);

        foreach (RoomInfo room in roomList)
        {
            if (room.RemovedFromList)
            {
                int index = _roomEntityList.FindIndex(r => r.RoomName == room.Name);
                if (index != -1)
                {
                    Destroy(_roomEntityList[index].gameObject);
                    _roomEntityList.RemoveAt(index);
                }
            }
            else
            {
                int index = _roomEntityList.FindIndex(r => r.RoomName == room.Name);
                if (index == -1)
                {
                    RoomEntityView newRoom = Instantiate(_roomEntityPrefab, _content);
                    newRoom.GetComponent<RoomEntityView>().Setup(room.Name, room.PlayerCount.ToString() + " / " + room.MaxPlayers.ToString());
                    _roomEntityList.Add(newRoom);
                }
            }
        }
    }

    private void ClearRoomEntityList()
    {
        for (int i = _roomEntityList.Count - 1; i >= 0; i--)
        {
            MonoBehaviour.DestroyImmediate(_roomEntityList[i].gameObject);
            _roomEntityList.RemoveAt(i);
        }

        _roomEntityList.Clear();
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        OnJoinedRoomFromRoomListView.Invoke();
    }
}