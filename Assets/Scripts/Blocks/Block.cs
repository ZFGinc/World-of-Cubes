using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour, IClickable
{
    [SerializeField] protected bool _storability = true;
    [SerializeField] protected int _idBlock;
    //[SerializeField] protected

    public ILinkable Link;

    public int GetID() => _idBlock;

    public virtual BlockInfo GetInfo() => (_storability) ? new BlockInfo(_idBlock, transform.position, transform.rotation) : null;

    public virtual void SetInfo(BlockInfo info)
    {
        DefaultSettings(info);
    }

    protected void SetTransform(Vector3 pos)
    {
        transform.position = pos;
    }

    protected void SetRotation(Quaternion rot)
    {
        transform.rotation = rot;
    }

    protected virtual void DefaultSettings(BlockInfo _info)
    {
        SetTransform(new Vector3(_info.posX, _info.posY, _info.posZ));
        SetRotation(new Quaternion(_info.rotX, _info.rotY, _info.rotZ, _info.rotW));
    }

    public void OnTurn(bool state)
    {
        //Методы при срабатывании ивента от блока на которого подписались
        if (state) Destroy(gameObject);
    }

    public List<UIComponents> GetUI()
    {
        return new List<UIComponents>(new UIComponents[] { UIComponents.Link });
    }
}
