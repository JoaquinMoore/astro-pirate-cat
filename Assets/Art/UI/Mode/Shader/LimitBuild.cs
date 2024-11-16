using UnityEngine;

public class LimitBuild : MonoBehaviour
{
    [SerializeField] Material _mat;
    [SerializeField] float _size;

    public void Start()
    {
        _size = GameManager.Instance._limitsize;
        _mat.SetFloat("_ScaleOrigin", transform.localScale.x > transform.localScale.y ? transform.localScale.x : transform.localScale.y);
        DrawCircle(_size);
    }

    private void Update()
    {
        if (_size != GameManager.Instance._limitsize)
        {
            _size = GameManager.Instance._limitsize;
            DrawCircle(_size);
        }
    }


#if UNITY_EDITOR
    private void OnValidate()
    {
        _mat.SetFloat("_ScaleOrigin", transform.localScale.x > transform.localScale.y ? transform.localScale.x : transform.localScale.y);
        DrawCircle(_size);
    }
#endif

    void DrawCircle(float radius)
    {
        _mat.SetFloat("_Ext", _size);
    }
}