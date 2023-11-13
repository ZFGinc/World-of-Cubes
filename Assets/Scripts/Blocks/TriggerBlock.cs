using System.Collections.Generic;
using UnityEngine;

namespace ZFGinc.Assets.WorldOfCubes
{
    public delegate void OnTurn(bool state);

    public class TriggerBlock : EventBlock, IClickable
    {
        [SerializeField] private bool _state = false;

        public override BlockInfo GetInfo()
        {
            if (!_storability) return null;

            List<BlockInfo> infos = new List<BlockInfo>();
            foreach (Block block in _linkedBlocks)
                infos.Add(block.GetInfo());

            return new TriggerBlockInfo(_idBlock, transform.position, transform.rotation, _state, infos);
        }

        public void SetState(bool state)
        {
            _state = state;

            InvokeAll(state);
        }

        public override void SetInfo(BlockInfo _info)
        {
            if (_info is not TriggerBlockInfo) return;

            TriggerBlockInfo info = (TriggerBlockInfo)_info;

            DefaultSettings(info);

            MapLoader.LoadLinkedBlocks(info.Links, this);

            SetState(info.State);
        }

        public new List<UIComponents> GetUI()
        {
            return new List<UIComponents>(new UIComponents[] { UIComponents.State });
        }
    }

}
