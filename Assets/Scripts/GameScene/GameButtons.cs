using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ZFGinc.WorldOfCubes {

    public class GameButtons : MonoBehaviour
    {
        [SerializeField] private GameObject _menuObject;
        [SerializeField] private GameObject _endPanel;
        [SerializeField] private GameObject _nextMapButton;
        [SerializeField] private TMP_Text _about;

        private LoadScene _loadScene;
        private bool _isPaused = false;
        private bool _isEndLevel = false;
        private bool _isHasNextMap;

        public void Initialization()
        {
            _loadScene = GetComponent<LoadScene>();

            string author = MapList.Instance.GetAboutCurrentMap().Item1;
            string name = MapList.Instance.GetAboutCurrentMap().Item2;
            _about.text = author + "\n" + name;
        }

        public void EndLevel()
        {
            _endPanel.SetActive(true);
            _isEndLevel = true;
            List<Player> players = new List<Player>(FindObjectsOfType<Player>());
            foreach (Player p in players)
            {
                p.SetZeroSpeed();
            }

            _isHasNextMap = MapList.Instance.NextMap();
            _nextMapButton.SetActive(_isHasNextMap);
        }

        private void Update ()
        {
            if (Input.GetKeyUp(KeyCode.Escape) || Hinput.anyGamepad.start.justPressed)
            {
                PauseGame();
            }
        }

        public void NextMap()
        {
            if (!_isEndLevel) return;

            if (_isHasNextMap)
            {
                MapList.Instance.Next();
                _loadScene.Load("play");
            }
        }

        public void PauseGame()
        {
            if (_isEndLevel) return;

            _isPaused = !_isPaused;
            _menuObject.SetActive(_isPaused);
            Time.timeScale = _isPaused ? 0f : 1f;
        }

        public void Restart()
        {
            Time.timeScale = 1f;
            _loadScene.Load("play");
        }

        public void Menu () 
        {
            Time.timeScale = 1f;
            _loadScene.Load("menu");
        }
    }
}
