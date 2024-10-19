using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Utility.SingletonMono<GameManager>
{
    [SerializeField] private GameObject _player;
    public GameObject player => _player;

    [field: SerializeField] public List<MaterialsVisual> Resources { get; private set; } 
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        
    }

    #region MATERIALS
    /// <summary>
    /// Agrega una cantidad de materiales segun su key.
    /// </summary>
    public void AddMaterialAmount(Materials mat)
    {

        Resources.Find(i => i.Type == mat.Type).Amount += mat.Amount;

    }

    public void RemoveMaterialAmount(Materials mat)
    {
        Resources.Find(i => i.Type == mat.Type).Amount -= mat.Amount;
    }

    public Sprite IconSprite(MaterialsVisual mat)
    {
        return Resources.Find(i => i.Type == mat.Type).Icon;
    }



    public bool CheckAmount(Materials price)
    {
        bool answere = false;

        if (Resources.Find(i => i.Type == price.Type).Amount < price.Amount)
            answere = true;
        return answere;
    }





    #endregion

    public bool CheckPlayerDistance(Vector3 Pos, float ReachDistance)
    {
        return Vector3.Distance(player.transform.position, Pos) <= ReachDistance;
    }


}


[System.Serializable]
public class MaterialsVisual : Materials
{
    public Sprite Icon;
}
[System.Serializable]
public class Materials
{
    public MaterialType Type;
    public int Amount;
}

public enum MaterialType
{
    mat1,
    mat2,
    mat3,
    mat4
}