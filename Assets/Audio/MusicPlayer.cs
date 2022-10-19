using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class MusicPlayer : MonoBehaviour
    {
        [SerializeField] private AudioClip[] _music;        

        private AudioSource _audioSource;
        private int _currentId;

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            _currentId = Random.Range(0, _music.Length);
            StartCoroutine(Play());
            
        }

        private IEnumerator Play()
        {
            while (true)
            {
                NewRandomId(ref _currentId);
                _audioSource.clip = _music[_currentId];
                _audioSource.Play();
                yield return new WaitForFixedUpdate();
                yield return new WaitForSourseEndPlay(_audioSource);
            }
        }

        private void NewRandomId(ref int id)
        {
            int temp;
            do temp = Random.Range(0, _music.Length);
            while (temp == id && _music.Length > 1);
            id = temp;
        }

        private class WaitForSourseEndPlay : CustomYieldInstruction
        {
            private AudioSource _audioSource;

            public WaitForSourseEndPlay(AudioSource audioSource)
            {
                _audioSource = audioSource;
            }

            public override bool keepWaiting => _audioSource.isPlaying;
        }
    }
}
