using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class energy_bar_gone : MonoBehaviour
{
    [HideInInspector]
    public Global_Variables _GlobalVariables;
    [HideInInspector]
    public GameObject GC;
    public int this_player = 1;

    void Start()
    {
        GC = GameObject.Find("GlobalVariables_holder");
        if (GC != null) _GlobalVariables = GC.GetComponent<Global_Variables>();
    }

}
