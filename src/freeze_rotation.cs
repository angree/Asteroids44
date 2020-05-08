using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class freeze_rotation : MonoBehaviour
{
    void Update()
    {
        transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0f);

    }
}
