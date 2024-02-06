using System.Collections.Generic;
using UnityEngine;
using ZFGinc.WorldOfCubes.UI;

namespace ZFGinc.WorldOfCubes
{
    public class TriggerWithTimerBlock : EventBlock, IClickable
    {
        [SerializeField] private bool _state = false;
        [SerializeField] private float _timer = 5f;
        [SerializeField] private Animator _animator;

        private bool _isTriggered = false;

        public override BlockInfo GetInfo()
        {
            if (!_storability) return null;

            List<BlockInfo> infos = new List<BlockInfo>();
            foreach (Block block in _linkedBlocks)
                infos.Add(block.GetInfo());

            return new TriggerWithTimerBlockInfo(_idBlock, transform.position, transform.rotation, _eventAction, _state, _timer, infos);
        }

        public void SetTimer(float timer) => _timer = timer;

        private void Update()
        {
            if (_isTriggered) return;
            if (!_state) return;

            _timer -= Time.deltaTime;
            _animator.SetBool("cracked", true);

            if (_timer > 0) return;

            _state = true;
            Action();
        }

        private void Action()
        {
            _isTriggered = true;

            Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag != "Player") return;
            
            _state = true;
        }

        public override void SetInfo(BlockInfo _info)
        {
            if (_info is not TriggerWithTimerBlockInfo) return;

            TriggerWithTimerBlockInfo info = (TriggerWithTimerBlockInfo)_info;

            DefaultSettings(info);

            MapLoader.LoadLinkedBlocks(info.Links, this);

            SetTimer(info.Timer);
        }

        public override List<UIComponents> GetUI()
        {
            return new List<UIComponents>(new UIComponents[] { UIComponents.Timer });
        }
    }
}