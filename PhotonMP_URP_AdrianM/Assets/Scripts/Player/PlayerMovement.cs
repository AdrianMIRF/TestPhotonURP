using Photon.Pun;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IPunObservable
{
    [SerializeField] private float _walkSpeed = 4f;
    [SerializeField] private float _maxVelocity = 10f;
    [SerializeField] private float _jumpHeight = 5f;

    private Vector2 _inputVector;
    private Rigidbody _rbRef;
    private Transform _transform;

    private bool _isOnGround;
    private bool _isJumping;

    void Awake()
    {
        PhotonNetwork.SendRate = 30;
        PhotonNetwork.SerializationRate = 10;

        _rbRef = GetComponent<Rigidbody>();
        _transform = GetComponent<Transform>();
    }

    //compensate for lag
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_rbRef.position);
            stream.SendNext(_rbRef.rotation);
            stream.SendNext(_rbRef.velocity);
        }
        else
        {
            _rbRef.position = (Vector3)stream.ReceiveNext();
            _rbRef.rotation = (Quaternion)stream.ReceiveNext();
            _rbRef.velocity = (Vector3)stream.ReceiveNext();

            float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
            _rbRef.position += _rbRef.velocity * lag;
        }
    }

    void Update()
    {
        _inputVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        _inputVector.Normalize();

        _isJumping = Input.GetButton("Jump");
        _isOnGround = CheckForGround();
    }

    private bool CheckForGround()
    {
        Ray ray = new Ray(_transform.position, -_transform.up);
        if (Physics.SphereCast(ray.origin, 0.5f, ray.direction, out RaycastHit _, _transform.localScale.y))
        {
            return true;
        }

        return false;
    }

    private void FixedUpdate()
    {
        if (_isOnGround)
        {
            if (_isJumping)
            {
                _rbRef.velocity = new Vector3(_rbRef.velocity.x, _jumpHeight, _rbRef.velocity.z);
            }

            CalculateMovement(_walkSpeed);
        }
    }

    private void CalculateMovement(float _speed)
    {
        Vector3 targetVelocity = new Vector3(_inputVector.x, 0f, _inputVector.y);
        targetVelocity = _transform.TransformDirection(targetVelocity);

        targetVelocity *= _speed;

        if (_inputVector.magnitude > 0.5f)
        {
            Vector3 velocityChange = targetVelocity - _rbRef.velocity;

            velocityChange.x = Mathf.Clamp(velocityChange.x, -_maxVelocity, _maxVelocity);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -_maxVelocity, _maxVelocity);

            velocityChange.y = 0;

            _rbRef.AddForce(velocityChange, ForceMode.VelocityChange);
        }
    }
}