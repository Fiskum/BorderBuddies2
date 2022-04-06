using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlPanel : MonoBehaviour
{
    public GameObject controlText;

    public float timer = 1f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            controlText.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKey(KeyCode.R))
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                SceneManager.LoadScene("Level");
            }
        }

        if (Input.GetKeyUp(KeyCode.R))
        {
            timer = 1f;
        }
    }

}
