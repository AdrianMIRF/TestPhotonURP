using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public Action OnPlayerDead;
    [SerializeField] private PhotonView _photonView;
    [SerializeField] private float _maxHealth = 100f;

    private float _currentHealth;
    private bool _isLocalPlayer;

    public float GetCurrentHealth()
    {
        return _currentHealth;
    }

    public float GetMaxHealth()
    {
        return _maxHealth;
    }

    public void IsLocalPlayer(bool value)
    {
        _isLocalPlayer = value;
    }

    [PunRPC]
    public void TakeDamage(float value, Player player)
    {
        _currentHealth -= value;
        CheckForStatus(player);
    }

    private void CheckForStatus(Player player)
    {
        if (_currentHealth <= 0f)
        {
            if (_isLocalPlayer)
            {
                PhotonNetwork.Destroy(gameObject);
                OnPlayerDead.Invoke();

                AwardOponentScore(player);
            }
        }
    }

    private void AwardOponentScore(Player player)
    {
        List<Player> playerList = (PhotonNetwork.PlayerList).ToList();
        int index = playerList.FindIndex(r => r == player);
        if (index != -1)
        {
            playerList[index].AddScore(1);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Projectiles"))
        {
            if (other.GetComponent<Bullet>().Owner != _photonView.Owner)
            {
                _photonView.RPC("TakeDamage", RpcTarget.AllBufferedViaServer, other.GetComponent<Bullet>().GetDamage(), other.GetComponent<Bullet>().Owner);
            }

            //Destroy(other.gameObject);
            other.gameObject.Recycle();
        }
    }

    private void OnEnable()
    {
        _currentHealth = _maxHealth;
    }
}