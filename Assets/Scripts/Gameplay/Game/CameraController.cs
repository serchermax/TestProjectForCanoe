using UnityEngine;

namespace Gameplay
{
    public class CameraController : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private Transform _player;
        [SerializeField] private float _yOffset = 10f;
        [SerializeField] private float _cameraSpeed = 1f;
        [SerializeField] [Range(0f, 1f)] private float _offsetToPlayer;

        private Transform _transform;
        private Camera _camera;
        private Vector3 _mousePos;
        private Vector3 _playerFollow;

        private void Start()
        {
            _player = _player == null ? GameObject.FindGameObjectWithTag(Constans.PLAYER_TAG).transform : _player;
            _transform = transform;
            _camera = Camera.main;
        }

        private void LateUpdate()
        {
            _mousePos = UnityEngine.Input.mousePosition;
            _mousePos.z = _yOffset - _player.position.y;
            _mousePos = _camera.ScreenToWorldPoint(_mousePos);

            _playerFollow = (_mousePos + _player.position) / 2;
            _playerFollow = Vector3.Lerp(_playerFollow, _player.position, _offsetToPlayer);
            _playerFollow = Vector3.Lerp(_transform.position, _playerFollow, _cameraSpeed * Time.deltaTime);           
            _playerFollow.y = _yOffset;         

            _transform.position = _playerFollow;
        }
    }
}