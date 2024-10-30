using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

namespace BuildSystem
{



    public class BuilderManager : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private GameObject _holoHolder;
        [SerializeField] private GameObject _VisualHoldRef;
        [SerializeField] private GameObject _rotationRef;
        [SerializeField] private GameObject _ArrowsRef;
        [SerializeField] private TextMeshPro _ErrorTextRef;
        [SerializeField] private LimitBuild _LimitSize;

        [Header("Visual Config")]
        [SerializeField] private List<ErrorData> _errorTypes;
        [SerializeField] private float _textShowTime;
        [SerializeField] private float _lerpAmount;
        [SerializeField] private Color _nonBlockColor;
        [SerializeField] private Color _blockColor;
        [SerializeField] private Vector3 _gridMouseAdjustment;
        [SerializeField] private AnimationCurve _visualRampUp;
        [SerializeField] private float _RampUpSpeed;
        [SerializeField] private Vector3 _rotationVisualAdjustment;

        [Header("Config")]
        [SerializeField] private LayerMask _DetectionMask;
        [SerializeField] private int _PlacedHoloMaskID;
        [SerializeField] private Vector2 _DestroyDetectionBoxSize = new Vector2(1,1);
        [SerializeField] private LayerMask _BuildingMask;
        [SerializeField] private Sprite _BoxDestroySprite;
        [SerializeField] private float _BuildInterval;


        [Header("debug")]
        [SerializeField] private SpriteRenderer _spriteRefs;
        [SerializeField] private BuildData _dataRefs;

        [SerializeField] private List<BuildingData> _current_Builds_Refs;
        [SerializeField] private List<Materials> _current_Prices_Gross_Code;
        [SerializeField] private List<Materials> _current_Prices_Gross_Visual;

        [SerializeField] private Grid _grid;

        [SerializeField] private Vector2 _ColSize;
        [SerializeField] private Vector2 _ColOffset;

        [SerializeField] private int _buildSelectorID;
        [SerializeField] private List<BuildingData> _empty_Builds_Refs;

        [SerializeField] private int _enable;
        [SerializeField] private bool _destruction;

        [SerializeField] private List<BuildingData> _selectedToDemolish;
        [SerializeField] private List<BuildingData> _selectedToDemolish_Empty;

        [SerializeField] private TextMeshPro _text;


        private BoxCollider2D _colRefDest;
        private BuildingBase _buildRefDest;

        private bool _lockUI;

        public static BuildManagerUI UIManager;

        private IEnumerator _error;

        private float _limitsize;

        [SerializeField] private List<SaveData> _SaveList = new();
        



        void Start()
        {
            _grid = GetComponent<Grid>();
            _text = _rotationRef.GetComponentInChildren<TextMeshPro>();
            _rotationRef.SetActive(false);
            BuildManagerUI.Manager = this;
            UIManager.SwichMode(false);
            _spriteRefs = _holoHolder.GetComponent<SpriteRenderer>();
            _spriteRefs.color = new Color(0, 0, 0, 0);
            _limitsize = GameManager.Instance._limitsize;
            foreach (var item in GameManager.Instance.Resources)
            {
                Materials holder = new() { Type = item.Type, };
                Materials holder2 = new() { Type = item.Type, };

                _current_Prices_Gross_Code.Add(holder);
                _current_Prices_Gross_Visual.Add(holder2);
            }
            _ErrorTextRef.gameObject.SetActive(false);
        }


        void Update()
        {

            ScreenControls();

            if (_limitsize != GameManager.Instance._limitsize)
                _limitsize = GameManager.Instance._limitsize;


            if (_lockUI)
                return;

            if (_enable == 1 && _destruction == false)
                ConstructionInputs();
            else
                DestructionInputs();




        }

        public void ScreenControls()
        {

            if (Input.GetKeyDown(KeyCode.B))
            {

                switch (_enable)
                {
                    case 1:
                        CloseWindow();
                        _enable = 0;
                        break;
                    case 0:
                        OpenWindow();
                        _enable = 1;
                        break;
                }
            }
        }



