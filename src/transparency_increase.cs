using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class transparency_increase : MonoBehaviour
{
    // Start is called before the first frame update
    float start_time = 0f;
    public float fade_out_speed = 1f;

    float last_transparency = -1f;
    void Start()
    {
        Color get_color = gameObject.GetComponent<Renderer>().material.color;
        get_color.a = 1f;
        gameObject.GetComponent<Renderer>().material.color = get_color;
    }

    // Update is called once per frame
    void Update()
    {
        Color get_color = gameObject.GetComponent<Renderer>().material.color;
        float alpha_channel = 1f-(start_time*fade_out_speed);
        if (alpha_channel < 0f) alpha_channel = 0f;
        get_color.a = alpha_channel;
        if(get_color.a!= last_transparency) gameObject.GetComponent<Renderer>().material.color = get_color;
        last_transparency = get_color.a;
        start_time += Time.deltaTime;

    }
}
