using UnityEngine;

namespace ZFGinc.Assets.WorldOfCubes {

    public class GameButtons : MonoBehaviour
    {
        [SerializeField] private GameObject _menuObject; 

        private LoadScene _loadScene;
        private bool _isPaused = false;

        private void Start ()
        {
            _loadScene = GetComponent<LoadScene>();
        }

        private void Update ()
        {
            if(Input.GetKeyUp(KeyCode.Escape))
            {
                PauseGame();
            }
        }

        public void PauseGame()
        {
            _isPaused = !_isPaused;
            _menuObject.SetActive(_isPaused);
            Time.timeScale = _isPaused ? 0f : 1f;
        }

        public void Restart()
        {
            _loadScene.Load("play");
        }

        public void Menu () 
        {
            _loadScene.Load("menu");
        }
    }
}
