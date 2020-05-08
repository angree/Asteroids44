using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Joypad_controller_script : MonoBehaviour
{
    General_settings _General_settings;
    GameObject GS;

    public int joypad_number = 0;

    public int joypad_setup = 1;
    //0 - analog and dpad normal
    //1 - analog/digital l/r + r/l trigger

    float dead_zone = 0.3f;
    int random_number = 0;
    float run_time = 0f;
    bool control_mode_changed = false;

    void Start()
    {
        GS = GameObject.Find("Settings_holder");
        if (GS != null) _General_settings = GS.GetComponent<General_settings>();
        random_number = Random.Range(0, 1000000000);
        reassing_controller();
    }

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void reassing_controller()
    {
        //Debug.Log("reassigning controllers");
        bool already_assigned = false;
        for (int i = 1; i <= 4; i++)
        {
            if ((_General_settings.joypad_last_call[i] == -1f) || (_General_settings.joypad_last_call[i] < (_General_settings.joypad_timer - dead_zone)))
            {
                _General_settings.joypad_id[i] = random_number;
                _General_settings.joypad_last_call[i] = _General_settings.joypad_timer;
                joypad_number = i;
                break;
            }
        }
    }


    void Update()
    {
        if (_General_settings.loaded_scene == 1)
        {
            joypad_setup = 1;
            control_mode_changed = false;
        }
        if (_General_settings.loaded_scene == 2)
        {
            if (control_mode_changed == false) joypad_setup = _General_settings.player_controller_profile[joypad_number];
        }
        if (joypad_number == 0)
        {
            reassing_controller();
        }
        if (joypad_number >= 1)
        {
            if (_General_settings.joypad_id[joypad_number] == random_number) _General_settings.joypad_last_call[joypad_number] = _General_settings.joypad_timer;
            else reassing_controller();
        }
        run_time += Time.deltaTime;
    }

    void OnMove(InputValue value)
    {
        Vector2 analog_stick;
        analog_stick = value.Get<Vector2>();
        //x axis
        if (joypad_number > 0) _General_settings.joypad_x[joypad_number] = analog_stick.x;
        //y axis
        if (joypad_number > 0)
        {
            if ((joypad_setup == 1) || (_General_settings.normal_joypad_controls_forced == true)) _General_settings.joypad_y[joypad_number] = analog_stick.y;
        }
    }

    void OnMoveDigital(InputValue value)
    {
        Vector2 analog_stick;
        analog_stick = value.Get<Vector2>();
        _General_settings.joypad_x[joypad_number] = analog_stick.x;
        if ((joypad_setup == 1) && (joypad_number > 0)) _General_settings.joypad_y[joypad_number] = analog_stick.y;
    }

    void OnTrigger_R(InputValue value)
    {
        float trigger_r;
        trigger_r = value.Get<float>();
        if (joypad_number > 0)
        {
            if ((trigger_r > 0.5f) && (joypad_setup == 0))
                _General_settings.joypad_y[joypad_number] = 1;
            else
                _General_settings.joypad_y[joypad_number] = 0;

        }
    }

    void OnTrigger_L(InputValue value)
    {
        float trigger_l;
        trigger_l = value.Get<float>();
        //Debug.Log(run_time+" ltrigger = " + trigger_l);
        if (joypad_number > 0)
        {
            if ((trigger_l > 0.5f) && (joypad_setup == 0))
                _General_settings.joypad_y[joypad_number] = -1;
            else
                _General_settings.joypad_y[joypad_number] = 0;

        }
    }

    void OnButton1(InputValue value)
    {
        float button_1;
        button_1 = value.Get<float>();
        if (joypad_number > 0) _General_settings.joypad_button1[joypad_number] = button_1;
        //Debug.Log("button1 detected");
    }
    void OnButton2(InputValue value)
    {
        float button_2;
        button_2 = value.Get<float>();
        if (joypad_number > 0) _General_settings.joypad_button2[joypad_number] = button_2;
    }


    void OnChangeControls(InputValue value)
    {
        float select;
        select = value.Get<float>();
        if (select > 0.5f) joypad_setup++;
        if (joypad_setup >= 2) joypad_setup = 0;
        control_mode_changed = true;
    }
}
