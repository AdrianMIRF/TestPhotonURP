using Photon.Pun;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private GameObject _playerViewCamera;
    [SerializeField] private GameObject _canvasUIStatus;
    [SerializeField] private HpBarLogic _hpBarLogic;
    [SerializeField] private PlayerHealth _playerHealth;

    [SerializeField] private TextMeshProUGUI _playerCanvasName;

    private bool _isLocalPlayer;
    private string _playerName;

    public void Setup(bool isLocalPlayer)
    {
        _isLocalPlayer = isLocalPlayer;

        _playerMovement.enabled = _isLocalPlayer;
        _playerViewCamera.SetActive(_isLocalPlayer);
        _playerHealth.IsLocalPlayer(_isLocalPlayer);
    }

    [PunRPC]
    public void SetupUIPropreties(string name)
    {
        _playerName = name;
        _playerCanvasName.text = _playerName;

        _canvasUIStatus.SetActive(!_isLocalPlayer);

        SetupHPBar();
    }

    private void SetupHPBar()
    {
        _hpBarLogic.Setup(_playerHealth);
    }
}
