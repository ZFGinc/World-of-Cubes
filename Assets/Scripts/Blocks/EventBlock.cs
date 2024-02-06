using System.Collections.Generic;

namespace ZFGinc.WorldOfCubes
{

    public class EventBlock : Block, ILinkable
    {
        public new event OnTurn OnTurn;
        protected List<Block> _linkedBlocks = new List<Block>();

        public List<Block> Links() => _linkedBlocks;

        public bool TrySubscibe(Block block)
        {
            if (_linkedBlocks.Contains(block))
                return false;

            OnTurn += block.OnTurn;
            _linkedBlocks.Add(block);
            block.Link = this;

            return true;
        }

        public bool TryUnsubscibe(Block block)
        {
            if (!_linkedBlocks.Contains(block))
                return false;

            OnTurn -= block.OnTurn;
            _linkedBlocks.Remove(block);
            block.Link = null;

            return true;
        }

        protected bool HasSubsritions()
        {
            if (OnTurn == null) return false;
            if (OnTurn.GetInvocationList().Length == 0) return false;

            return true;
        }

        private void RemoveNillReferceBlocks()
        {
            int i = 0;
            while(i < _linkedBlocks.Count)
            {
                if (_linkedBlocks[i] == null)
                {
                    _linkedBlocks.RemoveAt(i);
                    continue;
                }
                i++;
            }
        }

        protected void InvokeAll(bool state)
        {
            if (!HasSubsritions()) return;

            foreach (OnTurn func in OnTurn.GetInvocationList())
            {
                if (!HasSubsritions()) return;
                if (func == null) continue;
                func.Invoke(state);
            }

            RemoveNillReferceBlocks();
        }
    }

}