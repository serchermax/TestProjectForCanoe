using System;
using System.Collections;
using UnityEngine;

namespace Gameplay
{
    public class PlayerHealth : MonoBehaviour, IHealth, IDestroyable
    {
        public event Action OnHealthChanged;
        public event Action OnDestroy;

        public bool IsImmortal { get; set; }
        public float MaxHealth { get; private set; }
        public float Health { get; private set; }


        [Header("Links")]
        [SerializeField] private GameObject _deathImpact;

        [Header("Parameters")]
        [SerializeField] private float _health;

        private void Start()
        {
            MaxHealth = _health;
            Health = MaxHealth;
            OnHealthChanged?.Invoke();
        }

        public void Damage(float value)
        {
            if (IsImmortal) return;

            Health -= value;
            OnHealthChanged?.Invoke();
            if (Health <= 0) Die();
        }

        public void Heal(float value)
        {
            Health = Health + value > MaxHealth ? MaxHealth : Health + value;
            OnHealthChanged?.Invoke();
        }

        public void IncreaseMaxHealth(float value)
        {
            MaxHealth += value;
            Heal(value);
            OnHealthChanged?.Invoke();
        }

        private void Die()
        {
            OnDestroy?.Invoke();
            if (_deathImpact != null)
            {
                GameObject temp = Instantiate(_deathImpact);
                temp.transform.position = transform.position;
                Destroy(temp, 3f);
            }
            StartCoroutine(Destroy());
        }

        private IEnumerator Destroy()
        {
            yield return new WaitForSeconds(0.01f);
            gameObject.SetActive(false);
        }
    }
}