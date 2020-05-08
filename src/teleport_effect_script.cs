using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleport_effect_script : MonoBehaviour
{
    float runtime = 0f;

    void Update()
    {
        runtime += Time.deltaTime;
        if (runtime > 2f) Destroy(this.gameObject);
    }
}
