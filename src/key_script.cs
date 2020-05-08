using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class key_script : MonoBehaviour
{

    public Text debug_text;

    void Update()
    {
        string output = "w = " + Input.GetKey(KeyCode.W) + " \n";
        output += "s = " + Input.GetKey(KeyCode.S) + " \n";
        output += "a = " + Input.GetKey(KeyCode.A) + " \n";
        output += "d = " + Input.GetKey(KeyCode.D) + " \n";
        debug_text.text = output;
    }
}
