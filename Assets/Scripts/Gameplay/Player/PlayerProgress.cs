using System;
using UnityEngine;

namespace Gameplay
{ 
    public class PlayerProgress : MonoBehaviour
    {
        [Header("Links")]
        [SerializeField] private PlayerHealth _playerHealth;
        [SerializeField] private PlayerAttack _playerAttack;
        [SerializeField] private PlayerShield _playerShield;
        [SerializeField] private ParticleSystem _levelUpEffectPrefab;

        [Header("Progress Settings")]
        [SerializeField] private int[] _expChart;
        [Space]
        [SerializeField] private float _healthPerLevel;
        [SerializeField] private float _damagePerLevel;
        [SerializeField] private float _energyPerLevel;

        public event Action OnExpChanged;
        public event Action OnLevelChanged;

        public int Exp { get; private set; }
        public int ExpForNextLevel => _expChart.Length > Level ? _expChart[Level] : _expChart[_expChart.Length - 1];
        public int Level { get; private set; }

        private ParticleSystem _levelUpEffect;

        private void Start()
        {
            Exp = 0;
            Level = 0;
            _playerHealth = _playerHealth != null ? _playerHealth : FindObjectOfType<PlayerHealth>();
            _playerAttack = _playerAttack != null ? _playerAttack : FindObjectOfType<PlayerAttack>();
            _playerShield = _playerShield != null ? _playerShield : FindObjectOfType<PlayerShield>();
            _levelUpEffect = Instantiate(_levelUpEffectPrefab, transform);
            _levelUpEffect.Stop();
        }

        public void SetExp(int exp)
        {
            Exp += exp;
            if (_expChart.Length > Level && Exp >= _expChart[Level]) LevelUp();
            OnExpChanged?.Invoke();
        }

        private void LevelUp()
        {
            _levelUpEffect.Play();
            _playerAttack.Damage += _damagePerLevel;
            _playerHealth.IncreaseMaxHealth(_healthPerLevel);
            _playerShield.IncreaseMaxEnergy(_energyPerLevel);
            Exp -= _expChart[Level];
            Level++;
            _playerHealth.Heal(_playerHealth.MaxHealth);
            OnLevelChanged?.Invoke();
        }
    }
}