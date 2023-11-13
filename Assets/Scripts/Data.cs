using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ZFGinc.Assets.WorldOfCubes
{
    [Serializable]
    public struct BlockDictionary
    {
        public Block Block;
        public Sprite Sprite;
    }

    public class Data : MonoBehaviour
    {
        private readonly string PATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".SwordMasters\\maps\\");
        [SerializeField] private List<BlockDictionary> _blocks;
        [SerializeField] private List<Material> _materials;

        public string MainPath => PATH;

        private void Start()
        {
            SetColorMaterals(Color.white);
        }

        public void SetColorMaterals(Color color)
        {
            foreach (Material mat in _materials)
            {
                mat.SetColor("_Color", color);
            }
        }

        public List<BlockDictionary> Dictionary => _blocks;

        public Block GetBlock(int index) => _blocks[index].Block;
        public Sprite GetBlockSprite(int index) => _blocks[index].Sprite;

        public List<Block> GetListBlocks()
        {
            List<Block> list = new List<Block>();
            foreach (BlockDictionary dict in _blocks)
            {
                list.Add(dict.Block);
            }

            return list;
        }
    }
}