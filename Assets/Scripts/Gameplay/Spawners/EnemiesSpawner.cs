using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class EnemiesSpawner : MonoBehaviour
    {
        [Header("Links")]
        [SerializeField] private Transform _player;
        [SerializeField] private Transform _poolsContainer;

        [Header("Spawner Settings")]
        [SerializeField] private EnemyPack[] _enemyPacks;
        [SerializeField] private Zone[] _spawnZones;
        [Space]
        [SerializeField] private float _waveTime;
        [SerializeField] private float _startWaveValue;
        [SerializeField] private float _addWaveValue;
        [SerializeField] private float _distantionFromPlayer;
        [Space]
        [SerializeField] private bool _debug;

        private float _timer;
        private float _currentMaxValue;
        private bool _stopSpawn = false;
        private int _startEnemiesInPoolCount = 3;
        private Dictionary<object, PoolMono<EnemyCore>> _enemiesPool;

        private Transform _container;

        private const string ENEMIES_POOL_CONTAINER_NAME = "Enemies";

        public int Wave 
        {
            get { return _wave; }
            private set { _wave = value; OnWaveChanged?.Invoke(); }
        }
        private int _wave;

        public event System.Action<EnemyCore> OnEnemyDie;
        public event System.Action OnWaveChanged;

        private void Start()
        {
            _player = _player ? _player : GameObject.FindGameObjectWithTag(Constans.PLAYER_TAG).transform;
            _poolsContainer = _poolsContainer ? _poolsContainer : GameObject.FindGameObjectWithTag(Constans.POOLS_CONTAINER_TAG).transform;

            _container = new GameObject(ENEMIES_POOL_CONTAINER_NAME).transform;
            _container.SetParent(_poolsContainer);

            Wave = 0;
            _currentMaxValue = _startWaveValue;
            _enemiesPool = new Dictionary<object, PoolMono<EnemyCore>>();

            for (int i = 0; i < _enemyPacks.Length; i++)
                if (!_enemiesPool.ContainsKey(_enemyPacks[i].Enemy)) 
                    _enemiesPool.Add(_enemyPacks[i].Enemy, new PoolMono<EnemyCore>(_enemyPacks[i].Enemy, _startEnemiesInPoolCount, _container));

            Spawn();
        }

        private void Update()
        {
            if (_stopSpawn) return;

            if (_timer < _waveTime) _timer += Time.deltaTime;
            else
            {
                _timer = 0f;
                Wave++;
                _currentMaxValue += _addWaveValue;
                Spawn();
            }
        }

        private void Spawn()
        {
            float value = 0;

            while (value < _currentMaxValue)
            {
                if (TryGetEnemy(out EnemyCore enemy, ref value))
                {
                    enemy.OnDestroy += () => EnemyDie(enemy);
                    enemy.transform.position = GetSpawnPosition();
                    enemy.transform.LookAt(_player.position);
                }
                else value = _currentMaxValue;
            }
        }

        private bool TryGetEnemy(out EnemyCore enemy, ref float value)
        {
            enemy = null;
            ShuffleArray(ref _enemyPacks);

            for (int i = 0; i < _enemyPacks.Length; i++)
                if (_enemyPacks[i].StartSpawnFromWave <= Wave && _enemyPacks[i].Value + value <= _currentMaxValue)
                {
                    enemy = _enemiesPool[_enemyPacks[i].Enemy].GetObjectFromPool();
                    value += _enemyPacks[i].Value;
                    break;
                }
            return enemy != null;
        }

        private void EnemyDie(EnemyCore enemy)
        {
            enemy.OnDestroy -= () => EnemyDie(enemy);
            OnEnemyDie?.Invoke(enemy);
        }

        private Vector3 GetSpawnPosition()
        {
            Zone zone;
            Vector3 pos = new Vector3();

            for (int i = 0; i < 100; i++)
            {
                zone = _spawnZones[Random.Range(0, _spawnZones.Length)];
                pos = zone.GetRandomPosition();

                if (Vector3.Distance(pos, _player.position) > _distantionFromPlayer) return pos;
                else continue;
            }
            return pos;
        }

        private void ShuffleArray<T>(ref T[] array)
        {
            System.Random random = new System.Random();
            for (int i = array.Length - 1; i >= 1; i--)
            {
                int j = random.Next(i + 1);
                var temp = array[j];
                array[j] = array[i];
                array[i] = temp;
            }
        }

        [System.Serializable]
        private struct Zone
        {
            public Vector3 position;
            public Vector3 scale;
            public Vector3 GetRandomPosition()
            {
                return position 
                    + new Vector3(Random.Range(-scale.x / 2f, scale.x / 2f)
                    , Random.Range(-scale.y / 2f, scale.y / 2f)
                    , Random.Range(-scale.z / 2f, scale.z / 2f));
            }
        }

        [System.Serializable]
        private struct EnemyPack
        {
            public EnemyCore Enemy;
            public int Value;
            public int StartSpawnFromWave;
        }

        #region Debug
        private void OnDrawGizmosSelected()
        {
            if (!_debug || _spawnZones.Length <= 0) return;

            for (int i = 0; i < _spawnZones.Length; i++)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireCube(_spawnZones[i].position, _spawnZones[i].scale);
            }
        }
        #endregion
    }
}