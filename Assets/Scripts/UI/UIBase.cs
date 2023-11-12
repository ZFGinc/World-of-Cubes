using UnityEngine;

public abstract class UIBase : MonoBehaviour
{
    [SerializeField] protected BlockSettings _blockSettings;

    public abstract void SetInfo(BlockInfo info);

    public abstract void SetInfo(BlockInfo info, ref Block block);

    private void Start()
    {
        Disable();
    }

    public void Enable()
    {
        if (this != null)
            gameObject.SetActive(true);
    }

    public void Disable()
    {
        if(this != null)
            gameObject.SetActive(false);
    }
}
