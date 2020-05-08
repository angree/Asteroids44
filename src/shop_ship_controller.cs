using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shop_ship_controller : MonoBehaviour
{

    Global_Variables _GlobalVariables;
    GameObject GC;

    public int player_number = 1;
    float scroll_speed = 6f;

    void Start()
    {
        GC = GameObject.Find("GlobalVariables_holder");
        if (GC != null) _GlobalVariables = GC.GetComponent<Global_Variables>();
    }

    void Update()
    {
        float position_difference = 0f;
        if (player_number == 1) position_difference = transform.localPosition.y - (-88.66f * _GlobalVariables.menu_position[1]);
        if (player_number == 2) position_difference = transform.localPosition.y - (-88.66f * _GlobalVariables.menu_position[2]);
        if (player_number == 3) position_difference = transform.localPosition.y - (-88.66f * _GlobalVariables.menu_position[3]);
        if (player_number == 4) position_difference = transform.localPosition.y - (-88.66f * _GlobalVariables.menu_position[4]);
        position_difference = position_difference * scroll_speed * Time.deltaTime*2f;
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - position_difference, transform.localPosition.z);

    }
}