        public void ConstructionInputs()
        {

            if (_dataRefs.BuildRefs.Count == 0)
                return;

            MousePosition();
            CheckVisual();

            if (Input.GetKeyDown(KeyCode.Mouse0) && Checkcolitions() && CheckDistance())
            {
                PlaceHolo();
            }
            else if(Input.GetKeyDown(KeyCode.Mouse0) && !Checkcolitions() && CheckDistance())
            {
                ErrorFinder(ErrorType.PlaceError);
            }
            else if (Input.GetKeyDown(KeyCode.Mouse0) && Checkcolitions() && !CheckDistance())
            {
                ErrorFinder(ErrorType.Distance);
            }


            if (Input.mouseScrollDelta.y != 0)
            {
                ScrollFunc(Input.mouseScrollDelta.y);
            }


            if (Input.GetKeyDown(KeyCode.Z))
            {
                RemoveHolo(_current_Builds_Refs.Count - 1);
            }
        }

        public void DestructionInputs()
        {
            MouseDestroyPos(FindPlacedObjects());

            if (Input.GetKeyDown(KeyCode.Mouse0) && _colRefDest != null)
            {
                PlaceDestroyHolo();
            }


            if (Input.GetKeyDown(KeyCode.Z))
            {
                RemoveDestroyHolo(_selectedToDemolish.Count - 1);
            }
        }




        public void OpenWindow()
        {
            _dataRefs = new();
            StopAllCoroutines();
            ClearList();
        }

        public void CloseWindow()
        {
            StopAllCoroutines();
            if (_current_Builds_Refs.Count >= 1)
                StartCoroutine(ConstructionFunction());
            if (_selectedToDemolish.Count >= 1)
                StartCoroutine(DestroyFunction());
            _spriteRefs.color = new Color(0, 0, 0, 0);
            _dataRefs = new();
            if (_destruction)
                SwichMode();
            foreach (var item in _current_Prices_Gross_Visual)
                item.Amount = 0;
            UIManager.Gross(_current_Prices_Gross_Visual);
        }


        public void SwichMode()
        {
            _destruction = !_destruction;

            if (_destruction)
            {
                _spriteRefs.color = _blockColor;
                _spriteRefs.sprite = _BoxDestroySprite;
                _dataRefs = default;
                foreach (var item in _current_Builds_Refs)
                    Destroy(item.HoloLoc);
                _current_Builds_Refs.Clear();
                ClearPrices();
            }
            else if(_selectedToDemolish.Count <= 0)
            {
                Debug.Log("non destroy");
                foreach (var item in _selectedToDemolish)
                    Destroy(item.HoloLoc);
                _spriteRefs.gameObject.transform.localScale = new Vector3(1,1,1);
                _selectedToDemolish.Clear();
            }
            UIManager.SwichMode(_destruction);


        }



