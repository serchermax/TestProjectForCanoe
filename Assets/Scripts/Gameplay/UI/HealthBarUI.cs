using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI
{
    [RequireComponent(typeof(Image))]
    public class HealthBarUI : MonoBehaviour
    {
        [Header("Health Bar Settings")]
        [SerializeField] private GameObject _iHealth;
        [SerializeField] private Text _textForNumbers;

        private Image _image;
        private IHealth _health;

        private void Start()
        {
            _image = GetComponent<Image>();

            if (_iHealth.TryGetComponent(out _health)) _health.OnHealthChanged += OnDamage;
            else Debug.LogError(this + " can't get IHealth interface from " + _iHealth);
        }

        private void OnDestroy() { if (_health != null) _health.OnHealthChanged -= OnDamage; }

        private void OnDamage()
        {
            _image.fillAmount = _health.Health / _health.MaxHealth;
            if (_textForNumbers)
            {
                StringBuilder text = new StringBuilder();
                text.Append(_health.Health);
                text.Append("/");
                text.Append(_health.MaxHealth);
                _textForNumbers.text = text.ToString();
            }
        }
    }
}
