using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowRotate_all : MonoBehaviour
{
    public float rot_up = 1f;
    public float rot_left = 1f;
    public float rot_forward = 1f;

    public string split = "------------";
    public float go_x = 0f;
    public float go_y = 0f;
    public float go_z = 0f;

    void FixedUpdate()
    {
        transform.Rotate(Time.deltaTime * rot_left, Time.deltaTime * rot_up, Time.deltaTime * rot_forward, Space.Self);
        transform.localPosition = new Vector3(transform.localPosition.x + (go_x * Time.deltaTime), transform.localPosition.y + (go_y * Time.deltaTime), transform.localPosition.z + (go_z * Time.deltaTime));
    }
}
