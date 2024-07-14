using Photon.Pun;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Camera _playerCamera;
    [SerializeField] private float _fireRate = 0.5f;
    [SerializeField] private PhotonView _photonView;
    [SerializeField] private GameObject _playerBullet;

    private float _currentTimerForFireRate;
    private bool _isLocalPlayer;

    void Start()
    {
        _currentTimerForFireRate = _fireRate;
    }

    public void Setup(bool isLocalPlayer)
    {
        _isLocalPlayer = isLocalPlayer;
    }

    void Update()
    {
        if (!_isLocalPlayer)
        {
            return;
        }

        if (_currentTimerForFireRate > 0f)
        {
            _currentTimerForFireRate -= Time.deltaTime;
        }

        if (Input.GetButton("Fire1") && _currentTimerForFireRate <= 0)
        {
            _currentTimerForFireRate = _fireRate;
            _photonView.RPC("FireWeapon", RpcTarget.AllBufferedViaServer, _playerCamera.transform.position, _playerCamera.transform.forward, _playerCamera.transform.rotation);
        }
    }

    [PunRPC]
    public void FireWeapon(Vector3 pos, Vector3 dir, Quaternion rot)
    {
        //GameObject bullet = Instantiate(_playerBullet, pos + dir * 2f, rot);

        //Dev comment, setting transform parent is expensive, use it only in editor to avoid clutter
        GameObject bullet = _playerBullet.Spawn(ObjectPool.Instance.transform, pos + dir * 2f, rot);
        bullet.GetComponent<Bullet>().SetupPlayerBullet(_photonView.Owner, dir);
    }
}