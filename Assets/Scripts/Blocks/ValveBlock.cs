using System.Collections.Generic;
using UnityEngine;

namespace ZFGinc.Assets.WorldOfCubes
{
    [RequireComponent(typeof(AudioSource))]
    public class ValveBlock : EventBlock, IClickable, IContactable
    {
        [SerializeField] private float _currentRotation = 45f;
        [SerializeField] private float _triggerRotation = 180f;
        [SerializeField] private bool _isRemember = true;
        [Space]
        [SerializeField] private Transform _valve;
        [Space(10)]
        [SerializeField] private bool _isRotate = false;

        public AudioSource AudioSource { get; set; }

        public override BlockInfo GetInfo()
        {
            if (!_storability) return null;

            List<BlockInfo> infos = new List<BlockInfo>();
            foreach (Block block in _linkedBlocks)
                infos.Add(block.GetInfo());

            return new ValveBlockInfo(_idBlock, transform.position, transform.rotation, _eventAction, _triggerRotation, _isRemember, infos);
        }

        public void SetTriggerRotation(float triggerRotation) => _triggerRotation = triggerRotation;
        public void SetRemember(bool isRemember) => _isRemember = isRemember;

        private void Update()
        {
            Rotate();
            if (_isRotate) return;

            AudioSource.Stop();
            if (_isRemember) return;
            if (_currentRotation == 0f) return;

            _currentRotation -= Time.deltaTime * 20f;
            _valve.localRotation = Quaternion.Euler(0, _currentRotation, 0);

            if (_currentRotation < 0f) _currentRotation = 0f;
        }

        private void Rotate()
        {
            if (!_isRotate) return;

            _currentRotation += Time.deltaTime * 30f;
            _valve.localRotation = Quaternion.Euler(0, _currentRotation, 0);

            Action();
        }

        public void Rotate(bool isRotate) => _isRotate = isRotate;

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

            AudioSource = GetComponent<AudioSource>();
        }

        public override List<UIComponents> GetUI()
        {
            return new List<UIComponents>(new UIComponents[] { UIComponents.TriggerRotation, UIComponents.IsRemember });
        }

        public void Contact(bool state)
        {
            Rotate(state);

            AudioSource.Play();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                Rotate(false);

                AudioSource.Stop();
            }
        }
    }
}