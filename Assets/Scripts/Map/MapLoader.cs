using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using TMPro;
using System.Collections.Generic;

namespace ZFGinc.WorldOfCubes
{
    public delegate void Alert();

    [RequireComponent(typeof(Data))]
    public class MapLoader : MonoBehaviour
    {
        public string[] AllMaps { get; private set; }
        public MapData CurrentMap { get; private set; }
        public event Alert AlertLoad;
        public List<Transform> SpawnPoint { get; private set; } = new List<Transform>();

        [Header("Игровая сцена или нет")]
        [SerializeField] private bool _isGame = false;
        [Space]
        [Header("Загрузка информации о картах")]
        [SerializeField] private bool _loadChangeList = false;
        [Space]
        [Header("UI для отображдения карт в списке\nЕсли загружается информация о списке доступных карт,\nто эти компонены необходимы")]
        [SerializeField] private GameObject _objectForButtonChangeMap;
        [SerializeField] private Transform _parentForButtonChangeMap;

        private Data _data;

        public static MapLoader Instance;

        public void Initialization()
        {
            _data = GetComponent<Data>();

            Instance = this;

            if (!_isGame) LoadAllListMaps();
        }

        public void LoadMap()
        {
            if (_isGame)
            {
                if (PlayerPrefs.GetString("load_map", "null") != "null")
                {
                    LoadCpecificMap(PlayerPrefs.GetString("load_map"));
                }
            }
        }

        public void ClearMap()
        {
            Block[] blocks = FindObjectsOfType<Block>();

            for (int i = 0; i < blocks.Length; i++)
            {
                Destroy(blocks[i].gameObject);
            }
        }

        public void LoadCpecificMap(string path)
        {
            ClearMap();

            var json = File.ReadAllText(path);
            CurrentMap = JsonConvert.DeserializeObject<MapData>(json, _data.JSONSettings);

            MapConstruct(CurrentMap);
            SetActiveFalseForEnableActionBlocks();
            AlertLoad();
        }

        private void SetActiveFalseForEnableActionBlocks()
        {
            var blocks = FindObjectsOfType<Block>();
            foreach(Block block in blocks)
            {
                if (block.GetEventAction() == EventAction.Enable) block.gameObject.SetActive(false);
            }
        }

        public void LoadAllListMaps()
        {
            AllMaps = Directory.GetFiles(_data.MainPath);

            ShowListMaps();
        }

        private void ShowListMaps()
        {
            if (!_loadChangeList) return;

            while (_parentForButtonChangeMap.childCount > 0)
                Destroy(_parentForButtonChangeMap.GetChild(0));

            foreach (string file in AllMaps)
            {
                string name_map = Path.GetFileName(file);

                var obj = Instantiate(_objectForButtonChangeMap);
                obj.transform.SetParent(_parentForButtonChangeMap, false);

                obj.GetComponent<Button>().onClick.AddListener(delegate () { LoadCpecificMap(file); });
                obj.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = name_map;
            }
        }

        private void MapConstruct(MapData map)
        {
            _data.SetColorMaterals(new Color(map.ColorR, map.ColorG, map.ColorB));

            foreach (BlockInfo info in map.Blocks)
            {
                var obj = Instantiate(_data.GetBlock(info.IdBlock));

                obj.SetInfo(info);

                if (info.IdBlock == 0) SpawnPoint.Add(obj.transform);

                obj.transform.parent = transform;
            }
        }

        public static void LoadLinkedBlocks<T>(List<BlockInfo> info, T parrent) where T : ILinkable
        {
            foreach (BlockInfo inf in info)
            {
                Block block = Instantiate(Instance._data.GetBlock(inf.IdBlock));
                block.SetInfo(inf);
                parrent.TrySubscibe(block);

                if (inf.IdBlock == 0) Instance.SpawnPoint.Add(block.transform);

                block.transform.parent = Instance.transform;
            }
        }
    }
}