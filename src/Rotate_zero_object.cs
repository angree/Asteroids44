using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate_zero_object : MonoBehaviour
{

    Global_Variables _GlobalVariables;
    GameObject GC;

    public float rot_up = 0.2f;
    public float rot_left = 0.3f;
    public float rot_forward = 0.4f;
    public bool space_travel = false;

    void Start()
    {
        transform.rotation = Quaternion.Euler(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
        GC = GameObject.Find("GlobalVariables_holder");
        if (GC != null) _GlobalVariables = GC.GetComponent<Global_Variables>();
    }

    void FixedUpdate()
    {
        float rot_left_temp = rot_left;
        if(space_travel)
        {
            if ((_GlobalVariables.shop_ship_main_engines > 0f) && (_GlobalVariables.shop_ship_main_engines <= 1f)) { rot_left_temp += 2f * _GlobalVariables.shop_ship_main_engines; }
            if ((_GlobalVariables.shop_ship_rear_engines > 0f) && (_GlobalVariables.shop_ship_rear_engines <= 1f)) { rot_left_temp += 2f * _GlobalVariables.shop_ship_rear_engines; }
        }

        transform.Rotate(Time.deltaTime * rot_left_temp, Time.deltaTime * rot_up, Time.deltaTime * rot_forward, Space.Self);
    }
}
