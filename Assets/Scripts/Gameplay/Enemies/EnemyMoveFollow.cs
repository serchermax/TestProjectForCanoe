using UnityEngine;

namespace Gameplay
{
    public class EnemyMoveFollow : EnemyMove
    {
        protected override void Move() => MoveTo(_player.position);

        protected override void Rotation()
        {
            Quaternion unitRot = Quaternion.LookRotation(_player.transform.position - transform.position);
            _rigidbody.MoveRotation(unitRot);
        }
    }
}