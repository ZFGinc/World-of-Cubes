using UnityEngine;
using UnityEngine.SceneManagement;

namespace ZFGinc.Assets.WorldOfCubes
{
    public class LoadScene : MonoBehaviour
    {
        public static LoadScene Instance;

        private const string SITE = "http://worldofcubes.free.nf/";
        private const string SUPPORT = "https://boosty.to/zfginc_official";

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
            OpenURL(SITE);
        }

        public void Support()
        {
            OpenURL(SUPPORT);
        }

        public void OpenURL(string url)
        {
            Application.OpenURL(url);
        }
    }
}