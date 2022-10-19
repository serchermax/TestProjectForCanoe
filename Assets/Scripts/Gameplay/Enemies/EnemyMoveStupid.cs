using UnityEngine;

namespace Gameplay
{
    public class EnemyMoveStupid : EnemyMove
    {
        [Header("Stupid Parameters")]
        [SerializeField] private float _rotateSpeed;

        protected override void Move() => MoveForward();

        protected override void Rotation()
        {
            Quaternion unitRot = Quaternion.LookRotation(_player.transform.position - transform.position);
            _rigidbody.MoveRotation(Quaternion.Lerp(transform.rotation, unitRot, Time.deltaTime * _rotateSpeed));
        }
    }
}