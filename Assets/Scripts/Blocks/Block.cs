using System.Collections.Generic;
using UnityEngine;
using ZFGinc.WorldOfCubes.UI;

namespace ZFGinc.WorldOfCubes
{
    [SelectionBase]
    public class Block : MonoBehaviour, IClickable
    {
        [SerializeField] protected bool _storability = true;
        [SerializeField] protected int _idBlock;
        [SerializeField] protected EventAction _eventAction = EventAction.None;

        public ILinkable Link;

        public int GetID() => _idBlock;

        public virtual BlockInfo GetInfo() => (_storability) ? new BlockInfo(_idBlock, transform.position, transform.rotation, _eventAction) : null;

        public virtual void SetInfo(BlockInfo info)
        {
            DefaultSettings(info);
        }

        protected void SetTransform(Vector3 pos)
        {
            transform.position = pos;
        }

        protected void SetRotation(Quaternion rot)
        {
            transform.rotation = rot;
        }

        public void SetEventAction(EventAction eventAction)
        {
            _eventAction = eventAction;            
        }

        public EventAction GetEventAction() { return _eventAction; }

        protected virtual void DefaultSettings(BlockInfo _info)
        {
            SetTransform(new Vector3(_info.posX, _info.posY, _info.posZ));
            SetRotation(new Quaternion(_info.rotX, _info.rotY, _info.rotZ, _info.rotW));
            SetEventAction(_info.eventAction);
        }

        public void OnTurn(bool state)
        {
            //Методы при срабатывании ивента от блока на которого подписались

            switch(_eventAction) 
            {
                case EventAction.Destroy:
                    if (state)
                    {
                        Link.TryUnsubscibe(this);
                        Destroy(this.gameObject);
                    }
                    break;

                case EventAction.Enable:
                    this.gameObject.SetActive(true);
                    break;

                default: break;
            }
        }

        public virtual List<UIComponents> GetUI()
        {
            return new List<UIComponents>(new UIComponents[] { UIComponents.Link, UIComponents.EventAction });
        }
    }

}