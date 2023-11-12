using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class OptimizeRender : MonoBehaviour
{
    protected void Awake()
    {
        MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.SetPropertyBlock(materialPropertyBlock);
    }
}
