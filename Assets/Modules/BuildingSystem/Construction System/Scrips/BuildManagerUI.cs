using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;


namespace BuildSystem
{


    public class BuildManagerUI : MonoBehaviour
    {
        public static BuilderManager Manager;

        [Header("Config")]
        [SerializeField] private List<HudButtons> _buttons = new();
        [SerializeField] private List<ResourceHud> _resources = new();

        [SerializeField] private string _pricename;
        [SerializeField] private string _resourcename;

        [SerializeField] private GameObject _button;
        [SerializeField] private Transform _Hud_transform;
        [SerializeField] private DestrictionStuff DescriptionHolder;


        [Header("Destruction Hammer Config")]

        private string _type;
        [SerializeField] private HudButtons DestroyButton;


        // Start is called before the first frame update
        void Start()
        {
            BuilderManager.UIManager = this;
            ButtonsSetUp();
            int id = 0;
            DescriptionHolder.Holder.gameObject.SetActive(false);


            foreach (var item in _resources)
            {
                
                List<TextMeshProUGUI> stuff = new(item.Ref.GetComponentsInChildren<TextMeshProUGUI>());
                item.Ref.GetComponentInChildren<Image>().sprite = GameManager.Instance.Resources[id].Icon;

                item.Price = stuff.Find(x => x.gameObject.name == _pricename);
                item.Resource = stuff.Find(x => x.gameObject.name == _resourcename);

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
            }

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void FixedUpdate()
        {
            VisualUpdate();
        }

        public void SwichMode(bool mode)
        {
            if (mode)
                _type = "+ ";
            else
                _type = "- ";
        }

        public void VisualUpdate()
        {
            foreach (var item in _resources)
            {
                item.Resource.text = item.Mat.Amount.ToString();
                item.Price.text = _type + item.AddedValue.ToString();
            }
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


            DestroyButton.ButtonRef = Instantiate(_button, _Hud_transform).GetComponent<Button>();
            DestroyButton.Father = this;

            DestroyButton.SetUp();

            DestroyButton.ButtonRef.onClick.RemoveAllListeners();
            DestroyButton.ButtonRef.onClick.AddListener(SwitchDestroyMode);

            foreach (var item in _buttons)
            {
                AddButton(item);
            }
        }

        public void AddButton(HudButtons item)
        {
            item.ButtonRef = Instantiate(_button, _Hud_transform).GetComponent<Button>();




            item.Father = this;
            item.SetUp();
        }


        public void SwitchDestroyMode()
        {
            Manager.SwichMode();
        }


        public void SendData(BuildData Data)
        {
            Manager.GetData(Data);
        }


        public void OnHover(HudButtons button)
        {

            foreach (var items in button.Data.Prices)
            {
                var holder = DescriptionHolder.Prices.Find(x => x.Mat == items.Type);
                holder.Text.text = items.Amount.ToString();
                holder.Pref.SetActive(true);
            }
            DescriptionHolder.Header.text = button.Name;
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
        [HideInInspector] public Button ButtonRef;
        [Header("Data Config")]
        public BuildData Data = new();
        public string Name;
        [TextArea(4,20)]public string Description;


        [Header("visual Config")]
        public Sprite IconSprite;
        


        [HideInInspector] public BuildManagerUI Father;
        public void SetUp()
        {
            //set up visual
            ButtonRef.onClick.AddListener(Call);
            Image maintext = ButtonRef.gameObject.GetComponent<Image>();
            maintext.color = Color.blue;

            var hold = ButtonRef.gameObject.GetComponentsInChildren<Image>();
            foreach (var itema in hold)
            {
                if (itema != maintext)
                    itema.sprite = IconSprite;
            }
            

            //set up event stuff fml

            EventTrigger eventhold = ButtonRef.GetComponent<EventTrigger>();
            EventTrigger.Entry hover = new EventTrigger.Entry() { eventID = EventTriggerType.PointerEnter };
            EventTrigger.Entry exitHover = new EventTrigger.Entry() { eventID = EventTriggerType.PointerExit };
            hover.callback.AddListener(OnHover);
            exitHover.callback.AddListener(OnExit);
            eventhold.triggers.Add(hover);
            eventhold.triggers.Add(exitHover);

        }

        public void Call()
        {
            Father.SendData(Data);
        }

        public void OnHover(BaseEventData data)
        {
            Father.OnHover(this);
        }
        public void OnExit(BaseEventData data)
        {
            Father.HoverExit();
        }
    }
}