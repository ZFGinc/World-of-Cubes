using UnityEngine;

namespace ZFGinc.Assets.WorldOfCubes.Bootstrap
{
    public class GameBootstrap: MonoBehaviour
    {
        [SerializeField] private Data _data;
        [SerializeField] private MapLoader _mapLoader;
        [SerializeField] private SpawnPlayers _spawnPlayers;

        private void Start()
        {
            Time.timeScale = 0;

            _data.Initialization();
            _mapLoader.Initialization();
            _spawnPlayers.Initialization();
            _mapLoader.LoadMap();
            _spawnPlayers.SetSpawnPositions();

            Time.timeScale = 1;
        }
    }
}
