using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{

    private int _currentScreen;
    [SerializeField] private List<MenuScreen> _screens;

    // Start is called before the first frame update
    void Start()
    {
        UIManager.MenuManager = this;
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

}
