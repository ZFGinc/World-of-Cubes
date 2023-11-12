using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public static LoadScene Instance;

    private void Start()
    {
        if(Instance != null)
            Destroy(Instance.gameObject);
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
}
