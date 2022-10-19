using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI
{
    [RequireComponent(typeof(Image))]
    public class PlayerEnergyUI : MonoBehaviour
    {
        [Header("Health Bar Settings")]
        [SerializeField] private PlayerShield _playerShield;
        [SerializeField] private Text _textForNumbers;

        private Image _image;

        private void Start()
        {
            _image = GetComponent<Image>();

            if (_playerShield) _playerShield.OnEnergyChanged += OnEnergyChanged;
        }

        private void OnDestroy() { if (_playerShield) _playerShield.OnEnergyChanged -= OnEnergyChanged; }

        private void OnEnergyChanged()
        {
            _image.fillAmount = _playerShield.Energy / _playerShield.MaxEnergy;
            if (_textForNumbers)
            {
                int per = (int)(100 * (_playerShield.Energy / _playerShield.MaxEnergy)); 

                StringBuilder text = new StringBuilder();
                text.Append(per);
                text.Append("%");
                _textForNumbers.text = text.ToString();
            }
        }
    }
}