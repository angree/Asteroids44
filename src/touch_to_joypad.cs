using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class touch_to_joypad : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public int button_type = 0;

    Global_Variables _GlobalVariables;
    GameObject GC;

    void Start()
    {
        GC = GameObject.Find("GlobalVariables_holder");
        if (GC != null) _GlobalVariables = GC.GetComponent<Global_Variables>();
    }

    bool pressed = false;

    // Update is called once per frame
    void Update()
    {
        if (button_type == 1)
        {
            _GlobalVariables.press_left[1] = 0f;
            if (pressed) _GlobalVariables.press_left[1] = 1f;
        }
        if (button_type == 2)
        {
            _GlobalVariables.press_right[1] = 0f;
            if (pressed) _GlobalVariables.press_right[1] = 1f;
        }
        if (button_type == 3)
        {
            _GlobalVariables.press_up[1] = 0f;
            if (pressed) _GlobalVariables.press_up[1] = 1f;
        }
        if (button_type == 4)
        {
            _GlobalVariables.press_down[1] = 0f;
            if (pressed) _GlobalVariables.press_down[1] = 1f;
        }
        if (button_type == 5)
        {
            _GlobalVariables.press_b1[1] = 0f;
            if (pressed) _GlobalVariables.press_b1[1] = 1f;
        }
        if (button_type == 6)
        {
            _GlobalVariables.press_b2[1] = 0f;
            if (pressed) _GlobalVariables.press_b2[1] = 1f;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        pressed = true;
        Debug.Log("pressing button "+button_type);
    }

    public virtual void OnPointerUp(PointerEventData data)
    {
        pressed = false;
        Debug.Log("releasing button " + button_type);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        pressed = true;
        Debug.Log("pressing button " + button_type);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        pressed = false;
        Debug.Log("releasing button " + button_type);
    }
}
