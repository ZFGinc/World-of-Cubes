using UnityEngine;

namespace ZFGinc.Assets.WorldOfCubes
{
    public class Selecter : MonoBehaviour
    {
        [SerializeField] private SkinSelect[] _players;
        [SerializeField] private bool _isAnySelected;
        [SerializeField] private bool isAllofSelectedReady;

        private LoadScene _loader;

        public void Initialization(LoadScene loader)
        {
            _loader = loader;
        }

        private void FixedUpdate()
        {
            StartGame();
        }

        public void StartGame()
        {
            bool isAnySelected = AnySelected();
            bool isAllofSelectedReady = AllofSelectedReady();

            if (!isAnySelected) return;
            if (!isAllofSelectedReady) return;

            for (int i = 0; i < _players.Length; i++)
                _players[i].Save();
            _loader.Load("play");
        }

        private bool AllofSelectedReady()
        {
            for (int i = 0; i < _players.Length; i++)
            {
                if (_players[i].IsSelected && !_players[i].IsReady)
                {
                    return false;
                }
            }

            return true;
        }

        private bool AnySelected()
        {
            for (int i = 0; i < _players.Length; i++)
            {
                if (_players[i].IsSelected)
                {
                    return true;
                }
            }

            return false;
        }
    }
}