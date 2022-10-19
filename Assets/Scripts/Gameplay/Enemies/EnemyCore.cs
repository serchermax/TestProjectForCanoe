using System;
using UnityEngine;

namespace Gameplay
{
    [RequireComponent(typeof(Collider))]
    public class EnemyCore : PoolObject, IHealth, IDestroyable
    {
        public event Action OnHealthChanged;
        public event Action OnDestroy;

        public float MaxHealth { get; private set; }
        public float Health { get; private set; }

        [Header("Links")]
        [SerializeField] private GameObject _mainObject;
        [SerializeField] private GameObject _deathImpactPrefab;

        [Header("Parameters")]
        [SerializeField] private float _health;
        [SerializeField] private float _timeBackToPoolAfterImpact = 1f;

        private Collider _collider;
        private GameObject _deathImpact;
        private bool _died;

        private void OnEnable()
        {
            _mainObject.SetActive(true);
            if (_deathImpact) _deathImpact.SetActive(false);
            _collider.enabled = true;

            MaxHealth = _health;
            Health = MaxHealth;
            _died = false;
        }

        private void Awake()
        {
            _collider = GetComponent<Collider>();
            _deathImpact = Instantiate(_deathImpactPrefab, transform);
            _deathImpact.SetActive(false);
        }

        public void Damage(float value)
        {
            if (_died) return;

            Health -= value;
            OnHealthChanged?.Invoke();
            if (Health <= 0) Die();
        }

        public void Heal(float value)
        {
            Health = Health + value > MaxHealth ? MaxHealth : Health + value;
            OnHealthChanged?.Invoke();
        }

        private void Die()
        {
            OnDestroy?.Invoke();
            _died = true;
            _mainObject.SetActive(false);
            if (_deathImpact) _deathImpact.SetActive(true);
            _collider.enabled = false;
            BackToPool(_timeBackToPoolAfterImpact);
        }
    }
}
