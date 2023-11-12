using System.Collections.Generic;
using UnityEngine;

public class RotationBlock : Block, IClickable
{
    private float _speed;
    private Quaternion _oldRotation;
    private Quaternion _newRotation;

    public override BlockInfo GetInfo() => (_storability) ? new RotationBlockInfo(_idBlock, transform.position, _oldRotation, _newRotation, _speed) : null;

    public new void SetRotation(Quaternion rot)
    {
        _oldRotation = rot;
        transform.rotation = rot;
    }

    public void SetSpeed(float speed) => _speed = speed;

    public void SetNewRotation(Quaternion rot)
    {
        _newRotation = rot;
    }

    private void Update()
    {
        if (transform.rotation == _oldRotation)
        {
            _oldRotation = _newRotation;
            _newRotation = transform.rotation;
        }
        if (transform.rotation == _newRotation)
        {
            _newRotation = _oldRotation;
            _oldRotation = transform.rotation;
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, _newRotation, Time.deltaTime * _speed);
    }

    public override void SetInfo(BlockInfo _info)
    {
        if (_info is not RotationBlockInfo) return;

        RotationBlockInfo info = (RotationBlockInfo)_info;

        DefaultSettings(info);

        SetSpeed(info.Speed);
        SetNewRotation(new Quaternion(info.newrotX, info.newrotY, info.newrotZ, info.newrotW));
    }

    public List<UIComponents> GetUI()
    {
        return new List<UIComponents>(new UIComponents[] { UIComponents.Link, UIComponents.Speed, UIComponents.OldRotation, UIComponents.NewRotation });
    }
}
