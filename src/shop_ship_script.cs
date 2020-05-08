using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shop_ship_script : MonoBehaviour
{
    Global_Variables _GlobalVariables;
    GameObject GC;
    GameObject main_engines;
    GameObject reverse_engines;

    AudioSource audioData;

    int last_shop_time = 0;
    float timer_go = 0f;

    void Start()
    {
        GC = GameObject.Find("GlobalVariables_holder");
        if (GC != null) _GlobalVariables = GC.GetComponent<Global_Variables>();
        audioData = GetComponent<AudioSource>();
        main_engines = GameObject.Find("ShopEnginesBack");
        reverse_engines = GameObject.Find("ShopEnginesFront");
    }

    void Update()
    {
        if(_GlobalVariables.shop_time!=last_shop_time)
        {
            if(_GlobalVariables.shop_time==1)
            {
                //start coming
                timer_go = 0f;
                audioData.Play(0);
            }
            else if(_GlobalVariables.shop_time==3)
            {
                //start flying away
                timer_go = 0f;
            }
            last_shop_time = _GlobalVariables.shop_time;
        }
        if (_GlobalVariables.shop_time == 1)
        {
            float timer_go_copy = (((5f-timer_go) * (5f - timer_go) * (5f - timer_go)) /(5*5)); //5 > 0   wczesniej 0 -> 5
            if (timer_go_copy < 0f)
            {
                timer_go_copy = 0f;
                _GlobalVariables.shop_time++;
            }
            transform.localPosition = new Vector3(transform.localPosition.x, (((timer_go_copy) / 5f) * -300f), transform.localPosition.z);
        }
        if (_GlobalVariables.shop_time == 3)
        {
            float timer_go_copy = (timer_go * timer_go * timer_go) /(5*5);
            if (timer_go_copy > 5f)
            {
                timer_go_copy = 5f;
                _GlobalVariables.shop_time++;
                audioData.Stop();
            }
            transform.localPosition = new Vector3(transform.localPosition.x, (300f-(((5f - timer_go_copy) / 5f) * 300f)), transform.localPosition.z);
        }
        if (_GlobalVariables.shop_ship_main_engines > 0f)
        {
            main_engines.SetActive(true);
        }
        else
        {
            main_engines.SetActive(false);
        }
        if (_GlobalVariables.shop_ship_rear_engines > 0f)
        {
            reverse_engines.SetActive(true);
        }
        else
        {
            reverse_engines.SetActive(false);
        }

        timer_go += Time.deltaTime;

    }
}
