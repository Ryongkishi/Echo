using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncHuman : BaseHuman
{
    // Start is called before the first frame update
    public static void PrintStr(string str){
        Debug.Log("printStr"+str);
    }
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }
}
