using UnityEngine;

namespace Gameplay
{
    public abstract class Bonus : PoolObject
    {
        [Header("Links")]
        [SerializeField] private GameObject _bonus;
        [SerializeField] private GameObject _impact;

        [Header("Bonus Settings")]
        [SerializeField] private float _timeAfterImpact;

        private bool _isActivated;

        private void OnEnable()
        {
            _bonus.SetActive(true);
            _impact.SetActive(false);
            _isActivated = false;
        }

        private void OnTriggerEnter(Collider other)
        {            
            if (!_isActivated && other.CompareTag(Constans.PLAYER_TAG))
            {
                BonusEffect(other);
                Impact();
            }
        }

        protected abstract void BonusEffect(Collider player);

        private void Impact()
        {
            _isActivated = true;
            _bonus.SetActive(false);
            if (_impact) _impact.SetActive(true);
            BackToPool(_timeAfterImpact);
        }
    }
}