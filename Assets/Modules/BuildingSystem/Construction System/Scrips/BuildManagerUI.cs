using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;


namespace BuildSystem
{


    public class BuildManagerUI : MonoBehaviour
    {
        public static BuilderManager Manager;

        [Header("Config")]
        [SerializeField] private Sprite _destroyButtonSprite;
        [SerializeField] private List<HudButtons> _buttons = new();
        [SerializeField] private List<ResourceHud> _resources = new();

        [SerializeField] private string _pricename;
        [SerializeField] private string _resourcename;

        [SerializeField] private GameObject _button;
        [SerializeField] private GameObject _Trashbutton;
        [SerializeField] private Transform _Hud_transform;
        [SerializeField] private DestrictionStuff DescriptionHolder;

        [Header("Visual")]
        [SerializeField, Tooltip("Tiempo para que se apague la visual cuando pasa el raton por ensima")] 
        private float _UnhoverVisualButton;

        //[Header("Destruction Hammer Config")]
        private HudButtons _destroyButton = new();
        private string _type;
        private HudButtons _currentButton;

        private bool fuck;
        //[SerializeField] private HudButtons DestroyButton;
        


        // Start is called before the first frame update
        void Start()
        {
            BuilderManager.UIManager = this;
            ButtonsSetUp();
            int id = 0;
            DescriptionHolder.Holder.gameObject.SetActive(false);

            //VisualUpdate();
            foreach (var item in _resources)
            {
                
                List<TextMeshProUGUI> stuff = new(item.Ref.GetComponentsInChildren<TextMeshProUGUI>());
                item.Ref.GetComponentInChildren<Image>().sprite = GameManager.Instance.Resources[id].Icon;

                item.Price = stuff.Find(x => x.gameObject.name == _pricename);
                item.Resource = stuff.Find(x => x.gameObject.name == _resourcename);

                item.Price.color = Color.red;
                item.Mat = GameManager.Instance.Resources[id];

                GameObject holder = Instantiate(DescriptionHolder.PricePref, DescriptionHolder.PriceTransform);
                Debug.Log(holder.GetComponentInChildren<Image>());
                holder.GetComponentInChildren<Image>().sprite = GameManager.Instance.Resources[id].Icon;
                PriceList hold = new()
                {
                    Pref = holder,
                    Mat = GameManager.Instance.Resources[id].Type,
                    Text = holder.GetComponentInChildren<TextMeshProUGUI>()
                };
                hold.Pref.SetActive(false);
                DescriptionHolder.Prices.Add(hold);

                id++;

                item.Price.gameObject.SetActive(false);
                //item.Ref.SetActive(false);
            }
            VisualUpdate();
        }

        private void OnEnable()
        {
            fuck = false;
            _currentButton.Unselected();
            _currentButton = null;
        }
        private void OnDisable()
        {
            fuck = false;
        }


        // Update is called once per frame
        void Update()
        {
            if (_currentButton != null)
            {
                _currentButton.Selected();
            }
        }

        private void FixedUpdate()
        {
            VisualUpdate();
            if (fuck == false)
                Visualfix();

        }

        public void SwichMode(bool mode)
        {
            if (mode)
            {
                _type = "+ ";
                foreach (var item in _resources)
                {
                    item.Price.color = Color.green;
                }
                if (_currentButton != null)
                    _currentButton.Unselected();
            }
            else
            {
                _type = "- ";
                foreach (var item in _resources)
                {
                    item.Price.color = Color.red;
                }
            }

        }

        public void VisualUpdate()
        {
            foreach (var item in _resources)
            {
                if (item.AddedValue > 0)
                    item.Price.gameObject.SetActive(true);
                item.Resource.text = item.Mat.Amount.ToString();
                item.Price.text = _type + item.AddedValue.ToString();
            }

            foreach (var item in _buttons)
            {
                if (item._text == null)
                    continue;
                item._text.text = item.Data.CurrentBuildingAmount + "/" + item.Data.BuildingAmount;
            }
        }

        public void Visualfix()
        {
            foreach (var item in _resources)
            {
                item.Resource.gameObject.SetActive(false);

                item.Resource.gameObject.SetActive(true);
            }
            fuck = true;
        }


        public void Gross(List<Materials> mats)
        {
            foreach (var item in _resources)
            {
                item.AddedValue = mats.Find(x => x.Type == item.Mat.Type).Amount;
            }
        }
        public void ButtonsSetUp()
        {
            _destroyButton.IconSprite = _destroyButtonSprite;
            _destroyButton.ButtonRef = Instantiate(_Trashbutton, _Hud_transform).GetComponent<Button>();
            _destroyButton.Father = this;
            _destroyButton.SetUp();
            _destroyButton.ButtonRef.onClick.RemoveAllListeners();
            _destroyButton.ButtonRef.onClick.AddListener(SwitchDestroyMode);


            foreach (var item in _buttons)
                AddButton(item);
        }

        public void AddButton(HudButtons item)
        {
            item.ButtonRef = Instantiate(_button, _Hud_transform).GetComponent<Button>();

            item.Father = this;
            item._timer = _UnhoverVisualButton;
            item.SetUp();
        }


        public void SwitchDestroyMode()
        {
            Manager.SwichMode();
        }


