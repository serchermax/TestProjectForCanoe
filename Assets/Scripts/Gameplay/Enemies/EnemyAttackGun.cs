using UnityEngine;

namespace Gameplay
{
    [RequireComponent(typeof(IDestroyable))]
    public class EnemyAttackGun : AttackGun
    {
        [Header("Links")]
        [SerializeField] private Transform _poolsContainer;

        [Header("Enemy Parameters")]
        [SerializeField] private float _rayLength;
        [SerializeField] private int _startPoolBulletCount;

        protected override Bullet nextBullet => _bulletPool.GetObjectFromPool();
        protected override int targetLayer => Constans.PLAYER_LAYER;

        private IDestroyable _destroyable;
        private PoolMono<Bullet> _bulletPool;

        private bool _canShoot;

        protected override void Start()
        {
            base.Start();
            _bulletPool = new PoolMono<Bullet>(_bulletPrefab, _startPoolBulletCount,
                _poolsContainer == null ? GameObject.FindGameObjectWithTag(Constans.POOLS_CONTAINER_TAG).transform : _poolsContainer);

            _destroyable = GetComponent<IDestroyable>();
            _destroyable.OnDestroy += WhenDestroy;
        }   

        private void OnDestroy()
        {
            if (_destroyable != null) _destroyable.OnDestroy -= WhenDestroy;
        }

        private void OnEnable() => _canShoot = true;
        private void WhenDestroy() => _canShoot = false;

        protected override void Update()
        {
            base.Update();
            CheckTarget();
        }

        private void CheckTarget()
        {
            if (!_canShoot) return;

            if (Physics.Raycast(_attackPoint.position, _attackPoint.forward, _rayLength, 1 << Constans.PLAYER_LAYER))
                Shot();
        }
    }
}