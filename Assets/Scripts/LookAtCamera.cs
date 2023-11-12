using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField] private Transform _camera;
    [SerializeField] private bool _startFind;

    private void Start()
    {
        if (!_startFind) return;

        _camera = FindAnyObjectByType<Camera>().transform;
    }

    private void Update()
    {
        transform.LookAt(_camera.position);
    }
}
