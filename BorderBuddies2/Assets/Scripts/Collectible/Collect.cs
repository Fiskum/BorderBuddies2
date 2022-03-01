using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect : MonoBehaviour
{
    
    //public AudioSource collectSound;

    private void Update()
    {
        transform.Rotate(new Vector3(15, 60, 30) * Time.deltaTime);
    }
    
    void OnTriggerEnter(Collider other)
    {
        //collectSound.Play();
        ScoringSystem.theScore += 1;
        Destroy(gameObject);
    }
}
