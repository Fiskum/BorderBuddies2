using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPanel : MonoBehaviour
{
    public GameObject controlText;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            controlText.SetActive(false);
        }
    }

}
