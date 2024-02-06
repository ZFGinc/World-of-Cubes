using UnityEngine;

namespace ZFGinc.WorldOfCubes
{
    public class Lamp : MonoBehaviour
    {
        [SerializeField] private Material[] _materials;
        [SerializeField] private Color[] _colors;

        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private Light _light;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag != "Player") return;

            int index = Random.Range(0, _materials.Length);
            _meshRenderer.material = _materials[index];
            _light.color = _colors[index];
        }
    }
}