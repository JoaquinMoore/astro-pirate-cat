using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;
using System.IO;


public class FOWManager : MonoBehaviour
{
    [SerializeField] private bool _savePic;
    [SerializeField] private RenderTexture _rt;
    [SerializeField] private Texture2D _image;
    [SerializeField] private string _filepath;
    [SerializeField] private string _name;
    [SerializeField] private SpriteRenderer _sprite;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(load_image());
    }
    void Update()
    {
        if (_savePic)
        {
            SaveFunc();
            _savePic = false;
        }
    }

    public void SaveFunc()
    {
        SaveTextureToFileUtility.SaveRenderTextureToFile(_rt, _filepath + "/" + _name , SaveTextureToFileUtility.SaveTextureFileFormat.JPG);
       
        Debug.Log("ca");
    }


    IEnumerator started()
    {
        if (_image != null)
            _sprite.material.SetTexture("_Test", _image);
        _sprite.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        _sprite.gameObject.SetActive(false);
    }

    IEnumerator load_image()
    {
        string[] filePaths = Directory.GetFiles(@_filepath, "*.png"); // get every file in chosen directory with the extension.png
        WWW www = new WWW("file://" + filePaths[0]);                  // "download" the first file from disk
        yield return www;                                                               // Wait unill its loaded
        Texture2D new_texture = new Texture2D(512, 512);               // create a new Texture2D (you could use a gloabaly defined array of Texture2D )
        www.LoadImageIntoTexture(new_texture);                           // put the downloaded image file into the new Texture2D 
        _image = new_texture;           // put the new image into the current material as defuse material for testing.
        if (_image != null)
            StartCoroutine(started());
    }

}

