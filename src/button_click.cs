using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class button_click : MonoBehaviour
{
    General_settings _General_settings;
    GameObject GS;

    public int button_no = 1;

    float run_timer = 0f;

    void Start()
    {
        GS = GameObject.Find("Settings_holder");
        if (GS != null) _General_settings = GS.GetComponent<General_settings>();
        Button btn = this.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        //Debug.Log("setting mode to " + _General_settings.game_mode_selected);
        _General_settings.game_mode_selected = button_no;
    }


    void Update()
    {

        if (_General_settings.game_mode_select_position != button_no)
        {
            run_timer = 0f;
        }
        if (button_no == 1)
        {
            Color newColor = new Color(29f/255f, 35f / 255f, 154f / 255f, 0.5f);
            if (run_timer > 0.25f) newColor = new Color(59f/255f, 75f/255f, 184f/255f, 0.5f);
            GetComponent<Image>().color = newColor;
        }
        if (button_no == 2)
        {
            Color newColor = new Color(29f/255f, 154f / 255f, 35 / 255f, 0.5f);
            if (run_timer > 0.25f) newColor = new Color(59f/255f, 184f/255f, 75f/255f, 0.5f);
            GetComponent<Image>().color = newColor;
        }
        if (button_no == 3)
        {
            Color newColor = new Color(154f/255f, 29f / 255f, 35f / 255f, 0.5f);
            if (run_timer > 0.25f) newColor = new Color(184f/255f, 59f / 255f, 75f / 255f, 0.5f);
            GetComponent<Image>().color = newColor;
        }
        run_timer += Time.deltaTime;
        while (run_timer > 0.5f) run_timer -= 0.5f;
    }
}
