using UnityEngine;

namespace Gameplay
{
    public class EnemyMoveDodge : EnemyMoveDistantion
    {
        [Header("Dodge Parameters")]
        [SerializeField] protected Vector3 _visionZone;
        [SerializeField] protected bool _useOriginalSpeed;
        [SerializeField] protected float _dodgeSpeed;

        private Vector3 _vision;
        private DodgeTo _dodgeTo;

        private enum DodgeTo
        {
            No = 0,
            Right = 1,
            Left = 2
        }

        private void Update()
        {
            _vision = transform.position + transform.forward * (_visionZone.z / 2);

            Collider[] colliders = Physics.OverlapBox(_vision, _visionZone / 2f, _transform.rotation, 1 << Constans.PLAYER_BULLETS_LAYER);
            if (colliders.Length > 0)
            {
                Vector3 direction = transform.InverseTransformPoint(colliders[0].transform.position).normalized;
                float rightDot = Vector3.Dot(direction, Vector3.right);
                _dodgeTo = rightDot < 0f ? DodgeTo.Right : DodgeTo.Left;
            }
            else _dodgeTo = DodgeTo.No;

            DebugUpdate();
        }

        protected override void Move()
        {
            base.Move();
            switch (_dodgeTo)
            {
                case DodgeTo.Right: MoveRight(_useOriginalSpeed ? null : (float?)_dodgeSpeed); break;
                case DodgeTo.Left: MoveLeft(_useOriginalSpeed ? null : (float?)_dodgeSpeed); break;
                default: break;
            }
        }

        #region Debug
        private Transform primitive;

        private void DebugUpdate()
        {
            if (_debug)
            {
                if (!primitive) CreateDebug();
                primitive.position = _vision;
                primitive.localScale = _visionZone;
                primitive.rotation = _transform.rotation;
            }
            else if (primitive) Destroy(primitive.gameObject);

            void CreateDebug()
            {
                primitive = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
                primitive.GetComponent<Collider>().enabled = false;
            }
        }
        #endregion
    }
}