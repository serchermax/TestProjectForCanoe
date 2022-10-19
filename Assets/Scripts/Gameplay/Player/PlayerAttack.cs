using UnityEngine;

namespace Gameplay
{
    public class PlayerAttack : AttackGun
    {
        [Space]
        [SerializeField] private Transform _poolsContainer;
        [SerializeField] private int _startPoolBulletCount;

        private PoolMono<Bullet> _bulletPool;

        protected override Bullet nextBullet => _bulletPool.GetObjectFromPool(true);
        protected override int targetLayer => Constans.ENEMIES_LAYER;

        protected override void Start()
        {
            base.Start();
            _bulletPool = new PoolMono<Bullet>(_bulletPrefab, _startPoolBulletCount, 
                _poolsContainer == null ? GameObject.FindGameObjectWithTag(Constans.POOLS_CONTAINER_TAG).transform : _poolsContainer, Constans.PLAYER_BULLETS_LAYER);          
        }

        protected override void Update()
        {
            base.Update();
            if (UnityEngine.Input.GetMouseButton(0)) Shot();            
        }
    }
}
