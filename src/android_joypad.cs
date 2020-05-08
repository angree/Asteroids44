using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class android_joypad : MonoBehaviour
{

    Global_Variables _GlobalVariables;
    GameObject GC;
    void Start()
    {
        GC = GameObject.Find("GlobalVariables_holder");
        if (GC != null) _GlobalVariables = GC.GetComponent<Global_Variables>();
        if (_GlobalVariables.android == 0) Destroy(this);
    }

    void Update()
    {
        
    }
}
