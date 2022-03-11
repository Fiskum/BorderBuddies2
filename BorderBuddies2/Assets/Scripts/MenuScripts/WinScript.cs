using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer ("Player1"))
        {
            Invoke("LoadWin", 2f);
        }
    }

    void LoadWin()
    {
        SceneManager.LoadScene("WinScreen");
    }
}
