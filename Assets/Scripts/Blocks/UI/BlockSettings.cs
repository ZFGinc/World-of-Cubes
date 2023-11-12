using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
struct UIDictionary
{
    public UIBase Object;
    public UIComponents Component;
}

public class BlockSettings : MonoBehaviour
{
    [SerializeField] private List<UIDictionary> _uiComponents = new List<UIDictionary>();

    private Block _currentBlock;

    private Vector3 _v, _k;

    public void Show(ref Block currentBlock, List<UIComponents> components)
    {
        _currentBlock = currentBlock;

        ShowUI(components);

        foreach (UIDictionary dict in _uiComponents) 
        {
            if (components.Contains(dict.Component))
            {
                if (dict.Component is UIComponents.Link)
                {
                    dict.Object.SetInfo(currentBlock.GetInfo(), ref _currentBlock);
                }
                else
                {
                    dict.Object.SetInfo(currentBlock.GetInfo());
                }
            }
        }
    }

    public void ShowUI(List<UIComponents> components)
    {
        foreach (UIDictionary dict in _uiComponents)
        {
            dict.Object.gameObject.SetActive(components.Contains(dict.Component));
        }
    }

    public void HideUI()
    {
        foreach (UIDictionary dict in _uiComponents)
        {
            dict.Object.gameObject.SetActive(false);
        }
    }

    public Quaternion EulertoQuaternion(Vector3 v) => Quaternion.Euler(v.x, v.y, v.z);

    public void SetState(bool state) 
    {
        if (_currentBlock.TryGetComponent(out TriggerBlock block))
        {
            block.SetState(state);
        }
    }
    public void SetTimer(float time) 
    {
        if (_currentBlock.TryGetComponent(out TriggerWithTimerBlock block))
        {
            block.SetTimer(time);
        }
    }
    public void SetSpeed(float speed)
    {
        if (_currentBlock.TryGetComponent(out MovementBlock b1) | _currentBlock.TryGetComponent(out RotationBlock b2))
        {
            if (b1 != null) b1.SetSpeed(speed);
            if (b2 != null) b2.SetSpeed(speed);
        }
    }
    public void SetOldPosition(Vector3 v)
    {
        _v = v;
        if (_currentBlock.TryGetComponent(out MovementBlock block))
        {
            block.SetPositions(_v, _k);
        }
    }
    public void SetNewPosition(Vector3 k) 
    {
        _k = k;
        if (_currentBlock.TryGetComponent(out MovementBlock block))
        {
            block.SetPositions(_v,_k);
        }
    }
    public void SetOldRotation(Vector3 v) 
    {
        if (_currentBlock.TryGetComponent(out RotationBlock block))
        {
            block.SetRotation(EulertoQuaternion(v));
        }
    }
    public void SetNewRotation(Vector3 v) 
    {
        if (_currentBlock.TryGetComponent(out RotationBlock block))
        {
            block.SetNewRotation(EulertoQuaternion(v));
        }
    }
    public void SetTriggerRotation(float angle) 
    {
        if (_currentBlock.TryGetComponent(out ValveBlock block))
        {
            block.SetTriggerRotation(angle);
        }
    }
    public void SetRemember(bool rem) 
    {
        if (_currentBlock.TryGetComponent(out ValveBlock block))
        {
            block.SetRemember(rem);
        }
    }


}
