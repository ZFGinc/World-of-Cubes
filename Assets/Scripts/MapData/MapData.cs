using System.Collections.Generic;
using System;

namespace ZFGinc.Assets.WorldOfCubes
{
    [Serializable]
    public class MapData
    {
        public string Name { get; }
        public string Author { get; }
        public string Version { get; }
        public List<BlockInfo> Blocks { set; get; }
        public float ColorR { get; }
        public float ColorG { get; }
        public float ColorB { get; }

        public MapData(string name, string author, string version, float colorR, float colorG, float colorB)
        {
            Name = name;
            Author = author;
            Version = version;
            ColorR = colorR;
            ColorG = colorG;
            ColorB = colorB;

            Blocks = new List<BlockInfo>();
        }

        public void AddBlock(BlockInfo block) => Blocks.Add(block);
    }

    [Serializable]
    public class MainInfo
    {
        public string Name { get; }
        public string Author { get; }
        public string Version { get; }
        public float ColorR { get; }
        public float ColorG { get; }
        public float ColorB { get; }
        public List<Map> Maps { set; get; }

        public MainInfo(string name, string author, string version, float colorR, float colorG, float colorB)
        {
            Name = name;
            Author = author;
            Version = version;
            ColorR = colorR;
            ColorG = colorG;
            ColorB = colorB;

            Maps = new List<Map>();
        }
    }

    [Serializable]
    public class Map
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? PreviosId { get; set; } = null;

        public Map(int id, string name, int? previosId = null)
        {
            Id = id;
            Name = name;
            PreviosId = previosId;
        }
    }
}