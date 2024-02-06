using System.Collections.Generic;
using UnityEngine;

namespace ZFGinc.WorldOfCubes
{
    [RequireComponent(typeof(MapLoader))]
    public class SpawnPlayers : MonoBehaviour
    {
        [SerializeField] private GameObject _playerObject;

        private Dictionary<Player, int> _players = new Dictionary<Player, int>();
        private MapLoader _mapLoader;

        public void Initialization()
        {
            _mapLoader = GetComponent<MapLoader>();
            _mapLoader.AlertLoad += Prepare;
        }

        public void Prepare()
        {
            _mapLoader.AlertLoad -= Prepare;

            bool isPl0 = PlayerPrefs.GetInt("isPl0", 0) == 1;
            bool isPl1 = PlayerPrefs.GetInt("isPl1", 0) == 1;

            int countPlayers = 0;
            bool[] players = new bool[2] { isPl0, isPl1 };

            for (int j = 0; j < 2; j++)
            {
                if (players[j])
                {
                    countPlayers++;
                }
            }

            SpawnPlayer(countPlayers, players);
        }

        private void SpawnPlayer(int count, bool[] isplayers)
        {
            for (int i = 0; i < count; i++)
            {
                var obj = Instantiate(_playerObject);

                int idPlayer = 0;

                for (int j = 0; j < isplayers.Length; j++)
                {
                    if (isplayers[j])
                    {
                        idPlayer = j;
                        isplayers[j] = false;
                        break;
                    }
                }

                _players.Add(obj.GetComponent<Player>(), idPlayer);
            }
        }

        public void SetSpawnPositions()
        {
            foreach (var player in _players)
            {
                int indexSpawnPosition = Random.Range(0, _mapLoader.SpawnPoint.Count);
                Transform spawnPosition = _mapLoader.SpawnPoint[indexSpawnPosition];

                player.Key.Initialization(player.Value, spawnPosition);
            }
        }
    }
}