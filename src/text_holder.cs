using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class text_holder : MonoBehaviour
{
    public Text Energy_Text;
    public Text Multipler_Text;

    public Image Arrow;

    int my_energy = 100;
    int player_number = 0;
    [HideInInspector]
    public Global_Variables _GlobalVariables;
    [HideInInspector]
    public GameObject GC;

    Player_01_move parent;

    void Start()
    {
        GC = GameObject.Find("GlobalVariables_holder");
        if (GC != null) _GlobalVariables = GC.GetComponent<Global_Variables>();

        parent = GetComponentInParent<Player_01_move>();
    }

    void Update()
    {
        Vector3 namePos = Camera.main.WorldToScreenPoint(this.transform.position);

        bool hide_text = false;
        if ((player_number == 1) && (_GlobalVariables.Energy_Player[1] <= 0)) hide_text = true;
        if ((player_number == 2) && (_GlobalVariables.Energy_Player[2] <= 0)) hide_text = true;
        if ((player_number == 3) && (_GlobalVariables.Energy_Player[3] <= 0)) hide_text = true;
        if ((player_number == 4) && (_GlobalVariables.Energy_Player[4] <= 0)) hide_text = true;

        if (hide_text)
        {
            Energy_Text.transform.position = new Vector3(-2000f, 0f, 0f);
            Multipler_Text.transform.position = new Vector3(-2000f, 0f, 0f);
            Arrow.transform.position = new Vector3(-2000f, 0f, 0f);
        }
        else
        {
            Energy_Text.transform.position = namePos;
            Multipler_Text.transform.position = namePos;
            Arrow.transform.position = namePos;
            namePos = Arrow.transform.localPosition;

            float x_pos = namePos.x;
            float y_pos = namePos.y;
            float z_pos = namePos.z;

            bool visible = false;

            float object_rotation = 0f; //left

            if (x_pos > -2000)
            {
                if (x_pos > 975f) { x_pos = 935f; visible = true; object_rotation = 180f; }
                if (x_pos < -975f) { x_pos = -935f; visible = true; object_rotation = 0f; }
                if (y_pos > 552f) { y_pos = 512f; visible = true; object_rotation = 270f; }
                if (y_pos < -552f) { y_pos = -512f; visible = true; object_rotation = 90f; }
            }


            if (visible == false) x_pos = -14000;
            Vector3 namePos_limited = new Vector3(x_pos, y_pos, z_pos);
            Arrow.transform.localPosition = namePos_limited;
            Arrow.transform.localRotation = Quaternion.Euler(0, 0, object_rotation);
        }
        player_number = parent.player_number;

        Energy_Text.text = Mathf.Round(_GlobalVariables.Energy_Player[player_number]) + "/" + Mathf.Round(_GlobalVariables.Max_Energy_Player[player_number]);
        if(_GlobalVariables.player_score_multipler[player_number]>=1.3f)
        {
            float temp_holder_f = _GlobalVariables.player_score_multipler[player_number] * 10;
            int temp_holder_i = (int)temp_holder_f;
            temp_holder_f = temp_holder_i;
            temp_holder_f = temp_holder_f / 10;
            int text_size = (int)(22 + (temp_holder_f * 3));
            if (text_size > 35) text_size = 35;
            Color new_color = new Color(0.4f, 1f, 0.4f);
            if (temp_holder_f < 3f) new_color = new Color(0.4f + ((temp_holder_f - 1f) * 0.3f), 1f, 0.4f);
            else if (temp_holder_f < 5f) new_color = new Color(1f, 1f - ((temp_holder_f - 3f)*0.3f), 0.4f);
            else if (temp_holder_f < 7f) new_color = new Color(1f, 0.4f, 0.4f + ((temp_holder_f - 5f) * 0.3f));
            else new_color = new Color(1f, 0.4f, 1f);
            Multipler_Text.color = new_color;
            Multipler_Text.fontSize = text_size;
            Multipler_Text.text = "x "+ temp_holder_f;
        }
        else
        {
            Multipler_Text.text = "";
        }
        //x1.5x

        //Debug.Log("Player's Parent: " + player_number);

    }
}
