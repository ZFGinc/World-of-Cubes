using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ZFGinc.WorldOfCubes
{
    public class ScreenSettings : MonoBehaviour
    {
        [SerializeField] private Toggle _toggleFullScreen;

        private void Start()
        {
            _toggleFullScreen.isOn = PlayerPrefs.GetInt("FullScreen", 1) == 1;
            OnChangeToggleFullScreen();
        }

        public void OnChangeToggleFullScreen()
        {
            Save();
            if (_toggleFullScreen.isOn)
                Screen.SetResolution(1920, 1080, FullScreenMode.ExclusiveFullScreen);
            else
                Screen.SetResolution(800,600, FullScreenMode.Windowed);
        }

        private void Save()
        {
            PlayerPrefs.SetInt("FullScreen", _toggleFullScreen.isOn ? 1 : 0);
        }

        private void OnDestroy()
        {
            Save();
        }

        private void OnDisable()
        {
            Save();
        }
    }
}