using UnityEngine;

public class PingPongHover : MonoBehaviour
{
    [SerializeField] private float _height = 0.02f;
    [SerializeField] private float _speed = 0.01f;
    private Transform _transform;

    void Start()
    {
        _transform = GetComponent<Transform>();
    }

    void Update()
    {
        _transform.position = new Vector3(_transform.position.x, _transform.position.y + Mathf.PingPong(Time.time * _speed, _height) - _height / 2f, _transform.position.z);
    }
}