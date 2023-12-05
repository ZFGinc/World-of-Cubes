using UnityEngine;

namespace ZFGinc.Assets.WorldOfCubes
{
    [SelectionBase]
    [RequireComponent(typeof(Animator))]
    public class FinalTree : MonoBehaviour
    {
        private Animator _animator;
        private bool _isAlive = false;

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag != "Player") return;
            if (_isAlive) return;
                
            _isAlive = true;
            _animator.SetTrigger("alive");
        }
    }
}