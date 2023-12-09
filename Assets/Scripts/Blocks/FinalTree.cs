using UnityEngine;

namespace ZFGinc.Assets.WorldOfCubes
{
    [SelectionBase]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(AudioSource))]
    public class FinalTree : MonoBehaviour
    {
        private Animator _animator;
        private AudioSource _audioSource;
        private bool _isAlive = false;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _audioSource = GetComponent<AudioSource>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag != "Player") return;
            if (_isAlive) return;
                
            _isAlive = true;
            _animator.SetTrigger("alive");
            _audioSource.Play();
        }
    }
}