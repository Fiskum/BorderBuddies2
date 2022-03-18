using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect : MonoBehaviour
{
    public MeshRenderer mesh;
    public AudioSource collectSound;

    private void Update()
    {
        transform.Rotate(new Vector3(15, 60, 30) * Time.deltaTime);
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Snus")
        {
            mesh.enabled = false;
            collectSound.Play();
            ScoringSystem.theScore += 1;
            Invoke("Destroy", 3f);

        }
        
    }
    void Destroy()
    {
        Destroy(gameObject);
    }
}
