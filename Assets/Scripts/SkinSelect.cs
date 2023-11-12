using UnityEngine;

public class SkinSelect : MonoBehaviour
{
    [SerializeField] private int _idPlayers;
    [Space] 
    [SerializeField] private GameObject _uiSelectNewPlayer;
    [SerializeField] private GameObject _uiDiscardPlayer;
    [SerializeField] private GameObject _uiSelectReadiness;
    [Space]
    [SerializeField] private GameObject _objectPlayers;

    public bool IsSelected { get; private set; } = false;
    public bool IsReady { get; private set; } = false;

    private void Start()
    {
        PlayerPrefs.SetInt("isPl" + _idPlayers.ToString(), 0);
    }

    private void Update()
    {
        SelectNewPlayers();
    }

    private void SelectNewPlayers()
    {
        if (_idPlayers == 0)
        {
            IsReady = Input.GetKeyDown(KeyCode.Space) && IsSelected;

            if (Input.GetKey(KeyCode.Return)) IsSelected = true; 
            if (Input.GetKey(KeyCode.Backspace)) IsSelected = false;    
        }
        else
        {
            IsReady = Hinput.gamepad[_idPlayers - 1].Y && IsSelected;

            if (Hinput.gamepad[_idPlayers - 1].A) IsSelected = true;
            if (Hinput.gamepad[_idPlayers - 1].B) IsSelected = false;
        }

        _objectPlayers.SetActive(IsSelected);
        _uiSelectReadiness.SetActive(IsSelected);
        _uiSelectReadiness.transform.GetChild(_uiSelectReadiness.transform.childCount-1).gameObject.SetActive(!IsReady);
        _uiSelectNewPlayer.SetActive(!IsSelected);
        _uiDiscardPlayer.SetActive(IsSelected);
    }

    public void Save()
    {
        if (!IsSelected) return;

        PlayerPrefs.SetInt("isPl"+_idPlayers.ToString(), 1);
    }
}
