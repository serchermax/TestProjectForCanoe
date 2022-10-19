using UnityEngine;

namespace Gameplay
{
    [RequireComponent(typeof(Rigidbody))]
    public class Bullet : PoolObject
    {
        [Header("Required Links")]
        [SerializeField] private ParticleSystem _bullet;
        [SerializeField] private ParticleSystem _impact;

        [Header("Settings")]
        [SerializeField] private float _timeBackToPoolAfterImpact = 1f;
        [SerializeField] private float _checkRadius = 0.5f;

        private Rigidbody _rigidbody;
        private DecalsContainer _decalsContainer;
        private float _speed;
        private float _damage;
        private int _targetLayer;
        private float _lifeTime;

        private bool _shot;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Start()
        {            
            _decalsContainer = DecalsContainer.Instance; // Singleton for optimisation
        }

        public void Initilize(float speed, float damage, float lifeTime, int targetLayer)
        {
            _speed = speed;
            _damage = damage;
            _targetLayer = targetLayer;
            _lifeTime = lifeTime;

            _bullet.gameObject.SetActive(true);
            _impact.gameObject.SetActive(false);
            _shot = false;

            BackToPool(_lifeTime);
        }

     
        private void FixedUpdate()
        {
            if (!_shot)
            {
                _rigidbody.velocity = transform.forward * _speed * Time.deltaTime;
                CheckTarget();
            }           
        }

        private void CheckTarget()
        {
            if (Physics.CheckSphere(transform.position, _checkRadius, 1 << _targetLayer)) // Optimize way to find Target
            {
                Vector3 stepBack = transform.position - (transform.forward * 0.01f);
                if (Physics.Raycast(stepBack, (transform.position - stepBack).normalized, out RaycastHit targetHit, 5f, 1 << _targetLayer)) // The best way for big speed to GET target
                {
                    if (Physics.Raycast(stepBack, (transform.position - stepBack).normalized, out RaycastHit hit, 5f, 1 << Constans.COLLISION_FOR_DECALS)) // Ray for decal point finding
                    {
                        transform.position = hit.point + hit.normal * 0.01f;
                        if (targetHit.transform.gameObject.activeInHierarchy) MakeDecal(targetHit.collider, hit);
                    }
                    else transform.position = targetHit.point + targetHit.normal * 0.01f;

                    Impact();
                    Damage(targetHit.collider);
                    _shot = true;
                    _rigidbody.velocity = Vector3.zero;
                }
            }           
        }

        private void MakeDecal(Collider enemy, RaycastHit hit)
        {
            if (enemy.TryGetComponent(out IDestroyable destroyable))
                _decalsContainer.SetDecal(hit, enemy.transform, destroyable);
            else _decalsContainer.SetDecal(hit);
        }

        private void Damage(Collider col)
        {         
            if (col.TryGetComponent(out IHealth health))
            {
                health.Damage(_damage);                   
            }           
        }

        private void Impact()
        {
            StopTimer();

            _bullet.gameObject.SetActive(false);
            _impact.gameObject.SetActive(true);         
            BackToPool(_timeBackToPoolAfterImpact);
        }
    }
}