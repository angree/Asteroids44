using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class start_button_script : MonoBehaviour
{
    General_settings _General_settings;
    GameObject GS;

    Button btn;
    ColorBlock initial_colorBlock;
    ColorBlock initial_highlight_colorBlock;

    float run_time = 0f;

    void Start()
    {
        GS = GameObject.Find("Settings_holder");
        if (GS != null) _General_settings = GS.GetComponent<General_settings>();
        btn = this.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);

        initial_colorBlock = btn.colors;
        initial_highlight_colorBlock = btn.colors;
        initial_highlight_colorBlock.normalColor = new Color(0.5f, 0.5f, 0.5f, 0.7f);
        initial_highlight_colorBlock.highlightedColor = new Color(0.6f, 0.6f, 0.6f, 0.7f);
        initial_highlight_colorBlock.selectedColor = new Color(0.4f, 0.4f, 0.4f, 0.7f);
    }

    void TaskOnClick()
    {
        _General_settings.Start_game();
    }

    void Update()
    {
        if (_General_settings.game_mode_select_position == 6) btn.colors = initial_highlight_colorBlock;
        if (run_time < 0.25f) btn.colors = initial_colorBlock;
        run_time += Time.deltaTime;
        while (run_time > 0.5f) run_time -= 0.5f;
    }
}
