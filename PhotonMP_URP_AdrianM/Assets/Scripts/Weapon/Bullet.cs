using Photon.Realtime;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed = 200f;
    [SerializeField] private float _damage = 10f;
    public Player Owner { get; private set; }

    public float GetDamage()
    {
        return _damage;
    }

    public void SetupPlayerBullet(Player owner, Vector3 direction)
    {
        Owner = owner;

        transform.forward = direction;

        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.velocity = direction * _speed;
    }
}
