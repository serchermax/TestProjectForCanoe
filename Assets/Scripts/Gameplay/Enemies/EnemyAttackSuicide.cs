using UnityEngine;

namespace Gameplay
{
    [RequireComponent(typeof(EnemyCore))]
    public class EnemyAttackSuicide : MonoBehaviour
    {
        [Header("Sucide Parameters")]
        [SerializeField] private float _suicideDamage;

        private EnemyCore _enemyCore;
        private bool _damaged = false;

        private void OnEnable() => _damaged = false;

        private void Start()
        {
            _enemyCore = GetComponent<EnemyCore>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (_damaged) return;

            if (collision.transform.CompareTag(Constans.PLAYER_TAG))
            {
                _enemyCore.Damage(_enemyCore.MaxHealth);

                if (collision.transform.TryGetComponent(out IHealth health))
                    health.Damage(_suicideDamage);

                _damaged = true;
            }
        }
    }
}