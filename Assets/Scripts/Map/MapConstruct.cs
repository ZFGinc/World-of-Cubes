using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ZFGinc.WorldOfCubes.UI;

namespace ZFGinc.WorldOfCubes
{
    public enum Instruments
    {
        Paste = 0, Erase, Replace, Null
    }

    [RequireComponent(typeof(MapSaver))]
    [RequireComponent(typeof(MapLoader))]
    [RequireComponent(typeof(Data))]
    [RequireComponent(typeof(BlockSettings))]
    public class MapConstruct : MonoBehaviour
    {
        [Header("Base parameters")]
        [SerializeField] private TMP_InputField _name;
        [SerializeField] private TMP_InputField _author;
        [SerializeField] private TMP_InputField _version;
        [Space]
        [Header("Border block")]
        [SerializeField] private GameObject _baseBlock;
        [Space]
        [Header("Camera")]
        [SerializeField] private FreeFlyCamera _flyCamera;
        [Space]
        [Header("Context menu for add new blocks")]
        [SerializeField] private Transform _context;
        [SerializeField] private GameObject _button;
        [Space]
        [Header("Color parameters")]
        [SerializeField] private Slider _r;
        [SerializeField] private Slider _g;
        [SerializeField] private Slider _b;

        private MapSaver _saver;
        private MapLoader _loader;
        private Data _data;
        private BlockSettings _blockSettings;
        private float _kfc = 1f;
        private int _lastIdBlock = -1;
        private Color _color;

        private Block _currentBlock = null;
        private Instruments _instrument = Instruments.Null;
        private Instruments _lastInstrument = Instruments.Paste;

        public void Initialization()
        {
            _saver = GetComponent<MapSaver>();
            _loader = GetComponent<MapLoader>();
            _data = GetComponent<Data>();
            _blockSettings = GetComponent<BlockSettings>();

            _loader.AlertLoad += SetInformationLoadedMap;

            StartCoroutine(BasePlatform());
            AddBlockButtonsUI();
        }

        void Update()
        {
            if (_flyCamera.GetActive)
            {
                if (_instrument != Instruments.Null)
                {
                    if (_currentBlock != null)
                    {
                        Destroy(_currentBlock.gameObject);
                        _currentBlock = null;
                    }
                    _lastInstrument = _instrument;
                    _instrument = Instruments.Null;
                }
                return;
            }
            if (Input.GetKey(KeyCode.Tab))
            {
                _instrument = _lastInstrument;
            }

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.collider.TryGetComponent(out Block block) || hit.collider.gameObject.tag == "Border")
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        ShowSettingsBlock(block);
                    }

                    Vector3 pos = hit.collider.gameObject.transform.position;

