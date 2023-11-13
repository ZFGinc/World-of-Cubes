using UnityEngine.UI;
using UnityEngine;

namespace ZFGinc.Assets.WorldOfCubes
{
    public class Toggle : UIBase, IChange
    {
        [SerializeField] private Image _image;
        [SerializeField] private Button button;
        [Space]
        [SerializeField] private Sprite _turnOnSprite;
        [SerializeField] private Sprite _turnOffSprite;
        [Space]
        [SerializeField] private bool _state = false;

        private void Start()
        {
            button.onClick.AddListener(() => OnChange());
        }

        public void OnChange()
        {
            _state = !_state;

            SpriteSwitch();
            Action();
        }

        private void Action()
        {
            _blockSettings.SetState(_state);
            _blockSettings.SetRemember(_state);
        }

        public override void SetInfo(BlockInfo info)
        {
            if (info.GetType() == typeof(TriggerBlockInfo))
            {
                _state = ((TriggerBlockInfo)info).State;
            }
            else if (info.GetType() == typeof(ValveBlockInfo))
            {
                _state = ((ValveBlockInfo)info).IsRemember;
            }

            SpriteSwitch();
        }

        public override void SetInfo(BlockInfo info, ref Block block) { }

        private void SpriteSwitch()
        {
            if (_turnOffSprite == null || _turnOnSprite == null) return;

            if (_state) _image.sprite = _turnOnSprite;
            else _image.sprite = _turnOffSprite;
        }
    }
}