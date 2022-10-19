using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gameplay
{
    public class LevelProgress : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private GameObject _player;
        [SerializeField] private float _reloadTime;

        private IDestroyable _playerDestroyable;

        private void Start()
        {
            _player = _player ? _player : GameObject.FindGameObjectWithTag(Constans.PLAYER_TAG);
            if (_player.TryGetComponent(out _playerDestroyable)) _playerDestroyable.OnDestroy += GameLost;
            else Debug.LogError(this + " can't get IHealth interface from " + _player);
        }
        private void OnDestroy()
        {
             if (_playerDestroyable != null) _playerDestroyable.OnDestroy -= GameLost;
        }
       
        public void GameLost()
        {
            _playerDestroyable.OnDestroy -= GameLost;
            StartCoroutine(Reload());
        }

        private IEnumerator Reload()
        {
            yield return new WaitForSeconds(_reloadTime);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}