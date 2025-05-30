using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class MainMenuTest : MonoBehaviour
{
    [SerializeField] VideoPlayer mainMenuStart = null;
    [SerializeField] VideoPlayer mainMenuLoop = null;
    [SerializeField] GameObject mainMenu = null;
    [SerializeField] GameObject videoBG = null;
    [SerializeField] RenderTexture rendTexBG = null;

    Animator screenMainMenuAnim;

    void Start()
    {
        if (mainMenu)
            screenMainMenuAnim = mainMenu.GetComponent<Animator>();

        if (!mainMenuStart) return;

        mainMenuStart.loopPointReached += EndReached;

        mainMenuStart.Play();
    }

    void EndReached(VideoPlayer vp)
    {
        Debug.Log("El video termino");

        if (videoBG)
        {
            videoBG.GetComponent<RawImage>().texture = rendTexBG;
            mainMenuLoop.Play();
        }

        if (mainMenu)
            screenMainMenuAnim.Play("Show");
    }
}
