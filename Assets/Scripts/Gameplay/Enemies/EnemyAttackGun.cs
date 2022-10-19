using UnityEngine;

namespace Gameplay
{
    public class EnemyAttackGun : AttackGun
    {
        [Header("Links")]
        [SerializeField] private Transform _poolsContainer;

        [Header("Enemy Parameters")]
        [SerializeField] private float _rayLength;
        [SerializeField] private int _startPoolBulletCount;

        protected override Bullet nextBullet => _bulletPool.GetObjectFromPool();
        protected override int targetLayer => Constans.PLAYER_LAYER;

        private PoolMono<Bullet> _bulletPool;
        private RaycastHit _hit;

        protected override void Start()
        {
            base.Start();
            _bulletPool = new PoolMono<Bullet>(_bulletPrefab, _startPoolBulletCount,
                _poolsContainer == null ? GameObject.FindGameObjectWithTag(Constans.POOLS_CONTAINER_TAG).transform : _poolsContainer);
        }

        protected override void Update()
        {
            base.Update();
            CheckTarget();
        }

        private void CheckTarget()
        {          
            if (Physics.Raycast(_attackPoint.position, _attackPoint.forward, out _hit, _rayLength, 1 << Constans.PLAYER_LAYER))
                Shot();
        }
    }
}