using UnityEngine;

namespace Gameplay
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class EnemyMove : MonoBehaviour
    {
        [Header("Parameters")]
        [SerializeField] private float _speed;

        protected Transform _player;
        protected Rigidbody _rigidbody;
        protected Transform _transform;
        private Vector3 _move;

        protected virtual void Start()
        {
            _player = GameObject.FindGameObjectWithTag(Constans.PLAYER_TAG).transform;
            _rigidbody = GetComponent<Rigidbody>();
            _transform = transform;
        }

        protected void MoveForward(float? speed = null) => MoveTo(transform.position + _transform.forward, speed);
        protected void MoveBack(float? speed = null) => MoveTo(transform.position - _transform.forward, speed);
        protected void MoveRight(float? speed = null) => MoveTo(transform.position + _transform.right, speed);
        protected void MoveLeft(float? speed = null) => MoveTo(transform.position - _transform.right, speed);

        protected void MoveTo(Vector3 target, float? speed = null)
        {
            float currentSpeed = speed == null ? _speed : (float)speed;

            _move = Vector3.MoveTowards(_transform.position, target, 1);
            _move -= _transform.position;
            _move *= currentSpeed * Time.deltaTime;
            _rigidbody.AddForce(_move, ForceMode.Force);
        }

        protected void FixedUpdate()
        {
            if (!_player) return;
            Move();
            Rotation();
        }

        protected abstract void Move();
        protected abstract void Rotation();
    }
}