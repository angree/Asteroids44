using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroy_in_time : MonoBehaviour
{
    public float time_destroy = 2f;
    float time_exists = 0f;

    void Update()
    {
        time_exists += Time.deltaTime;
        if (time_exists > time_destroy) Destroy(this.gameObject);
    }
}
