using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class title_flying_ship : MonoBehaviour
{
    public float speed = 1f;
    public float start_z = -20f;
    public float end_z = 20f;

    void FixedUpdate()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + (speed * Time.deltaTime));
        if (transform.localPosition.z > end_z) transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z - (end_z - start_z));
    }
}
