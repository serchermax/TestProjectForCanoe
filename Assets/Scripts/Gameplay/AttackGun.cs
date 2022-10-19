using UnityEngine;

namespace Gameplay
{
    public abstract class AttackGun : MonoBehaviour
    {
        [Header("Links")]
        [SerializeField] protected Transform _attackPoint;
        [SerializeField] protected Bullet _bulletPrefab;
        [SerializeField] protected ParticleSystem _shotEffectPrefab;

        [Header("Parameters")]
        [SerializeField] private float _startDamage;
        [SerializeField] private float _bulletSpeed;
        [SerializeField] private float _bulletLifeTime;
        [SerializeField] private float _fireRate;

        protected abstract Bullet nextBullet { get; }
        protected abstract int targetLayer { get; }

        private ParticleSystem _shotEffect;
        private float _timer;

        public float Damage
        {
            get { return _damage; }
            set { _damage = value < 0 ? 0 : value; }
        }
        private float _damage;

        protected virtual void Start()
        {
            Damage = _startDamage;
            _shotEffect = Instantiate(_shotEffectPrefab, _attackPoint);
        }

        protected virtual void Update()
        {
            if (_timer < _fireRate) _timer += Time.deltaTime;
        }

        protected virtual void Shot()
        {
            if (_timer < _fireRate) return;
            else _timer = 0;

            _shotEffect.Play();
            Bullet bullet = nextBullet;
            bullet.transform.SetPositionAndRotation(_attackPoint.position, _attackPoint.rotation);
            bullet.Initilize(_bulletSpeed, Damage, _bulletLifeTime, targetLayer);
        }
    }
}