using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BuilderManager : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Transform _holoHolder;

    [Header("Visual Config")]
    [SerializeField] private float _lerpAmount;
    [SerializeField] private Color _nonBlockColor;
    [SerializeField] private Color _blockColor;
    [SerializeField] private Vector3 _gridMouseAdjustment;

    [Header("Config")]
    [SerializeField] private LayerMask _placedMask;

    [Header("debug")]
    [SerializeField] private SpriteRenderer _spriteRefs;


    [SerializeField] private UnNamedClass3 _dataRefs;
    [SerializeField] private List<UnNamedClass1> _current_Builds_Refs;
    [SerializeField] private UnNamedClass2 _current_Prices_Gross;

    [SerializeField] private Vector2 holder;

    [SerializeField] private Grid _grid;

    void Start()
    {
        _grid = GetComponent<Grid>();
        BuildManagerUI.Manager = this;

        _spriteRefs = _holoHolder.GetComponent<SpriteRenderer>();
        _spriteRefs.color = _nonBlockColor;


    }


    void Update()
    {
        MousePosition();

        if (Checkcolitions())
        {
            _spriteRefs.color = _nonBlockColor;
        }
        else
        {
            _spriteRefs.color = _blockColor;
        }
    }



    public void MousePosition()
    {
        Vector3Int gridposs = _grid.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition) + _gridMouseAdjustment);
        _holoHolder.transform.position = Vector3.Lerp(_holoHolder.transform.position , _grid.CellToWorld(gridposs) + new Vector3(_grid.cellSize.x / 2, _grid.cellSize.y / 2, 0), Time.deltaTime * _lerpAmount);
        _holoHolder.transform.position = new Vector3(_holoHolder.transform.position.x, _holoHolder.transform.position.y, 0);

    }




    public void GetData(UnNamedClass3 data)
    {
        Debug.Log("def");
    }






    public bool Checkcolitions()
    {
        return Physics2D.OverlapBox(_holoHolder.transform.position, _spriteRefs.size, 0, _placedMask) == null;
    }










    [System.Serializable]
    public class UnNamedClass1
    {
        public GameObject PrefLoc;
        public List<Materials> Prices;
    }

    [System.Serializable]
    public class UnNamedClass2
    {
        public List<Materials> Prices;
    }



}
[System.Serializable]
public class UnNamedClass3
{
    public List<GameObject> BuildRefs;

    public List<Materials> Prices;
}