using UnityEngine;
using TMPro;

namespace ZFGinc.Assets.WorldOfCubes
{
    public class DropBox : UIBase, IChange
    {
        [SerializeField] private TMP_Dropdown _dropdown;
        [Space]
        [SerializeField] private int _value;

        private void Start()
        {
            _dropdown.onValueChanged.AddListener(delegate { OnChange(); });
        }

        public void OnChange()
        {
            _value = _dropdown.value;

            _blockSettings.SetEventAction((EventAction)_value);
        }

        public override void SetInfo(BlockInfo info)
        {
            _value = (int)info.eventAction;
            _dropdown.value = _value;
        }

        public override void SetInfo(BlockInfo info, Block block) { }
    }
}