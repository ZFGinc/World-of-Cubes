using System.Collections.Generic;
using UnityEngine;

namespace ZFGinc.Assets.WorldOfCubes
{

    [SerializeField]
    public interface ILinkable
    {
        public event OnTurn OnTurn;
        public List<Block> Links();
        public bool TrySubscibe(Block block);
        public bool TryUnsubscibe(Block block);
    }

}