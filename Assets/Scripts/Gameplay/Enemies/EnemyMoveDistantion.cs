using UnityEngine;

namespace Gameplay
{
    public class EnemyMoveDistantion : EnemyMove
    {
        [Header("Distantion Parameters")]
        [SerializeField] protected float _followDistantion;
        [SerializeField] protected float _retreatDistantion;
        [Space]
        [SerializeField] protected bool _debug;

        protected float _distantion;

        protected override void Move()
        {
            _distantion = Vector3.Distance(_transform.position, _player.position);

            if (_distantion > _followDistantion) MoveTo(_player.position);
            else if (_distantion < _retreatDistantion) MoveBack();
        }

        protected override void Rotation()
        {
            Quaternion unitRot = Quaternion.LookRotation(_player.transform.position - transform.position);
            _rigidbody.MoveRotation(unitRot);
        }

        #region Debug
        protected virtual void OnDrawGizmos()
        {
            if (!_debug) return;

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _followDistantion);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _retreatDistantion);
        }
        #endregion
    }
}