using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button_changing_player_controls : MonoBehaviour
{
    General_settings _General_settings;
    GameObject GS;

    Button btn;
    ColorBlock initial_colorBlock;
    ColorBlock initial_highlight_colorBlock;
    ColorBlock initial_bright_highlight_colorBlock;
    ColorBlock gray_colorBlock;

    public int player_no = 1;

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

        initial_bright_highlight_colorBlock = btn.colors;
        initial_bright_highlight_colorBlock.normalColor = new Color(0.8f, 0.8f, 0.8f, 0.8f);
        initial_bright_highlight_colorBlock.highlightedColor = new Color(0.85f, 0.85f, 0.85f, 0.7f);
        initial_bright_highlight_colorBlock.selectedColor = new Color(0.7f, 0.7f, 0.7f, 0.7f);

        gray_colorBlock = initial_colorBlock;
        gray_colorBlock.normalColor = new Color(0.1f, 0.1f, 0.1f, 0.7f);
        gray_colorBlock.highlightedColor = new Color(0.2f, 0.2f, 0.2f, 0.7f);
        gray_colorBlock.selectedColor = new Color(0.2f, 0.2f, 0.2f, 0.7f);

    }

    void TaskOnClick()
    {
        switch(player_no)
        {
            case 1: _General_settings.change_controller(1); break;
            case 2: _General_settings.change_controller(2); break;
            case 3: _General_settings.change_controller(3); break;
            case 4: _General_settings.change_controller(4); break;
        }

    }

    float run_time = 0f;
    void Update()
    {
        if (_General_settings.player_controller_scheme[player_no] == 8)
        {
            if ((_General_settings.game_mode_select_position == player_no) && (player_no != 3)) btn.colors = initial_highlight_colorBlock;
            if ((_General_settings.game_mode_select_position == player_no) && (player_no == 3)) btn.colors = initial_bright_highlight_colorBlock;
            if (run_time < 0.25f) btn.colors = gray_colorBlock;
        }
        else
        {
            if (_General_settings.game_mode_select_position == player_no) btn.colors = initial_highlight_colorBlock;
            if(run_time < 0.25f) btn.colors = initial_colorBlock;
        }
        
        run_time += Time.deltaTime;
        while(run_time > 0.5f) run_time -= 0.5f;
    }
}
