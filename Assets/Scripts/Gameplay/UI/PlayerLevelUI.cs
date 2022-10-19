using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI
{
    [RequireComponent(typeof(Image))]
    public class PlayerLevelUI : MonoBehaviour
    {
        [Header("Level Bar Settings")]
        [SerializeField] private PlayerProgress _playerProgress;
        [SerializeField] private Text _textForLevel;

        private Image _bar;

        private void Start()
        {
            _playerProgress = _playerProgress != null ? _playerProgress : FindObjectOfType<PlayerProgress>();
            _bar = GetComponent<Image>();
            _playerProgress.OnExpChanged += OnExpChanged;
            _playerProgress.OnExpChanged += OnLevelChanged;

            OnExpChanged();
            OnLevelChanged();
        }

        private void OnDestroy() 
        { 
            if (_playerProgress != null)
            {
                _playerProgress.OnExpChanged -= OnExpChanged;
                _playerProgress.OnExpChanged -= OnLevelChanged;
            }
        }

        private void OnExpChanged()
        {
            _bar.fillAmount = (float)_playerProgress.Exp / (float)_playerProgress.ExpForNextLevel;
        }

        private void OnLevelChanged()
        {
            if (_textForLevel) _textForLevel.text = (_playerProgress.Level+1).ToString();
        }
    }
}