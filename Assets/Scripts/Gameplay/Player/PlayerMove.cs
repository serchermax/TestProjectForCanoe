using UnityEngine;
using Gameplay.Input;

namespace Gameplay
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMove : MonoBehaviour
    {
        [Header("Parameters")]
        [SerializeField] private float _speed;

        private Rigidbody _rigidbody;
        private PlayerMoveInput _playerMoveInput;

        private Vector3 _moveVector;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            _playerMoveInput = PlayerMoveInput.Instance;
            _playerMoveInput.AddListenerOnInput(Move);
        }

        private void OnDestroy() { if (_playerMoveInput) _playerMoveInput.RemoveListenerOnInput(Move); }

        private void Move()
        {
            _moveVector.x = _playerMoveInput.X;
            _moveVector.z = _playerMoveInput.Y;

            _moveVector = _moveVector.magnitude > 1.1f ? _moveVector *= (_moveVector.magnitude / 2) : _moveVector;
            _moveVector *= _speed * Time.deltaTime;

            _rigidbody.AddForce(_moveVector, ForceMode.Force);           
        }
    }
}
    

    