                    switch (_instrument)
                    {
                        case Instruments.Paste:
                            if (_currentBlock == null) return;

                            if (Input.GetMouseButtonDown(1))
                            {
                                _currentBlock.enabled = true;
                                _currentBlock = null;
                                NewBlock(_lastIdBlock);
                                return;
                            }
                            if (Input.GetMouseButtonDown(0))
                            {
                                _currentBlock.transform.Rotate(new Vector3(0, 90, 0));
                            }
                            if (block != null && !block.enabled) return;

                            _currentBlock.transform.position = hit.normal * _kfc + pos;
                            break;
                        case Instruments.Erase:
                            if (_lastIdBlock == -1) return;
                            if (pos.y < 0) return;

                            if (Input.GetMouseButtonDown(1))
                            {
                                Destroy(block.gameObject);
                            }
                            break;
                        case Instruments.Replace:
                            if (_lastIdBlock == -1) return;
                            if (pos.y < 0) return;

                            if (Input.GetMouseButtonDown(1))
                            {
                                Destroy(block.gameObject);
                                var obj = Instantiate(_data.GetBlock(_lastIdBlock));
                                obj.transform.position = pos;
                            }
                            break;
                        default: break;
                    }
                }
            }
        }

        private void ShowSettingsBlock(Block block)
        {
            if (block == null || !block.enabled) return;

            if (block.TryGetComponent(out IClickable clickable))
            {
                List<UIComponents> components = clickable.GetUI();
                _blockSettings.Show(ref block, components);
            }
            else _blockSettings.HideUI();
        }

        public void SetInstrument(int id)
        {
            if (_instrument == (Instruments)id) return;
            _instrument = (Instruments)id;

            switch (_instrument)
            {
                case Instruments.Paste: NewBlock(_lastIdBlock); break;
                case Instruments.Erase: if (_currentBlock != null) Destroy(_currentBlock.gameObject); _currentBlock = null; break;
                case Instruments.Replace: if (_currentBlock != null) Destroy(_currentBlock.gameObject); _currentBlock = null; break;
            }
        }

        private void AddBlockButtonsUI()
        {
            var list = _data.GetListBlocks();
            int[] continues = new int[] { 27 };
            foreach (BlockDictionary block in _data.Dictionary)
            {
                int id = block.Block.GetID();
                if (continues.Contains(id)) continue;

                var obj = Instantiate(_button, Vector3.zero, Quaternion.identity);
                obj.transform.parent = _context;
                obj.GetComponent<Button>().onClick.AddListener(() => NewBlock(id));
                obj.transform.GetChild(0).GetComponent<Image>().sprite = block.Sprite;
            }
        }

        private IEnumerator BasePlatform()
        {
            float startXZ = -24f, y = -_kfc;

            for (int i = 0; i < 64; i++)
            {
                for (int j = 0; j < 64; j++)
                {
                    Instantiate(_baseBlock, new Vector3(startXZ + i * _kfc, y, startXZ + j * _kfc), Quaternion.identity);
                    Instantiate(_baseBlock, new Vector3(startXZ + i * _kfc, j * _kfc, startXZ + 64 * _kfc), Quaternion.identity);
                    Instantiate(_baseBlock, new Vector3(startXZ + 64 * _kfc, j * _kfc, startXZ + i * _kfc), Quaternion.identity);
                }
            }

            yield return null;
        }

        public void UpdateColor()
        {
            _color = ColorConver((int)_r.value, (int)_g.value, (int)_b.value);
            _data.SetColorMaterals(_color);
        }

        public Color ColorConver(int r, int g, int b)
        {
            return new Color(((float)r) / 255, ((float)g) / 255, ((float)b) / 255);
        }

        public float[] ColorConver(float r, float g, float b)
        {
            return new float[3] { r * 255, g * 255, b * 255 };
        }

        public void SaveMap()
        {
            _color = ColorConver((int)_r.value, (int)_g.value, (int)_b.value);
            _saver.SaveMap(_name.text, _author.text, _version.text, _color.r, _color.g, _color.b);
        }

        public void ClearMap()
        {
            _loader.ClearMap();

            _name.text = "";
            _author.text = "";
            _version.text = "";
            _r.value = 255;
            _g.value = 255;
            _b.value = 255;
        }

        public void NewBlock(int id)
        {
            _lastIdBlock = id;

            if (_instrument == Instruments.Erase) return;

            if (_currentBlock != null)
            {
                Destroy(_currentBlock.gameObject);
            }

            _currentBlock = Instantiate(_data.GetBlock(id)).GetComponent<Block>();
            _currentBlock.enabled = false;
        }

        private void SetInformationLoadedMap()
        {
            _name.text = _loader.CurrentMap.Name;
            _author.text = _loader.CurrentMap.Author;
            _version.text = _loader.CurrentMap.Version;
            float[] color = ColorConver(_loader.CurrentMap.ColorR, _loader.CurrentMap.ColorG, _loader.CurrentMap.ColorB);
            _r.value = color[0];
            _g.value = color[1];
            _b.value = color[2];
        }

        public void OpenMapsFolder()
        {
            Process.Start("explorer.exe", _data.MainPath);
        }
    }
}