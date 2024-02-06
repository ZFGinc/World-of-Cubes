using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ZFGinc.WorldOfCubes
{
    public struct Status
    {
        private int _code;
        private string _header;
        private string _message;
        private Color _color;
        private bool _result;

        public Status(int code, string header, string message, Color color, bool result)
        {
            _code = code;
            _header = header;
            _message = message;
            _color = color;
            _result = result;
        }

        public bool Result => _result;
        public string Header =>_header+"  "+_code.ToString();
        public string Message =>_message;
        public Color Color => _color;
        
    }

    public class ErrorProtocolUI : MonoBehaviour
    {
        [SerializeField] private GameObject _mainObjectUI;
        [SerializeField] private Image _mainObjectImage;
        [SerializeField] private TMP_Text _header;
        [SerializeField] private TMP_Text _message;

        private readonly IReadOnlyDictionary<int, Status> _reservedStatuses = new Dictionary<int, Status>()
        {
            { 200, new Status(200, "Good!", "Map downloaded successfully", new Color(0, 0.4078431f, 0), true) },
            { 404, new Status(404, "Ooops...", "Map not found", new Color(0.4078431f, 0, 0), false) },
            { 502, new Status(502, "Ooops...", "You do not have an internet connection", new Color(0.4078431f, 0, 0), false) },
            { 400, new Status(400, "Why?", "Please enter your ID before you start downloading", new Color(0.4078431f, 0, 0), false) }
        };

        public bool CheckStatus(int code, string message = "")
        {
            if (_reservedStatuses.ContainsKey(code))
            {
                if (_reservedStatuses.TryGetValue(code, out Status status))
                {
                    ShowStatus(status.Header, status.Message, status.Color);
                    return status.Result;
                }
            }
            else {
                ShowStatus("Ooops...  " + code.ToString(), message, new Color(0.4078431f, 0, 0));
            }

            return false;
        }

        private void ShowStatus(string header, string message, Color color)
        {
            _header.text = header;
            _message.text = message;
            _mainObjectImage.color = color;

            _mainObjectUI.SetActive(true);
        }
    }
}