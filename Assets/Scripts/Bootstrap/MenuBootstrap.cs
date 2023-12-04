using UnityEngine;

namespace ZFGinc.Assets.WorldOfCubes.Bootstrap
{
    public class MenuBootstrap: MonoBehaviour
    {
        [SerializeField] private Data _data;
        [SerializeField] private LoadScene _loadScene;
        [SerializeField] private MainInfoLoader _mainInfoLoader;
        [SerializeField] private Selecter _selecter;
        [SerializeField] private SkinSelect skinSelect_1;
        [SerializeField] private SkinSelect skinSelect_2;

        private void Start()
        {
            _data.Initialization();
            _mainInfoLoader.Initialization();
            _selecter.Initialization(_loadScene);
            skinSelect_1.Initialization();
            skinSelect_2.Initialization();
        }
    }
}
