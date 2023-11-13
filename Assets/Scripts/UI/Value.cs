using UnityEngine;
using TMPro;

namespace ZFGinc.Assets.WorldOfCubes
{
    public class Value : UIBase, IChange
    {
        [SerializeField] private TMP_InputField _text;
        [Space]
        [SerializeField] private float _value;

        private void Start()
        {
            _text.onEndEdit.AddListener(delegate { OnChange(); });
        }

        public void OnChange()
        {
            if (_text.text == "") return;

            _value = float.Parse(_text.text);

            _blockSettings.SetTimer(_value);
            _blockSettings.SetSpeed(_value);
            _blockSettings.SetTriggerRotation(_value);
        }

        public override void SetInfo(BlockInfo info)
        {
            if (info.GetType() == typeof(TriggerWithTimerBlockInfo))
            {
                _value = ((TriggerWithTimerBlockInfo)info).Timer;
            }
            else if (info.GetType() == typeof(MovementBlockInfo))
            {
                _value = ((MovementBlockInfo)info).Speed;
            }
            else if (info.GetType() == typeof(RotationBlockInfo))
            {
                _value = ((RotationBlockInfo)info).Speed;
            }
            else if (info.GetType() == typeof(ValveBlockInfo))
            {
                _value = ((ValveBlockInfo)info).TriggerRotation;
            }

            _text.text = _value.ToString();
        }

        public override void SetInfo(BlockInfo info, ref Block block) { }
    }
}