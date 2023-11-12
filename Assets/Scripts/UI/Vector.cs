using UnityEngine;
using TMPro;

public class Vector : UIBase, IChange
{
    [SerializeField] private TMP_InputField _x;
    [SerializeField] private TMP_InputField _y;
    [SerializeField] private TMP_InputField _z;
    [Space]
    [SerializeField] private Vector3 _v;
    [SerializeField] private bool _isNewVector = false;

    private void Start()
    {
        _x.onEndEdit.AddListener(delegate { OnChange(); });
        _y.onEndEdit.AddListener(delegate { OnChange(); });
        _z.onEndEdit.AddListener(delegate { OnChange(); });
    }

    public void OnChange()
    {
        if (_x.text == "" || _y.text == "" || _z.text == "") return;

        _v = new Vector3(float.Parse(_x.text), float.Parse(_y.text), float.Parse(_z.text));

        if (_isNewVector)
        {
            _blockSettings.SetNewPosition(_v);
            _blockSettings.SetNewRotation(_v);
        }
        else
        {
            _blockSettings.SetOldPosition(_v);
            _blockSettings.SetOldRotation(_v);
        }                
    }

    public override void SetInfo(BlockInfo info)
    {
        Vector3 v = Vector3.zero;

        if (info.GetType() == typeof(MovementBlockInfo))
        {
            v = GetXYZ((MovementBlockInfo)info);
        }
        else if (info.GetType() == typeof(RotationBlockInfo))
        {
            v = GetXYZ((RotationBlockInfo)info);
        }

        _x.text = v.x.ToString();
        _y.text = v.y.ToString();
        _z.text = v.z.ToString();

        _v = v;
    }

    public override void SetInfo(BlockInfo info, ref Block block) { }

    private Vector3 GetXYZ(MovementBlockInfo info)
    {
        if (_isNewVector) return new Vector3(info.newposX, info.newposY, info.newposZ);
        return new Vector3(info.posX, info.posY, info.posZ);
    }

    private Vector3 GetXYZ(RotationBlockInfo info)
    {
        if (_isNewVector) return QuaternionToEuler(new Quaternion(info.newrotX, info.newrotY, info.newrotZ, info.newrotW));
        return QuaternionToEuler(new Quaternion(info.rotX, info.rotY, info.rotZ, info.rotW));
    }

    public Vector3 QuaternionToEuler(Quaternion q) => q.eulerAngles;
}
