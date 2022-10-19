using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
    [RequireComponent(typeof(Text))]
    public class WaveUI : MonoBehaviour
    {
        [Header("Links")]
        [SerializeField] private EnemiesSpawner _enemiesSpawner;

        private Text _text;

        private void Start()
        {
            _text = GetComponent<Text>();
            _enemiesSpawner = _enemiesSpawner ? _enemiesSpawner : FindObjectOfType<EnemiesSpawner>();
            _enemiesSpawner.OnWaveChanged += OnWaveChanged;
            OnWaveChanged();
        }

        private void OnDestroy() { if (_enemiesSpawner) _enemiesSpawner.OnWaveChanged -= OnWaveChanged; }

        private void OnWaveChanged()
        {
            _text.text = (_enemiesSpawner.Wave + 1).ToString();
        }
    }
}