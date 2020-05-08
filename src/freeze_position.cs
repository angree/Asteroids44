using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class freeze_position : MonoBehaviour
{

    void Update()
    {
        transform.localPosition = new Vector3(0f, 0f, 0f);
    }
}
