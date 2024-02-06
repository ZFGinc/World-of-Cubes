using System.Collections.Generic;
using UnityEngine;
using ZFGinc.WorldOfCubes.UI;

namespace ZFGinc.WorldOfCubes
{
    public delegate void OnTurn(bool state);

    [RequireComponent(typeof(AudioSource))]
    public class TriggerBlock : EventBlock, IClickable, IContactable
    {
        [SerializeField] private bool _state = false;
        [SerializeField] private bool _isPressurePlate = false;

        public AudioSource AudioSource { get; set; }

        public override BlockInfo GetInfo()
        {
            if (!_storability) return null;

            List<BlockInfo> infos = new List<BlockInfo>();
            foreach (Block block in _linkedBlocks)
                infos.Add(block.GetInfo());

            return new TriggerBlockInfo(_idBlock, transform.position, transform.rotation, _eventAction, _state, infos);
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

            AudioSource = GetComponent<AudioSource>();
        }

        public override List<UIComponents> GetUI()
        {
            return new List<UIComponents>(new UIComponents[] { UIComponents.State });
        }

        public void Contact(bool state)
        {
            SetState(state);

            AudioSource.Play();
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "Player")
            {
                if (!_isPressurePlate) return;

                SetState(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                if (!_isPressurePlate) return;

                SetState(false);
            }
        }
    }

}
