using UnityEngine;
using Gameplay.Input;

namespace Gameplay
{
    public class ShipMotorPlayer : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private ParticleSystem _effectPrefab;
        [SerializeField] private Transform _pointForEffect;
        [Space]
        [SerializeField] private PlayerMoveInput.MoveInputTypes _effectCallOnInput;
        [SerializeField] private Vector3 _effectEulerAngles;

        private PlayerMoveInput _playerMoveInput;
        private ParticleSystem _effectObj;

        private void Start()
        {
            _playerMoveInput = PlayerMoveInput.Instance;
            _playerMoveInput.AddListenerOnStartInput(PlayEffect, _effectCallOnInput);
            _playerMoveInput.AddListenerOnStopInput(StopEffect, _effectCallOnInput);

            _effectObj = Instantiate(_effectPrefab, _pointForEffect);
            _effectObj.transform.eulerAngles = _effectEulerAngles;

            StopEffect();
        }

        private void OnDestroy()
        {
            if (_playerMoveInput)
            {
                _playerMoveInput.RemoveListenerOnStartInput(PlayEffect, _effectCallOnInput);
                _playerMoveInput.RemoveListenerOnStopInput(StopEffect, _effectCallOnInput);
            }
        }

        private void PlayEffect() => _effectObj.Play();
        private void StopEffect() => _effectObj.Stop();
    }
}