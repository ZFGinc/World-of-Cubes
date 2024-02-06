using UnityEngine;
using System.Collections;
using TMPro;
using System;
using UnityEngine.Networking;

namespace ZFGinc.WorldOfCubes
{
    public class CertificateWhore : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            return true;
        }
    }

    [RequireComponent(typeof(MainInfoLoader))]
    [RequireComponent(typeof(ErrorProtocolUI))]
    [RequireComponent(typeof(Data))]
    public class SimpleZipDownloader : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _inputIdMap;
        [SerializeField] private GameObject _loadingScreen;

        private Data _data;
        private MainInfoLoader _mainInfoLoader;
        private ErrorProtocolUI _errorProtocolUI;


        private int _statusCode = 0;
        private string _statusMessage = "";

        //For web requests
        //<!>
        private string _cookie = "";

        private const string BUM = "http://193.124.9.14:8001/";
        private const string BASE = "http://worldofcubes.free.nf/";
        private const string URL_GET_ZIP_URL = "http://worldofcubes.free.nf/api/?id=";
        private const string USER_AGENT = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.5112.79 Safari/537.36";

        //<!>

        [Obsolete]
        private void Start()
        {
            _data = GetComponent<Data>();
            _mainInfoLoader = GetComponent<MainInfoLoader>();
            _errorProtocolUI = GetComponent<ErrorProtocolUI>();

            StartCoroutine(SetActualCookie());
        }

        [Obsolete]
        public void DownloadMap()
        {
            _loadingScreen.SetActive(true);
            StartDownload(_inputIdMap.text);
        }

        [Obsolete]
        private void StartDownload(string id)
        {
            if (id == "" || id == string.Empty)
            {
                _statusCode = 400;
                OnDownloadDone();
            }
            else
            {
                StartCoroutine(Download(URL_GET_ZIP_URL + id, OnDownloadDone));
            }
        }

        private void OnDownloadDone()
        {
            _loadingScreen.SetActive(false);
            if (_errorProtocolUI.CheckStatus(_statusCode, _statusMessage))
                _mainInfoLoader.LoadAllListMaps();
        }

        [Obsolete]
        private IEnumerator SetActualCookie()
        {
            string temp = "";
            using (UnityWebRequest www = UnityWebRequest.Get(BASE))
            {
                www.SetRequestHeader("user-agent", USER_AGENT);
                www.certificateHandler = new CertificateWhore();

                yield return www.SendWebRequest();

                if (www.isNetworkError)
                {
                    _statusCode = 503;
                    _statusMessage = www.error;
                    OnDownloadDone();
                    yield break;
                }

                if (www.downloadHandler.isDone)
                {
                    temp = www.downloadHandler.text;
                }
            }

            using (UnityWebRequest www = UnityWebRequest.Post(BUM, temp, "text/html"))
            {
                www.SetRequestHeader("user-agent", USER_AGENT);
                www.certificateHandler = new CertificateWhore();

                yield return www.SendWebRequest();

                if (www.isNetworkError)
                {
                    _statusCode = 503;
                    _statusMessage = www.error;
                    OnDownloadDone();
                    yield break;
                }

                if (www.downloadHandler.isDone)
                {
                    _cookie += www.downloadHandler.text;
                }
            }
        }

        [Obsolete]
        IEnumerator Download(string url, Alert onFinish)
        {
            string newUrl = "";

            using (UnityWebRequest wwwPath = UnityWebRequest.Get(url))
            {
                wwwPath.SetRequestHeader("user-agent", USER_AGENT);
                wwwPath.SetRequestHeader("Cookie", _cookie);
                wwwPath.certificateHandler = new CertificateWhore();

                yield return wwwPath.SendWebRequest();

                if (wwwPath.isNetworkError)
                {
                    _statusCode = 502;
                    goto endCorotine;
                }

                if (wwwPath.downloadHandler.isDone)
                {

                    newUrl = wwwPath.downloadHandler.text;
                }
            }

            using (UnityWebRequest www = UnityWebRequest.Get(newUrl))
            {
                www.SetRequestHeader("user-agent", USER_AGENT);
                www.SetRequestHeader("Cookie", _cookie);
                www.certificateHandler = new CertificateWhore();

                yield return www.SendWebRequest();

                if (www.isNetworkError)
                {
                    _statusCode = 502;
                    _statusMessage = www.error;
                    goto endCorotine;
                }

                byte[] data = www.downloadHandler.data;
                ZipFile.UnZip(_data.MainPath, data);
                _statusCode = 200;
            }

        endCorotine:
            if (onFinish != null)
            {
                onFinish();
            }
        }
    }
}