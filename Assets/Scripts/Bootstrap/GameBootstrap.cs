using UnityEngine;

namespace ZFGinc.Assets.WorldOfCubes.Bootstrap
{
    public class GameBootstrap: MonoBehaviour
    {
        [SerializeField] private Data _data;
        [SerializeField] private MapLoader _mapLoader;
        [SerializeField] private SpawnPlayers _spawnPlayers;
        [SerializeField] private OutOfBounds _outOfBounds;
        [SerializeField] private GameButtons _gameButtons;

        private void Start()
        {
            Time.timeScale = 0;

            _data.Initialization();
            _mapLoader.Initialization();
            _gameButtons.Initialization();
            _spawnPlayers.Initialization();
            _mapLoader.LoadMap();
            _outOfBounds.Initialization(_gameButtons);
            _spawnPlayers.SetSpawnPositions();

            Time.timeScale = 1;
        }
    }
}
