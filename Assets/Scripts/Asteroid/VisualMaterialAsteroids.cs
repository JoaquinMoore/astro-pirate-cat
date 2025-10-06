using UnityEngine;
using TMPro;

namespace Asteroid
{
    public class VisualMaterialAsteroids : MonoBehaviour
    {
        [field: SerializeField] public ResoursVisual[] Resours { get; private set; }

        public void Initial(MaterialType type, int count)
        {
            foreach (var item in Resours)
            {
                item.Obj.SetActive(false);

                if (item.Type == type)
                {
                    item.Obj.SetActive(true);
                    item.Text.text = count.ToString();
                }
            }
        }

        public void Desactive() => Destroy(gameObject, 10);
    }

    [System.Serializable]
    public class ResoursVisual
    {
        public MaterialType Type;
        public GameObject Obj;
        public TMP_Text Text;
    }
}