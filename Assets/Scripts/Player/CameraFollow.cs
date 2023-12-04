using UnityEngine;

namespace ZFGinc.Assets.WorldOfCubes
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform _targetPlayer;
        [SerializeField] private Vector3 _offset;
        [SerializeField] private float _speed;

        private void Start()
        {
            transform.parent = null;
        }

        private void Update()
        {
            float distance = Vector3.Distance(transform.position, _targetPlayer.position);
            transform.localPosition = Vector3.Lerp(transform.position, _targetPlayer.position + _offset, distance * _speed * Time.deltaTime);
        }
    }
}