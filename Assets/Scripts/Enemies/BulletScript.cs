using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public PlayerMovement playerScript;
    public MeshRenderer mesh1, mesh2, mesh3;
    public Collider col1, col2, col3;
    public float timer;

    private void Start()
    {
        playerScript = GameObject.Find("Player1").GetComponent<PlayerMovement>();
        timer = 20f;
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Snus")
        {
            playerScript.playerSpeed = 4f;
            mesh1.enabled = false;
            col1.enabled = false;
            mesh2.enabled = false;
            col2.enabled = false;
            mesh3.enabled = false;
            col3.enabled = false;
            Invoke("ResetSpeed", 3f);
        }
    }

    void ResetSpeed()
    {
        playerScript.playerSpeed = 6f;
    }
}
