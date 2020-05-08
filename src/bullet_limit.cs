using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet_limit : MonoBehaviour
{

    float time_exists = 0;

    void Update()
    {
        time_exists += Time.deltaTime;
        if (time_exists > 0.9f) Destroy(this.gameObject);

        if (transform.localPosition.x < -107.5f) transform.localPosition = new Vector3(transform.localPosition.x + 215.0f, transform.localPosition.y, transform.localPosition.z);
        if (transform.localPosition.x > 107.5f) transform.localPosition = new Vector3(transform.localPosition.x - 215.0f, transform.localPosition.y, transform.localPosition.z);

        if (transform.localPosition.y < -62.2f) transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + 124.4f, transform.localPosition.z);
        if (transform.localPosition.y > 62.2f) transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - 124.4f, transform.localPosition.z);

    }
}
