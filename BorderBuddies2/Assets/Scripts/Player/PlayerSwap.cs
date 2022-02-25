using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwap : MonoBehaviour
{
    public GameObject cam1;
    public GameObject cam2;

    public bool player1Active;
    public bool player2Active = false;

    void Start()
    {
        player1Active = true;
        player2Active = false;
        cam1.SetActive(true);
        cam2.SetActive(false);
    }
    void Update()
    {
        if (player1Active == false)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                player1Active = true;
                player2Active = false;
                cam2.SetActive(false);
                cam1.SetActive(true);
            }
        }

        if (player2Active == false)
        {
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                player1Active = false;
                player2Active = true;
                cam1.SetActive(false);
                cam2.SetActive(true);
            }
        }
    }
}
