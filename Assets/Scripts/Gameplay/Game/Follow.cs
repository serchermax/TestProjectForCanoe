using UnityEngine;

namespace Gameplay
{
    public class Follow : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        private Transform _transform;

        private void Start() => _transform = transform;

        private void Update() => _transform.position = _target.position;
    }
}