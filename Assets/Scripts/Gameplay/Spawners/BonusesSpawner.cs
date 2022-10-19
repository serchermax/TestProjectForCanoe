using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class BonusesSpawner : MonoBehaviour
    {
        [Header("Spawner Settings")]
        [SerializeField] private EnemiesSpawner _enemiesSpawner;
        [SerializeField] private Transform _poolsContainer;
        [SerializeField] private Bonus[] _bonusesPrefabs;
        [Space]
        [SerializeField] private float _spawnChance;

        private Transform _container;

        private Dictionary<object, PoolMono<Bonus>> _bonusesPools;
        private int _startBonusesInPoolCount = 5;

        private const string BONUS_POOL_CONTAINER_NAME = "Bonuses";        

        private void Start()
        {
            _bonusesPools = new Dictionary<object, PoolMono<Bonus>>();

            _enemiesSpawner = _enemiesSpawner != null ? _enemiesSpawner : FindObjectOfType<EnemiesSpawner>();
            _poolsContainer = _poolsContainer ? _poolsContainer : GameObject.FindGameObjectWithTag(Constans.POOLS_CONTAINER_TAG).transform;

            _container = new GameObject(BONUS_POOL_CONTAINER_NAME).transform;
            _container.SetParent(_poolsContainer);           

            for (int i = 0; i < _bonusesPrefabs.Length; i++)
                if (!_bonusesPools.ContainsKey(_bonusesPrefabs[i]))
                    _bonusesPools.Add(_bonusesPrefabs[i], new PoolMono<Bonus>(_bonusesPrefabs[i], _startBonusesInPoolCount, _container));

            _enemiesSpawner.OnEnemyDie += SpawnBonus;
        }

        private void OnDestroy()
        {
            if (_enemiesSpawner) _enemiesSpawner.OnEnemyDie -= SpawnBonus;
        }

        private void SpawnBonus(EnemyCore enemy)
        {
            float chance = Random.Range(1, 101);
            if (chance <= _spawnChance)
            {
                Transform bonus = _bonusesPools[_bonusesPrefabs[Random.Range(0, _bonusesPrefabs.Length)]].GetObjectFromPool().transform;
                bonus.position = enemy.transform.position;
            }
        }
    }
}
