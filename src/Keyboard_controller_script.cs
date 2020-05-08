using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Keyboard_controller_script : MonoBehaviour
{
    General_settings _General_settings;
    GameObject GS;

    public int keyboard_number = 0;

    void Start()
    {
        GS = GameObject.Find("Settings_holder");
        if (GS != null) _General_settings = GS.GetComponent<General_settings>();
    }

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    float run_time = 0f;

    void Update()
    {
        run_time += Time.deltaTime;
    }


    void OnMoveLeft(InputValue value)
    {
        float directional_key;
        directional_key = value.Get<float>();
        _General_settings.keyboard_x[keyboard_number] = -directional_key;
    }
    void OnMoveRight(InputValue value)
    {
        float directional_key;
        directional_key = value.Get<float>();
        _General_settings.keyboard_x[keyboard_number] = +directional_key;
    }
    void OnMoveUp(InputValue value)
    {
        float directional_key;
        directional_key = value.Get<float>();
        _General_settings.keyboard_y[keyboard_number] = +directional_key;
    }
    void OnMoveDown(InputValue value)
    {
        float directional_key;
        directional_key = value.Get<float>();
        _General_settings.keyboard_y[keyboard_number] = -directional_key;
    }

    void OnButton1(InputValue value)
    {
        float button_1;
        button_1 = value.Get<float>();
        _General_settings.keyboard_button1[keyboard_number] = button_1;
    }


    void OnButton2(InputValue value)
    {
        float button_2;
        button_2 = value.Get<float>();
        _General_settings.keyboard_button2[keyboard_number] = button_2;
    }

    void OnEscape(InputValue value)
    {
        float input_key;
        input_key = value.Get<float>();
        _General_settings.keyboard_escape = input_key;
    }

}
