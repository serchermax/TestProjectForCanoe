using UnityEngine;

namespace Gameplay
{
    public class GunController : MonoBehaviour
    {
        private Transform _transform;
        private Camera _camera;
        private Transform _cameraTransform;
        private Vector3 _mousePos;

        private void Start()
        {
            _transform = transform;
            _camera = Camera.main;
            _cameraTransform = _camera.transform;
        }

        private void Update()
        {
            _mousePos = UnityEngine.Input.mousePosition;
            _mousePos.z = _cameraTransform.position.y - _transform.position.y;
            _mousePos = _camera.ScreenToWorldPoint(_mousePos);
            _transform.LookAt(_mousePos);
        }
    }
}