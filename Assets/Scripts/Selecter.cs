using UnityEngine;

[RequireComponent (typeof(LoadScene))]
public class Selecter : MonoBehaviour
{
    [SerializeField] private SkinSelect[] _players;

    private LoadScene _loader;

    private void Start()
    {
        _loader = GetComponent<LoadScene>();
    }

    private void Update()
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
