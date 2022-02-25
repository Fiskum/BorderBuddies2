using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRotate : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(new Vector3(0, 10, 0) * Time.deltaTime);
    }
}
