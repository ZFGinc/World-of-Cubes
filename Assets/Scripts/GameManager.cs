using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

namespace ZFGinc.Assets.WorldOfCubes
{
    [RequireComponent(typeof(MapLoader))]
    [RequireComponent(typeof(NavMeshSurface))]
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private List<Player> _players = new List<Player>();
        [SerializeField] private GameObject _playerObject;

        private MapLoader _mapLoader;
        public bool gameIsLoading = false;

        private void Start()
        {
            _mapLoader = GetComponent<MapLoader>();

            _mapLoader.Alert += StartGame;
        }

        public int GetCountPlayers() => _players.Count;
        public Vector3 GetPositionPlayer(int playerIndex) => _players[playerIndex].gameObject.transform.position;
        public Vector3[] GetPositionsAllPlayers()
        {
            Vector3[] positions = new Vector3[_players.Count];
            for (int i = 0; i < _players.Count; i++)
                positions[i] = _players[i].transform.position;

            return positions;
        }

        public void StartGame()
        {
            bool isPl0 = PlayerPrefs.GetInt("isPl0", 0) == 1;
            bool isPl1 = PlayerPrefs.GetInt("isPl1", 0) == 1;
            bool isPl2 = PlayerPrefs.GetInt("isPl2", 0) == 1;
            bool isPl3 = PlayerPrefs.GetInt("isPl3", 0) == 1;

            int countPlayers = 0;
            bool[] players = new bool[] { isPl0, isPl1, isPl2, isPl3 };

            for (int j = 0; j < 4; j++)
            {
                if (players[j])
                {
                    countPlayers++;
                }
            }

            SpawnPlayers(countPlayers, _mapLoader.SpawnPoint, players);
        }

        public void SpawnPlayers(int count, List<Transform> spawnPosition, bool[] isplayers)
        {
            for (int i = 0; i < count; i++)
            {
                var obj = Instantiate(_playerObject);
                Player player = obj.GetComponent<Player>();
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

                player.SetNumber(idPlayer);
                _players.Add(player);

                if (spawnPosition.Count > 0)
                    obj.transform.position = spawnPosition[i].position + Vector3.up * 3;
                else
                    obj.transform.position = spawnPosition[0].position + Vector3.up * 3;
            }

            gameIsLoading = true;
        }
    }
}