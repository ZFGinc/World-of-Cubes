using UnityEngine;
using TMPro;

namespace ZFGinc.WorldOfCubes.UI
{
    public class Link : UIBase, IChange
    {
        [SerializeField] private Block block;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private GameObject _buttonAddLink;
        [SerializeField] private GameObject _buttonRemoveLink;

        private bool _newLink = false;
        private ILinkable _linkable;

        private void Update()
        {
            if (!_newLink) return;

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.collider.TryGetComponent(out ILinkable _linkBlock))
                {
                    if (Input.GetMouseButtonDown(1))
                    {
                        _linkable = _linkBlock;
                        Subscibe(_linkBlock);
                        ShowInfo(_linkBlock);
                        _newLink = false;
                    }
                }
            }
        }

        private void Subscibe(ILinkable linkBlock)
        {
            bool res = linkBlock.TrySubscibe(block);
            ShowButton(res);

            if (!res) TryUnsubscibe(linkBlock);
        }

        private void TryUnsubscibe(ILinkable linkBlock)
        {
            bool res = linkBlock.TryUnsubscibe(block);
            ShowButton(!res);
        }

        public void Unsubscibe()
        {
            TryUnsubscibe(_linkable);
        }

        private void ShowButton(bool isLinked)
        {
            if (isLinked)
            {
                _buttonAddLink.SetActive(false);
                _buttonRemoveLink.SetActive(true);
            }
            else
            {
                _buttonAddLink.SetActive(true);
                _buttonRemoveLink.SetActive(false);
                _text.text = "null";
            }
        }

        private void ShowInfo(ILinkable linkBlock)
        {
            _text.text = linkBlock.ToString();
        }

        public void OnChange()
        {
            _newLink = true;
        }

        public override void SetInfo(BlockInfo info) { }

        public override void SetInfo(BlockInfo info, Block block)
        {
            if (block == null) return;
            this.block = block;

            bool isHasLink = block.Link == null;

            if (isHasLink) _text.text = "null";
            else _text.text = block.Link.ToString();

            ShowButton(!isHasLink);
        }
    }
}