        public void SendData(BuildData Data, bool variants, HudButtons button)
        {
            if (_currentButton != null)
                _currentButton.Unselected();
            Manager.GetData(Data, variants);
            _currentButton = button;
        }


        public void ChangeValueData(BuildingBase building, bool add)
        {
            foreach (var item in _buttons)
            {
                var hold = item.Data.BuildRefs.Find(x => x == building.gameObject);
                var hold2 = item.Data.BuildRefs.Find(x => x == PrefabUtility.IsPartOfAnyPrefab(building));
                Debug.Log(PrefabUtility.GetPrefabInstanceHandle(building));
                if (hold != null || hold2 != null)
                {
                    if (add)
                        item.Data.CurrentBuildingAmount++;
                    else
                        item.Data.CurrentBuildingAmount--;
                    Debug.Log(item.Data.CurrentBuildingAmount);
                    break;
                }

            }

        }


        public void OnHover(HudButtons button)
        {
            if (button.Data.BuildRefs.Count == 0)
            {
                ChangeLock(true);
                return;
            }


            foreach (var items in button.Data.Prices)
            {
                var holder = DescriptionHolder.Prices.Find(x => x.Mat == items.Type);
                holder.Text.text = items.Amount.ToString();
                holder.Pref.SetActive(true);
            }
            DescriptionHolder.Header.text = button.name;
            DescriptionHolder.Text.text = button.Description;

            DescriptionHolder.Holder.gameObject.transform.position = button.ButtonRef.transform.position;
            DescriptionHolder.Holder.gameObject.SetActive(true);
            ChangeLock(true);
        }

        public void HoverExit()
        {
            DescriptionHolder.Holder.gameObject.SetActive(false);
            foreach (var item in DescriptionHolder.Prices)
            {
                item.Pref.SetActive(false);
            }
            ChangeLock(false);
        }


        public void ChangeLock(bool intput)
        {
            Manager.LockInputs(intput);
            Debug.Log(intput);
        }

        [System.Serializable]
        public class ResourceHud
        {
            public GameObject Ref;
            public TextMeshProUGUI Resource;
            public TextMeshProUGUI Price;
            public Materials Mat;
            public int AddedValue;
        }

        [System.Serializable]
        public class DestrictionStuff
        {
            public GameObject Holder;
            public Transform PriceTransform;
            public GameObject PricePref;
            public TextMeshProUGUI Header;
            public TextMeshProUGUI Text;

            [Header("debug")]
            public List<PriceList> Prices = new();
        }
        [System.Serializable]
        public class PriceList
        {
            public GameObject Pref;
            public MaterialType Mat;
            public TextMeshProUGUI Text;
        }

    }


    [System.Serializable]
    public class HudButtons
    {
        public string name;
        [TextArea(4, 20)] public string Description;
        [HideInInspector] public Button ButtonRef;



        public BuildData Data = new();

        [HideInInspector] public EventTrigger _event;
        [HideInInspector] public TextMeshProUGUI _text;
        [HideInInspector] public Animator _anim;
        [HideInInspector] public float _timer;
        [Header("visual Config")]
        public Sprite IconSprite;
        public bool Variants;

        private bool selected;
        private IEnumerator animsel;

        [HideInInspector] public BuildManagerUI Father;
        public void SetUp()
        {
            //set up visual
            ButtonRef.onClick.AddListener(Call);
            Image maintext = ButtonRef.gameObject.GetComponent<Image>();
            _anim = ButtonRef.GetComponent<Animator>();
            var hold = ButtonRef.gameObject.GetComponentsInChildren<Image>();
            foreach (var itema in hold)
            {
                if (itema != maintext)
                    itema.sprite = IconSprite;
                Debug.Log(itema.sprite);
            }

            _text = ButtonRef.GetComponentInChildren<TextMeshProUGUI>();

            //set up event stuff fml

            _event = ButtonRef.GetComponent<EventTrigger>();
            EventTrigger.Entry hover = new EventTrigger.Entry() { eventID = EventTriggerType.PointerEnter };
            EventTrigger.Entry exitHover = new EventTrigger.Entry() { eventID = EventTriggerType.PointerExit };
            hover.callback.AddListener(OnHover);
            exitHover.callback.AddListener(OnExit);
            _event.triggers.Add(hover);
            _event.triggers.Add(exitHover);

        }

        public void Call()
        {
            Father.SendData(Data, Variants, this);
            _anim.SetTrigger("SelectedT");
        }

        public void OnHover(BaseEventData data)
        {
            Father.OnHover(this);
        }
        public void OnExit(BaseEventData data)
        {
            Father.HoverExit();
            if (!selected)
            {
                if (animsel != null)
                    Father.StopCoroutine(animsel);

                animsel = unHoveredTimer();
                Father.StartCoroutine(animsel);
            }
                
        }

        public void Selected()
        {

            selected = true;
            _anim.SetTrigger("Selected");
        }
        public void Unselected()
        {
            selected = false;
            _anim.SetTrigger("NormalT");
        }


        IEnumerator unHoveredTimer()
        {
            yield return new WaitForSeconds(_timer);
            _anim.SetTrigger("NormalT");
        }
    }
}