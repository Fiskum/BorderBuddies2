using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    PlayerMovement playerScript;
    Animator playerAnim;
    public MeshRenderer mesh1, mesh2, mesh3;
    public Collider col1, col2, col3;
    float timer;

    [Header("Parameters")]
    [Range(1f, 6f)]
    public float slowSpeed = 4f;
    [Range(1f, 10f)]
    public float slowDuration = 3f;

    private void Start()
    {
        playerScript = GameObject.Find("Player1").GetComponent<PlayerMovement>();
        playerAnim = GameObject.Find("CharacterNorway").GetComponent<Animator>();
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
            playerAnim.SetBool("Injured", true);
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
        playerAnim.SetBool("Injured", false);
    }
}
