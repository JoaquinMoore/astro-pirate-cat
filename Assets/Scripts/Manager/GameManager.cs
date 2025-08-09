using System.Collections;
using System.Collections.Generic;
using _UTILITY;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonMono<GameManager>
{
    [SerializeField] private MaincharacterController _player;
    [field: SerializeField] public float _limitsize { get; private set; }
    public MaincharacterController player => _player;


    [field: SerializeField] public List<MaterialsVisual> Resources { get; private set; }
    public static WaveManager _wave;
    private bool finishedGame;
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

    #region GameStates

    public void WinState()
    {
        if (finishedGame)
            return;
        finishedGame = true;
        UIManager.MenuManager.ChangeScreen(3);
        _player.SwichControlScreme(ControlScheme.FailWin);
    }
    public void FailState()
    {
        if (finishedGame)
            return;
        finishedGame = true;

        UIManager.MenuManager.FailState();
        UIManager.MenuManager.ChangeScreen(3);
        _player.SwichControlScreme(ControlScheme.FailWin);
    }


    public void FinishGameControl()
    {
        if (UIManager.MenuManager.GetFinalState())
        {
            RestartGame();
        }
        UIManager.MenuManager.ChangeScreen(0);
        _player.SwichControlScreme(ControlScheme.Gameplay);
    }

    public void ReturnToMenu()
    {
        //al menu
    }

    public void RestartGame()
    {
        //Application.LoadLevel(Application.loadedLevel);
        Debug.Log("ca");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

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
    Croqueta,
    Aquafish,
    Catnipfish,
    Goldfish,
    CurrentCrew
}