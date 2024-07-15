using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class GamePlayView : MonoBehaviour, IView
{
    private const string WIN_MAP_CONDITION_STATUS = "Win map target score: ";
    private const string PLAYER_SCORE = "Player score: ";
    private const string MAP_WON_BY_PLAYER = "Map won by player :";

    [SerializeField] private GameObject _previewRoomCamera;
    [SerializeField] private GameObject _mapWonPanel;
    
    [SerializeField] private TextMeshProUGUI _mapWonInfoText;
    [SerializeField] private TextMeshProUGUI _winMapConditionStatusInfoText;

    public void ShowView()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        gameObject.SetActive(true);
    }

    public void HideView()
    {
        gameObject.SetActive(false);
    }

    public void ShowMapWin(Player player, int _winMapScore)
    {
        _previewRoomCamera.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        _mapWonPanel.SetActive(true);
        _mapWonInfoText.text = MAP_WON_BY_PLAYER + player.NickName + "\n" + WIN_MAP_CONDITION_STATUS + _winMapScore;
    }

    public void UpdateLocalPlayerUIScore(int localPlayerScore, int _winMapScore)
    {
        _winMapConditionStatusInfoText.text = WIN_MAP_CONDITION_STATUS + _winMapScore + "\n" + PLAYER_SCORE + localPlayerScore;
    }

    public void OnButtonReturnToLobby()
    {
        Debug.Log(Time.time + " Map won, OnButtonReturnToLobby()");
        PhotonNetwork.LeaveRoom(true);
        HideView();
    }
}