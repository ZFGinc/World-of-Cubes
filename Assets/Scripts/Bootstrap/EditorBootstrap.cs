using UnityEngine;

namespace ZFGinc.Assets.WorldOfCubes.Bootstrap
{
    public class EditorBootstrap: MonoBehaviour
    {
        [SerializeField] private Data _data;
        [SerializeField] private MapLoader _mapLoader;
        [SerializeField] private MapSaver _mapSaver;
        [SerializeField] private MapConstruct _mapConstruct;

        private void Start()
        {
            _data.Initialization();
            _mapLoader.Initialization();
            _mapSaver.Initialization();
            _mapConstruct.Initialization();
        }
    }
}
