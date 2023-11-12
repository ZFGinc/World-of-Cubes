using System.Collections.Generic;
using UnityEngine;

public class ValveBlock : EventBlock, IClickable
{
    [SerializeField] private float _currentRotation = 45f;
    [SerializeField] private float _triggerRotation = 180f;
    [SerializeField] private bool _isRemember = true;
    [Space]
    [SerializeField] private Transform _valve;
    [Space(10)]
    [SerializeField] private bool _isRotate = false;

    public override BlockInfo GetInfo()
    {
        if(!_storability) return null;

        List<BlockInfo> infos = new List<BlockInfo>();
        foreach(Block block in _linkedBlocks)
            infos.Add(block.GetInfo());
        
        return new ValveBlockInfo(_idBlock, transform.position, transform.rotation, _triggerRotation, _isRemember, infos);
    }

    public void SetTriggerRotation(float triggerRotation) => _triggerRotation = triggerRotation;
    public void SetRemember(bool isRemember) => _isRemember = isRemember;

    private void Update()
    {
        Rotate();
        if (_isRotate) return;
        if (_isRemember) return;
        if(_currentRotation == 0f) return;

        _currentRotation -= Time.deltaTime * 20f;
        _valve.localRotation = Quaternion.Euler(0, _currentRotation, 0);

        if (_currentRotation < 0f) _currentRotation = 0f;
    }

    public void Rotate()
    {
        if (!_isRotate) return;

        _currentRotation += Time.deltaTime * 30f;
        _valve.localRotation = Quaternion.Euler(0, _currentRotation, 0);

        Action();
    }

    private void Action()
    {
        if (_currentRotation < _triggerRotation) return;

        InvokeAll(true);
    }

    public override void SetInfo(BlockInfo _info)
    {
        if (_info is not ValveBlockInfo) return;

        ValveBlockInfo info = (ValveBlockInfo)_info;

        DefaultSettings(info);

        MapLoader.LoadLinkedBlocks(info.Links, this);

        SetRemember(info.IsRemember);
        SetTriggerRotation(info.TriggerRotation);
    }

    public List<UIComponents> GetUI()
    {
        return new List<UIComponents>(new UIComponents[] { UIComponents.TriggerRotation, UIComponents.IsRemember });
    }
}
