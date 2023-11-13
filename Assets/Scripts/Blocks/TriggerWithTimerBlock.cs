using System.Collections.Generic;
using UnityEngine;

namespace ZFGinc.Assets.WorldOfCubes
{
    public class TriggerWithTimerBlock : EventBlock, IClickable
    {
        [SerializeField] private bool _state = false;
        [SerializeField] private float _timer = 5f;

        private bool _isTriggered = false;

        public override BlockInfo GetInfo()
        {
            if (!_storability) return null;

            List<BlockInfo> infos = new List<BlockInfo>();
            foreach (Block block in _linkedBlocks)
                infos.Add(block.GetInfo());

            return new TriggerWithTimerBlockInfo(_idBlock, transform.position, transform.rotation, _state, _timer, infos);
        }

        public void SetTimer(float timer) => _timer = timer;

        private void Update()
        {
            if (_isTriggered) return;
            if (!_state) return;

            _timer -= Time.deltaTime;

            if (_timer > 0) return;

            _state = true;
            Action();
        }

        private void Action()
        {
            _isTriggered = true;

            InvokeAll(true);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                _state = true;
            }
        }

        public override void SetInfo(BlockInfo _info)
        {
            if (_info is not TriggerWithTimerBlockInfo) return;

            TriggerWithTimerBlockInfo info = (TriggerWithTimerBlockInfo)_info;

            DefaultSettings(info);

            MapLoader.LoadLinkedBlocks(info.Links, this);

            SetTimer(info.Timer);
        }

        public new List<UIComponents> GetUI()
        {
            return new List<UIComponents>(new UIComponents[] { UIComponents.Timer });
        }
    }
}