using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class transparency_decrease : MonoBehaviour
{
    public float start_time = 0f;
    public float fade_out_speed = 1f;

    float last_transparency = 0f;

    void Start()
    {
        Color get_color = gameObject.GetComponent<Renderer>().material.color;
        get_color.a = 0f;
        gameObject.GetComponent<Renderer>().material.color = get_color;
    }

    void Update()
    {
        Color get_color = gameObject.GetComponent<Renderer>().material.color;
        float alpha_channel = 0 - (start_time * fade_out_speed);
        if (alpha_channel > 1f) alpha_channel = 1f;
        if (alpha_channel < 0f) alpha_channel = 0f;
        get_color.a = alpha_channel;
        if (get_color.a != last_transparency) gameObject.GetComponent<Renderer>().material.color = get_color;
        last_transparency = get_color.a;
        start_time -= Time.deltaTime;

    }
}
