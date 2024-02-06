using System.Collections.Generic;
using UnityEngine;
using ZFGinc.WorldOfCubes.UI;

namespace ZFGinc.WorldOfCubes
{
    public class MovementBlock : Block, IClickable
    {
        [SerializeField] private float _speed;
        [SerializeField] private Vector3 _oldPosition;
        [SerializeField] private Vector3 _newPosition;

        public override BlockInfo GetInfo() => (_storability) ? new MovementBlockInfo(_idBlock, _oldPosition, transform.rotation, _eventAction, _newPosition, _speed) : null;

        public new void SetTransform(Vector3 pos)
        {
            transform.position = pos;
        }

        public void SetSpeed(float speed) => _speed = speed;

        public void SetPositions(Vector3 oldpos, Vector3 newpos)
        {
            _oldPosition = oldpos;
            _newPosition = newpos;
        }

        private void Update()
        {
            if (transform.position == _oldPosition)
            {
                _oldPosition = _newPosition;
                _newPosition = transform.position;
            }

            if (transform.position == _newPosition)
            {
                _newPosition = _oldPosition;
                _oldPosition = transform.position;
            }

            transform.position = Vector3.MoveTowards(transform.position, _newPosition, Time.deltaTime * _speed);
        }

        public override void SetInfo(BlockInfo _info)
        {
            DefaultSettings(_info);
            if (_info is not MovementBlockInfo) return;

            MovementBlockInfo info = (MovementBlockInfo)_info;

            SetSpeed(info.Speed);
            SetPositions(new Vector3(info.posX, info.posY, info.posZ), new Vector3(info.newposX, info.newposY, info.newposZ));
        }

        public override List<UIComponents> GetUI()
        {
            return new List<UIComponents>(new UIComponents[] { UIComponents.Link, UIComponents.Speed, UIComponents.OldPosition, UIComponents.NewPosition });
        }

        private void OnEnable()
        {
            _oldPosition = transform.position;
        }
    }
}
