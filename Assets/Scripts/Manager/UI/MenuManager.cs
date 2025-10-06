using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    private bool _failState;
    private int _currentScreen;
    [SerializeField] private List<MenuScreen> _screens;
    [SerializeField] private int _startingscreen;

    // Start is called before the first frame update
    void Start()
    {
        UIManager.MenuManager = this;
        ChangeScreen(_startingscreen);
    }




    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    public void ChangeScreen(int screen)
    {
        int oldscreen = _currentScreen;
        _currentScreen = screen;

        _screens[oldscreen].CloseScreen();
        _screens[_currentScreen].OpenScreen();

        if (!_screens[_currentScreen].gameObject.activeSelf)
            _screens[_currentScreen].OpenScreen();
    }

    public void SwichMenuState(int id, bool state)
    {
        if (state)
            _screens[id].OpenScreen();
        else
            _screens[id].CloseScreen();

    }

    public void FailState()
    {
        _failState = true;
    }

    public bool GetFinalState()
    {
        return _failState;
    }



}
