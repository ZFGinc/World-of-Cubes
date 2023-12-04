using UnityEngine;

namespace ZFGinc.Assets.WorldOfCubes
{
    public class SkinSelect : MonoBehaviour
    {
        [SerializeField] private int _idPlayers;
        [SerializeField] private int _skinIndex;
        [Space]
        [SerializeField] private GameObject _uiSelectNewPlayer;
        [SerializeField] private GameObject _uiDiscardPlayer;
        [SerializeField] private GameObject _uiSelectReadiness;
        [Space]
        [SerializeField] private Transform _skinParent;

        public bool IsSelected { get; private set; } = false;
        public bool IsReady { get; private set; } = false;

        private int _countSkins = 1;

        public void Initialization()
        {
            PlayerPrefs.SetInt("isPl" + _idPlayers.ToString(), 0);
            _countSkins = _skinParent.childCount;
        }

        private void Update()
        {
            SelectNewPlayers();
        }

        private void SelectNewPlayers()
        {
            if (_idPlayers == 0)
            {
                IsReady = Input.GetKey(KeyCode.Space) && IsSelected;

                if (Input.GetKeyUp(KeyCode.Return)) IsSelected = true;
                if (Input.GetKeyDown(KeyCode.Backspace)) IsSelected = false;

                if (Input.GetKeyDown(KeyCode.R))
                {
                    _skinIndex++;

                    if( _skinIndex == _countSkins ) _skinIndex = 0;

                    ShowSkin();
                }
            }
            else
            {
                IsReady = Hinput.gamepad[_idPlayers - 1].Y && IsSelected;

                if (Hinput.gamepad[_idPlayers - 1].A) IsSelected = true;
                if (Hinput.gamepad[_idPlayers - 1].B) IsSelected = false;

                if (Hinput.gamepad[_idPlayers - 1].X)
                {
                    _skinIndex++;

                    if (_skinIndex == _countSkins) _skinIndex = 0;
                    
                    ShowSkin();
                }
            }

            _skinParent.gameObject.SetActive(IsSelected);
            _uiSelectReadiness.SetActive(IsSelected);
            _uiSelectReadiness.transform.GetChild(_uiSelectReadiness.transform.childCount - 1).gameObject.SetActive(!IsReady);
            _uiSelectNewPlayer.SetActive(!IsSelected);
            _uiDiscardPlayer.SetActive(IsSelected);
        }

        private void ShowSkin()
        {
            //disable all skins//
            for(int i = 0; i < _countSkins; i++) _skinParent.GetChild(i).gameObject.SetActive(false);

            //enable changed skin//
            _skinParent.GetChild(_skinIndex).gameObject.SetActive(true);
        }

        public void Save()
        {
            if (!IsSelected) return;

            PlayerPrefs.SetInt("isPl" + _idPlayers.ToString(), 1);
            PlayerPrefs.SetInt("skin_" + _idPlayers.ToString(), _skinIndex);
        }
    }
}