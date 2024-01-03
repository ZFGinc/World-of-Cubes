using System.IO;
using Unity.VisualScripting;
using UnityEngine;

namespace ZFGinc.Assets.WorldOfCubes
{
    public class MapList : MonoBehaviour
    {
        [SerializeField] private MainInfo _mainInfo;
        [SerializeField] private int _currentMapIndex = 0;
        [SerializeField] private string _path;

        public static MapList Instance;

        private bool _infoLoaded = false;

        private void Start()
        {
            if(Instance != null) Destroy(this.gameObject);
            Instance = this;

            DontDestroyOnLoad(this);
        }

        public void LoadInfo(MainInfo mainInfo, int currentMapIndex, string path)
        {
            _mainInfo = mainInfo;
            _currentMapIndex = currentMapIndex;
            _path = path;
            _infoLoaded = true;
        }

        public bool NextMap()
        {
            if (!_infoLoaded) return false;
            if(_currentMapIndex+1 >= _mainInfo.Maps.Count) return false;

            Map map = _mainInfo.Maps[_currentMapIndex+1];
            string path = _path + "\\" + _mainInfo.Name + "\\maps\\" + map.Name;

            PlayerPrefs.SetString("load_map", path);
            return true;
        }

        public void Next()
        {
            _currentMapIndex++;
        }
    }
}