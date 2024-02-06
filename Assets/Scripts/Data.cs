using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ZFGinc.WorldOfCubes
{
    [Serializable]
    public struct BlockDictionary
    {
        public Block Block;
        public Sprite Sprite;
    }

    public class Data : MonoBehaviour
    {
        private readonly string PATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".WorldOfCubes\\maps\\");
        private readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

        [SerializeField] private List<BlockDictionary> _blocks;
        [SerializeField] private List<Material> _materials;

        public string MainPath { get { return PATH; } }
        public JsonSerializerSettings JSONSettings => JsonSerializerSettings;

        public void Initialization()
        {
            SetColorMaterals(Color.white);

            if (!Directory.Exists(MainPath)) Directory.CreateDirectory(MainPath);
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