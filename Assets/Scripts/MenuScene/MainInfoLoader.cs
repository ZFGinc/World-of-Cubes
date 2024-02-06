using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using TMPro;
using DanielLochner.Assets.SimpleScrollSnap;

namespace ZFGinc.WorldOfCubes
{
    [RequireComponent(typeof(Data))]
    public class MainInfoLoader : MonoBehaviour
    {
        [Header("UI куб для отображения названия")]
        [SerializeField] private GameObject _prefabCubeUI;
        [Header("UI куб для автоматического скачивания карт")]
        [SerializeField] private GameObject _prefabAutoDownloader;
        [Header("Где хранятся все ветки прохождения")]
        [SerializeField] private Transform _parentListInfos;
        [Header("Где хранятся карты из отдельной ветки прохождения")]
        [SerializeField] private Transform _parentListMaps;
        [Header("UI для вводе индекса карты на сайте")]
        [SerializeField] private GameObject _autoDownloaderUI;
        [Header("UI для выбора уровня и игроков")]
        [SerializeField] private GameObject _playerChangeUI;

        [Space]
        [Header("SnapScrolling веток прохождения")]
        [SerializeField] private GameObject _listInfos;
        [Header("SnapScrolling карт")]
        [SerializeField] private GameObject _listMaps;

        private List<MainInfo> _mainInfo;
        private List<string> _allMaps;
        private Data _data;

        public bool IsMapSelected { get; private set; } = false;

        public void Initialization()
        {
            _data = GetComponent<Data>();
            LoadAllListMaps();
        }


        public void LoadAllListMaps()
        {
            _allMaps = new List<string>();
            string[] list = Directory.GetDirectories(_data.MainPath);

            foreach(string s in list)
            {
                if (Directory.Exists(s+"\\maps") && File.Exists(s+"\\maininfo.json"))
                { 
                    _allMaps.Add(s);
                }
            }

            AddNewMainInfo();
            ShowListInfo();
        }

        private void AddNewMainInfo()
        {
            _mainInfo = new List<MainInfo>();

            foreach (string file in _allMaps)
            {
                string mainInfoPath = file + "\\maininfo.json";

                var json = File.ReadAllText(mainInfoPath);
                MainInfo newMainInfo = JsonConvert.DeserializeObject<MainInfo>(json);

                _mainInfo.Add(newMainInfo);
            }
        }

        private void ShowListInfo()
        {
            _listInfos.SetActive(false);
            ClearParent(_listInfos.GetComponent<SimpleScrollSnap>());

            ShowResourcesMaps();

            for (int i = 0; i < _mainInfo.Count; i++)
            {
                MainInfo mi = _mainInfo[i];
                string name = mi.Name;
                

                var obj = Instantiate(_prefabCubeUI);
                obj.transform.SetParent(_parentListInfos, false);

                obj.GetComponent<Button>().onClick.AddListener(delegate () { ShowListMaps(mi, _data.MainPath); });
                obj.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = mi.Name;

                Color color = new Color(mi.ColorR, mi.ColorG, mi.ColorB);
                obj.GetComponent<Image>().color = color;
            }

            var endAutoDownloader = Instantiate(_prefabAutoDownloader);
            endAutoDownloader.transform.SetParent(_parentListInfos, false);
            endAutoDownloader.GetComponent<Button>().onClick.AddListener(delegate () { _autoDownloaderUI.SetActive(true); _playerChangeUI.SetActive(false); });

            _listInfos.SetActive(true);
        }

        private void ShowResourcesMaps()
        {
            #if UNITY_EDITOR
            string path = "Assets/Resources";
            #elif UNITY_STANDALONE
            string path = "World of Cubes_Data/Resources";
            #endif

            var json = Resources.Load<TextAsset>("Lessons/maininfo").ToString();

            MainInfo mi = JsonConvert.DeserializeObject<MainInfo>(json);

            string name = mi.Name;

            var obj = Instantiate(_prefabCubeUI);
            obj.transform.SetParent(_parentListInfos, false);

            obj.GetComponent<Button>().onClick.AddListener(delegate () { ShowListMaps(mi, path); });
            obj.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = mi.Name;

            Color color = new Color(mi.ColorR, mi.ColorG, mi.ColorB);
            obj.GetComponent<Image>().color = color;
        }

        private void ShowListMaps(MainInfo mi, string basePath) 
        {
            _listMaps.SetActive(false);
            ClearParent(_listMaps.GetComponent<SimpleScrollSnap>());

            for(int i = 0; i < mi.Maps.Count; i++) 
            {
                Map map = mi.Maps[i];
                string path = basePath + "\\" + mi.Name + "\\maps\\"+ map.Name;
                string name = Path.GetFileName(map.Name);
                name = name.Split(".")[0];

                var obj = Instantiate(_prefabCubeUI);
                obj.transform.SetParent(_parentListMaps, false);

                obj.GetComponent<Button>().onClick.AddListener(delegate () { SelectMap(mi, basePath, path, map); });
                obj.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = name;
            }

            _listInfos.SetActive(false);
            _listMaps.SetActive(true);
        }

        private void ClearParent(SimpleScrollSnap scrollSnap)
        {
            if (scrollSnap.NumberOfPanels == 0) return;

            while(scrollSnap.NumberOfPanels > 0)
            {
                Destroy(scrollSnap.Panels[0].gameObject);
                scrollSnap.Remove(0);
            }
        }

        private void SelectMap(MainInfo mi, string basePath, string path, Map map)
        {
            PlayerPrefs.SetString("load_map", path);
            int index = mi.Maps.IndexOf(map);
            MapList.Instance.LoadInfo(mi, index, basePath);

            IsMapSelected = true;
        }
    }
}