        #region BuildFuncs
        public void MousePosition()
        {
            Vector3Int gridposs = _grid.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition) + _gridMouseAdjustment);
            _holoHolder.transform.position = Vector3.Lerp(_holoHolder.transform.position, _grid.CellToWorld(gridposs) + new Vector3(_grid.cellSize.x / 2, _grid.cellSize.y / 2, 0), Time.deltaTime * _lerpAmount);
            _holoHolder.transform.position = new Vector3(_holoHolder.transform.position.x, _holoHolder.transform.position.y, 0);

            Vector3 hold = new Vector3(0, -_ColSize.y, 0);
            _VisualHoldRef.transform.position = Vector3.Lerp(_VisualHoldRef.transform.position, _grid.CellToWorld(gridposs) + new Vector3(_grid.cellSize.x / 2, _grid.cellSize.y / 2, 0) +_rotationVisualAdjustment + hold, Time.deltaTime * _lerpAmount);
            _VisualHoldRef.transform.position = new Vector3(_VisualHoldRef.transform.position.x, _VisualHoldRef.transform.position.y, 0);

        }


        public Vector3 GetGridPoss()
        {
            Vector3Int gridposs = _grid.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition) + _gridMouseAdjustment);

            return _grid.CellToWorld(gridposs) + new Vector3(_grid.cellSize.x / 2, _grid.cellSize.y / 2, 0);
        }

        public void ScrollFunc(float direction)
        {
            _buildSelectorID += (int)direction;
            _buildSelectorID = (int)Mathf.Repeat(_buildSelectorID, _dataRefs.BuildRefs.Count);
            GetBuildStuff(_buildSelectorID);
        }


        public void GetData(BuildData data, bool variants)
        {
            if (data == null)
                return;
            _dataRefs = data;

            if (_dataRefs.BuildRefs.Count > 1)
            {
                _rotationRef.SetActive(true);
                if (variants)
                {
                    _ArrowsRef.SetActive(false);
                    _text.text = "Variaciones";
                }
                else
                {
                    _text.text = "Rotacion";
                    _ArrowsRef.SetActive(true);
                }

            }
            else
                _rotationRef.SetActive(false);

            //misc resets
            if (_destruction)
                SwichMode();
            Debug.Log(_dataRefs.BuildRefs.Count);
            _buildSelectorID = 0;
            _destruction = false;
            UIManager.SwichMode(_destruction);
            GetBuildStuff(_buildSelectorID);

        }







        public void PlaceHolo()
        {


            BuildingData holder = new();
            Vector2 pos = GetGridPoss();

            holder.HoloLoc = Instantiate(_holoHolder, pos, _holoHolder.transform.rotation);
            holder.Prices = _dataRefs.Prices;
            holder.PrefRef = _dataRefs.BuildRefs[_buildSelectorID];

            holder.HoloLoc.layer = _PlacedHoloMaskID;
            StopAllCoroutines();
            AddToGross(holder.Prices);
            _current_Builds_Refs.Add(holder);


            if (CheckGrossOverFlow())
            {
                Debug.Log("aaa");
                ErrorFinder(ErrorType.NotEnoughtMoney);
            }
        }



        public void RemoveHolo(int place)
        {
            if (_current_Builds_Refs.Count == 0)
                return;

            BuildingData holder = _current_Builds_Refs[place];
            StopAllCoroutines();
            RemoveFromGross(holder.Prices);
            Destroy(holder.HoloLoc);
            _current_Builds_Refs.Remove(holder);
        }




        public IEnumerator ConstructionFunction()
        {
            yield return new WaitForSeconds(_BuildInterval);
            foreach (var item in _current_Builds_Refs)
            {
                if (CheckPrices(item.Prices))
                    continue;
                RemoveFromGross(item.Prices);
                foreach (var itema in item.Prices)
                    GameManager.Instance.RemoveMaterialAmount(itema);


                SaveData holder = new()
                {
                    GameBuildRef = Instantiate(item.PrefRef, item.HoloLoc.transform.position, item.HoloLoc.transform.rotation),
                    Prices = item.Prices
                };
                holder.Pos = holder.GameBuildRef.transform.position;

                Destroy(item.HoloLoc);
                _empty_Builds_Refs.Add(item);
                _SaveList.Add(holder);
                yield return new WaitForSeconds(_BuildInterval);
            }

            ClearList();
        }


        public void GetBuildStuff(int id)
        {
            var hitbox = _dataRefs.BuildRefs[id].GetComponentInChildren<BoxCollider2D>();
            var sprite = _dataRefs.BuildRefs[id].GetComponentInChildren<SpriteRenderer>();

            if (hitbox != null)
            {
                _ColSize = hitbox.size;
                _ColOffset = hitbox.offset;
            }
            else
            {
                _ColSize = sprite.size;
                _ColOffset = sprite.gameObject.transform.position;
            }


            _spriteRefs.sprite = sprite.sprite;
        }



        #endregion


        #region DestroyFuncs


        public void MouseDestroyPos(BuildingBase building)
        {
            if (_buildRefDest != building)
            {
                _colRefDest = null;

            }


            if (building == null)
            {
                _colRefDest = null;
                _spriteRefs.gameObject.transform.localScale = Vector2.Lerp(_spriteRefs.gameObject.transform.localScale, new Vector3(1, 1, 1), Time.deltaTime * (_lerpAmount / 2));

                MousePosition();
                return;
            }
            if (_colRefDest == null)
            {
                _colRefDest = building.GetComponentInChildren<BoxCollider2D>();
                _buildRefDest = building;
            }
            _holoHolder.transform.position = Vector2.Lerp(_holoHolder.transform.position, _colRefDest.transform.position, Time.deltaTime * (_lerpAmount / 2));
            _spriteRefs.gameObject.transform.localScale = Vector2.Lerp(_spriteRefs.gameObject.transform.localScale, _colRefDest.size, Time.deltaTime * (_lerpAmount / 2));


        }


        public void PlaceDestroyHolo()
        {
            BuildingData holder = new();
            holder.HoloLoc = Instantiate(_holoHolder, _colRefDest.transform.position, _colRefDest.transform.rotation);
            holder.HoloLoc.transform.localScale = _colRefDest.size;
            holder.PrefRef = _buildRefDest.gameObject;

            List<Materials> mats = new();
            foreach (var prieces in _SaveList.Find(x => x.GameBuildRef == holder.PrefRef).Prices)
            {
                Materials holded = new();
                holded.Amount = prieces.Amount / 2;
                holded.Type = prieces.Type;
                mats.Add(holded);
            }



            holder.Prices = mats;



            StopAllCoroutines();
            AddToGross(holder.Prices);
            _selectedToDemolish.Add(holder);
        }

        public IEnumerator DestroyFunction()
        {

            foreach (var item in _selectedToDemolish)
            {
                Debug.Log(item.PrefRef.name);
                yield return new WaitForSeconds(_BuildInterval);


                foreach (var prieces in item.Prices)
                    GameManager.Instance.AddMaterialAmount(prieces);

                RemoveFromGross(item.Prices);
                Destroy(item.PrefRef);
                Destroy(item.HoloLoc);
                //
                _selectedToDemolish_Empty.Add(item);
                _SaveList.Remove(_SaveList.Find(x => x.GameBuildRef == item.PrefRef));
            }
            Debug.Log("?");
            ClearDestroyList();
        }


        public void RemoveDestroyHolo(int place)
        {
            if (_selectedToDemolish.Count == 0)
                return;

            BuildingData holder = _selectedToDemolish[place];
            StopAllCoroutines();
            RemoveFromGross(holder.Prices);
            Destroy(holder.HoloLoc);
            _selectedToDemolish.Remove(holder);
        }


        #endregion




        #region Functions

        public void ClearPrices()
        {
            foreach (var item in _current_Prices_Gross_Code)
                item.Amount = 0;
            foreach (var item in _current_Prices_Gross_Visual)
                item.Amount = 0;
            UIManager.Gross(_current_Prices_Gross_Visual);
        }



        public void LockInputs(bool input)
        {
            _lockUI = input;

            if (_lockUI)
                _spriteRefs.color = new Vector4(0,0,0,0);

            if (_lockUI == false && _destruction == false)
            {
                if (_dataRefs.BuildRefs.Count != 0)
                    _spriteRefs.color = _nonBlockColor;
            }
            else if(_lockUI == false && _destruction == true)
            {
                _spriteRefs.color = _blockColor;
            }

        }


        public void AddToGross(List<Materials> mats)
        {

            foreach (var item in mats)
            {
                var holdc = _current_Prices_Gross_Code.Find(i => i.Type == item.Type);

                holdc.Amount += item.Amount;

                StartCoroutine(AddToGrossAnimIndv(holdc, _current_Prices_Gross_Visual.Find(i => i.Type == item.Type)));
            }
        }

        public void RemoveFromGross(List<Materials> mats)
        {
            foreach (var item in mats)
            {
                var holdc = _current_Prices_Gross_Code.Find(i => i.Type == item.Type);

                holdc.Amount -= item.Amount;

                StartCoroutine(RemoveToGrossAnimIndv(holdc, _current_Prices_Gross_Visual.Find(i => i.Type == item.Type)));

            }
        }


        public IEnumerator AddToGrossAnimIndv(Materials CodeMat, Materials VisualMat)
        {
            float _currentTime = 0;
            int dif = CodeMat.Amount - VisualMat.Amount;


            while (VisualMat.Amount < CodeMat.Amount)
            {
                _currentTime += Time.deltaTime / dif;
                yield return new WaitForSeconds(_visualRampUp.Evaluate(_currentTime * _RampUpSpeed));
                VisualMat.Amount++;
                UIManager.Gross(_current_Prices_Gross_Visual);
            }
        }

        public IEnumerator RemoveToGrossAnimIndv(Materials CodeMat, Materials VisualMat)
        {
            float _currentTime = 0;
            int dif = VisualMat.Amount - CodeMat.Amount;


            while (VisualMat.Amount > CodeMat.Amount)
            {
                _currentTime += Time.deltaTime / dif;
                yield return new WaitForSeconds(_visualRampUp.Evaluate(_currentTime * _RampUpSpeed));
                VisualMat.Amount--;
                UIManager.Gross(_current_Prices_Gross_Visual);
            }
            if (VisualMat.Amount < 0)
                VisualMat.Amount = 0;
        }

        public void ClearList()
        {
            foreach (var item in _empty_Builds_Refs)
            {
                _current_Builds_Refs.Remove(item);
            }

            _empty_Builds_Refs.Clear();
        }

        public void ClearDestroyList()
        {
            foreach (var item in _selectedToDemolish_Empty)
            {
                _selectedToDemolish.Remove(item);
            }

            _empty_Builds_Refs.Clear();
        }

        public void CheckVisual()
        {
            Debug.Log(Checkcolitions() && CheckDistance());
            if (Checkcolitions() && CheckDistance())
            {
                _spriteRefs.color = _nonBlockColor;
            }
            else
            {
                _spriteRefs.color = _blockColor;
            }
        }


        public bool CheckPrices(List<Materials> mats)
        {
            bool check = true;

            foreach (var item in mats)
            {
                if (check == true && !GameManager.Instance.CheckAmount(item))
                    check = false;
            }


            return check;
        }


        public bool Checkcolitions()
        {
            return Physics2D.OverlapBox(_holoHolder.transform.position + (Vector3)_ColOffset, _ColSize, 0, _DetectionMask) == null;
        }

        public bool CheckDistance()
        {
            return Vector3.Distance(_holoHolder.transform.position, transform.position) < _limitsize;
        }
        public BuildingBase FindPlacedObjects()
        {
            var holder = Physics2D.OverlapBox(Camera.main.ScreenToWorldPoint(Input.mousePosition), _DestroyDetectionBoxSize, 0, _BuildingMask);
            if (holder != null)
                return holder.GetComponentInParent<BuildingBase>();
            else
                return null;
        }

        public void ErrorFinder(ErrorType type)
        {
            if (_error != null)
                StopCoroutine(_error);
            _error = ErrorShower(_errorTypes.Find(x => x.Type == type));
            StartCoroutine(_error);
        }


        public IEnumerator ErrorShower(ErrorData error)
        {
            _ErrorTextRef.text = error.ErrorDescription;
            _ErrorTextRef.gameObject.SetActive(true);
            yield return new WaitForSeconds(_textShowTime);
            _ErrorTextRef.gameObject.SetActive(false);
            Debug.Log("patata");
        }

        public bool CheckGrossOverFlow()
        {
            foreach (var item in _current_Prices_Gross_Code)
            {
                if (GameManager.Instance.CheckAmount(item))
                {
                    return true;
                }
            }

            return false;
        }
        #endregion




        [System.Serializable]
        public class BuildingData
        {
            public GameObject HoloLoc;
            public GameObject PrefRef;
            public List<Materials> Prices = new();
        }

        [System.Serializable]
        public class SaveData
        {
            public GameObject GameBuildRef;
            public Vector2 Pos;
            public List<Materials> Prices = new();
        }

        [System.Serializable]
        public class ErrorData
        {
            [TextArea(4, 20)] public string ErrorDescription;
            public ErrorType Type;
        }

        public enum ErrorType
        {
            PlaceError,
            NotEnoughtMoney,
            Distance,
            Type4
        }
    }
}
[System.Serializable]
public class BuildData
{
    public List<GameObject> BuildRefs = new();

    public List<Materials> Prices = new();
}