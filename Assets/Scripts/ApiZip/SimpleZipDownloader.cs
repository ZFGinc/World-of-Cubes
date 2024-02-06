using UnityEngine;
using System.Collections;
using TMPro;
using System;
using UnityEngine.Networking;
using System.Collections.Generic;

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
        private string _cookie = "__test=";

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
            string tempReturn = "<html><body><script type=\"text/javascript\" src=\"/aes.js\" ></script><script>function toNumbers(d){var e=[];d.replace(/(..)/g,function(d){e.push(parseInt(d,16))});return e}function toHex(){for(var d=[],d=1==arguments.length&&arguments[0].constructor==Array?arguments[0]:arguments,e=\"\",f=0;f<d.length;f++)e+=(16>d[f]?\"0\":\"\")+d[f].toString(16);return e.toLowerCase()}var a=toNumbers(\"f655ba9d09a112d4968c63579db590b4\"),b=toNumbers(\"98344c2eee86c3994890592585b49f80\"),c=toNumbers(\"44f96da75fc3d1bf7d9272945ddff8bb\");document.cookie=\"__test=\"+toHex(slowAES.decrypt(c,2,a,b))+\"; expires=Thu, 31-Dec-37 23:55:55 GMT; path=/\"; location.href=\"http://worldofcubes.free.nf/?i=1\";</script><noscript>This site requires Javascript to work, please enable Javascript in your browser or use a browser with Javascript support</noscript></body></html>";
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
                    tempReturn = www.downloadHandler.text;
                }
            }

            string[] tmp = tempReturn.Split("=toNumbers(\"");
            List<string> newTmp = new List<string>();
            for (int i = 0; i < tmp.Length; i++) newTmp.AddRange(tmp[i].Split("\")"));
            int r = int.Parse(newTmp[7].Split(".decrypt(c,")[1].Split(",a,b))+\";")[0]);
            string hashKey = "";

            JavaScriptExecuter jsExecuter = new JavaScriptExecuter();
            jsExecuter.Init();
            jsExecuter.GetHashKey(newTmp[2], newTmp[4], newTmp[6], r, out hashKey);

            _cookie += hashKey;

            Debug.Log("hash=" + hashKey);

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