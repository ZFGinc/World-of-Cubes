using UnityEngine;
using UnityEngine.SceneManagement;

namespace ZFGinc.Assets.WorldOfCubes
{
    public class LoadScene : MonoBehaviour
    {
        public static LoadScene Instance;

        private const string SITE = "http://worldofcubes.free.nf/";

        private void Start()
        {
            if (Instance != null)
                Destroy(this.gameObject);
            Instance = this;
        }

        public void Load(string name)
        {
            SceneManager.LoadScene(name);
        }

        public void Quit()
        {
            Application.Quit();
        }

        public void ShowMoreContent()
        {
            Application.OpenURL(SITE);
        }
    }
}