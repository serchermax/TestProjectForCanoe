using System;
using UnityEngine;

namespace Gameplay
{
    public class PlayerShield : MonoBehaviour
    {
        [Header("Links")]
        [SerializeField] protected ParticleSystem _shieldEffectPrefab;
        [SerializeField] protected PlayerHealth _playerHealth;

        [Header("Parameters")]
        [SerializeField] private float _startEnergy;
        [SerializeField] private float _regeneration;
        [SerializeField] private float _cooldown;

        private ParticleSystem _shieldEffect;
        private float _cooldownTimer;
        private bool _isShield;

        public event Action OnEnergyChanged;
        public float MaxEnergy { get; private set; }

        public float Energy 
        {
            get { return _energy; }
            private set { _energy = value > MaxEnergy ? MaxEnergy : value; OnEnergyChanged?.Invoke(); }
        }
        private float _energy;

        public float ForceEnergy
        {
            get { return _forceEnergy; }
            set 
            {                
                if (value > _forceEnergy)
                {
                    _cooldownTimer = 0;
                    Energy = MaxEnergy;
                    ShieldOn();
                }
                else if (value <= 0) ShieldOff();
                _forceEnergy = value;
            }
        }
        private float _forceEnergy;

        protected virtual void Start()
        {
            MaxEnergy = _startEnergy;
            Energy = MaxEnergy;
            _shieldEffect = Instantiate(_shieldEffectPrefab, transform);
            _shieldEffect.Stop();
        }

        protected virtual void Update()
        {
            if (ForceEnergy > 0) ForceEnergy -= Time.deltaTime;
            else ShieldCheck();
        }

        public void IncreaseMaxEnergy(float value)
        {
            MaxEnergy += value;
            Energy += value;
        }

        private void ShieldCheck()
        {
            if (!_isShield)
            {
                if (_cooldownTimer < _cooldown) _cooldownTimer += Time.deltaTime;
                else if (Energy < MaxEnergy) Energy += _regeneration * Time.deltaTime;

                if (Energy > 0 && UnityEngine.Input.GetMouseButtonDown(1)) ShieldOn();
            }
            else
            {
                Energy -= Time.deltaTime;
                if (UnityEngine.Input.GetMouseButtonUp(1) || Energy <= 0) ShieldOff();
            }
        }

        private void ShieldOn()
        {
            _cooldownTimer = 0;
            _isShield = true;
            _shieldEffect.Play();
            _playerHealth.IsImmortal = true;
        }
        private void ShieldOff()
        {
            _isShield = false;
            _shieldEffect.Stop();
            _playerHealth.IsImmortal = false;
        }
    }
}