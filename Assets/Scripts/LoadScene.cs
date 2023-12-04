using UnityEngine;
using UnityEngine.SceneManagement;

namespace ZFGinc.Assets.WorldOfCubes
{
    public class LoadScene : MonoBehaviour
    {
        public static LoadScene Instance;

        private void Start()
        {
            if (Instance != null)
                Destroy(this.gameObject);
            Instance = this;
        }

        public void Load(string name)
        {
            Application.LoadLevel(name);
            //SceneManager.LoadScene(name);
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}