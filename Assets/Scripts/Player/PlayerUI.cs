using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ZFGinc.Assets.WorldOfCubes
{
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _numberPlayer;
        [SerializeField] private Color[] _colors;

        public void SetNumber(int number)
        {
            _numberPlayer.text = "P" + (number + 1).ToString();
            _numberPlayer.color = _colors[number];
        }
    }
}