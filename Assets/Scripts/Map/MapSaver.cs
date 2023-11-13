using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;

namespace ZFGinc.Assets.WorldOfCubes
{
    [RequireComponent(typeof(Data))]
    public class MapSaver : MonoBehaviour
    {
        private Data _data;

        private JsonSerializerSettings _settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

        private void Start()
        {
            _data = GetComponent<Data>();

            if (!Directory.Exists(_data.MainPath)) Directory.CreateDirectory(_data.MainPath);
        }

        public void SaveMap(string name, string author, string version, float colorR, float colorG, float colorB)
        {
            List<Block> blocks = new List<Block>(FindObjectsOfType<Block>());

            List<BlockInfo> infos = new List<BlockInfo>();
            MapData mapData = new MapData(name, author, version, colorR, colorG, colorB);

            int i = 0;
            while (i < blocks.Count)
            {
                if (blocks[i].GetType() == typeof(TriggerBlock) ||
                    blocks[i].GetType() == typeof(TriggerWithTimerBlock) ||
                    blocks[i].GetType() == typeof(ValveBlock))
                {
                    List<BlockInfo> appendList = GetInfo(ref blocks, i);
                    infos.AddRange(appendList);

                    continue;
                }
                i++;
            }

            foreach (Block block in blocks)
            {
                if (block == null) continue;
                infos.Add(block.GetInfo());
            }

            mapData.Blocks = infos;

            string json = JsonConvert.SerializeObject(mapData, _settings);
            File.WriteAllText(_data.MainPath + mapData.Name + "_" + mapData.Author + "_" + mapData.Version + ".json", json);
        }

        private List<BlockInfo> GetInfo(ref List<Block> blocks, int index)
        {
            List<BlockInfo> infos = new List<BlockInfo>();

            List<Block> links = ((ILinkable)blocks[index]).Links();

            infos.Add(blocks[index].GetInfo());
            blocks.RemoveAt(index);

            for (int j = 0; j < links.Count; j++) blocks.Remove(links[j]);

            return infos;
        }
    }

}