using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using TMPro;
using DanielLochner.Assets.SimpleScrollSnap;

namespace ZFGinc.Assets.WorldOfCubes
{
    [RequireComponent(typeof(Data))]
    public class MainInfoLoader : MonoBehaviour
    {
        [Header("UI куб дл€ отображени€ названи€")]
        [SerializeField] private GameObject _prefabCubeUI;
        [Header("√де хран€тс€ все ветки прохождени€")]
        [SerializeField] private Transform _parentListInfos;
        [Header("√де хран€тс€ карты из отдельной ветки прохождени€")]
        [SerializeField] private Transform _parentListMaps;
        [SerializeField] private SimpleScrollSnap _scrollSnap;

        [Space]
        [Header("SnapScrolling веток прохождени€")]
        [SerializeField] private GameObject _listInfos;
        [Header("SnapScrolling карт")]
        [SerializeField] private GameObject _listMaps;

        private List<MainInfo> _mainInfo;
        private List<string> _allMaps;
        private Data _data;

        void Start()
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
                if (Directory.Exists(s) && File.Exists(s+"\\maininfo.json"))
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
            for(int i = 0; i < _mainInfo.Count; i++)
            {
                MainInfo mi = _mainInfo[i];
                string name = mi.Name;
                

                var obj = Instantiate(_prefabCubeUI);
                obj.transform.SetParent(_parentListInfos, false);

                obj.GetComponent<Button>().onClick.AddListener(delegate () { ShowListMaps(mi); });
                obj.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = mi.Name;

                Color color = new Color(mi.ColorR, mi.ColorG, mi.ColorB);
                obj.GetComponent<Image>().color = color;
            }
        }

        private void ShowListMaps(MainInfo mi) 
        {
            _listMaps.SetActive(false);
            ClearParent();

            foreach (Map map in mi.Maps)
            {
                string path = Path.GetFullPath(map.Name);
                string name = Path.GetFileName(map.Name);

                var obj = Instantiate(_prefabCubeUI);
                obj.name = "zaglotys";
                obj.transform.SetParent(_parentListMaps, false);

                obj.GetComponent<Button>().onClick.AddListener(delegate () { SelectMap(path); });
                obj.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = name;
            }

            _listInfos.SetActive(false);
            _listMaps.SetActive(true);
        }

        private void ClearParent()
        {
            if (_scrollSnap.NumberOfPanels == 0) return;

            while(_scrollSnap.NumberOfPanels > 0)
            {
                Destroy(_scrollSnap.Panels[0].gameObject);
                _scrollSnap.Remove(0);
            }
        }

        private void SelectMap(string path)
        {
            PlayerPrefs.SetString("load_map", path);
        }
    }
}