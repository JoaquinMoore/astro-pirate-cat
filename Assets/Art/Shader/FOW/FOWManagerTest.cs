using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FOWManagerTest : MonoBehaviour
{
    public bool SavePic;
    public RenderTexture _rt;
    public Texture2D image;
    public string filepath;
    public string LoadFilePath;
    public string Name;
    public SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        image = (Texture2D)AssetDatabase.LoadAssetAtPath(LoadFilePath + "/" + Name + ".png", typeof(Texture2D));


        if (image != null)
            StartCoroutine(started());

    }

    // Update is called once per frame
    void Update()
    {
        if (SavePic)
        {

            SaveFunc();
            SavePic = false;
        }
    }

    public void SaveFunc()
    {
        SaveTextureToFileUtility.SaveRenderTextureToFile(_rt, filepath + "/" + Name , SaveTextureToFileUtility.SaveTextureFileFormat.PNG);
        Debug.Log("ca");
    }


    IEnumerator started()
    {
        if (image != null)
            sprite.material.SetTexture("_Test", image);
        sprite.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        sprite.gameObject.SetActive(false);

    }
}

