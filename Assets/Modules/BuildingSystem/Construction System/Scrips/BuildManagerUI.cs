using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildManagerUI : MonoBehaviour
{
    public static BuilderManager Manager;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(Manager);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SendData(UnNamedClass3 Data)
    {
        Manager.GetData(default);
    }


    public class UnnamedClass1
    {
        public Button ButtonRef;
        public UnNamedClass3 Data;

        public BuildManagerUI Father;
        public void SetUp()
        {

        }

        public void Call()
        {
            Father.SendData(Data);
        }
    }


}
