using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButtons : MonoBehaviour
{
    public GameObject blackOutSquare;

    void Start()
    {
        StartCoroutine(FadeBlackOutSquare(false));
    }
    public void Play()
    {
        StartCoroutine(FadeBlackOutSquare(true));
        Invoke("DelayLoad", 2f);
    }


    void DelayLoad()
    {
        SceneManager.LoadScene("Level");
    }
    public void Quit()
    {
        StartCoroutine(FadeBlackOutSquare(true));
        Invoke("DelayQuit", 2f);
    }

    void DelayQuit()
    {
        Application.Quit();
        Debug.Log("Quitting...");
    }

    public IEnumerator FadeBlackOutSquare(bool fadeToBlack = true, int fadeSpeed = 1)
    {
        Color objectColor = blackOutSquare.GetComponent<Image>().color;
        float fadeAmount;

        if (fadeToBlack)
        {
            while (blackOutSquare.GetComponent<Image>().color.a < 1)
            {
                fadeAmount = objectColor.a + (fadeSpeed * Time.deltaTime);

                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                blackOutSquare.GetComponent<Image>().color = objectColor;
                yield return null;
            }
        }
        else
        {
            while (blackOutSquare.GetComponent<Image>().color.a > 0)
            {
                fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime);

                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                blackOutSquare.GetComponent<Image>().color = objectColor;
                yield return null;
            }
        }
    }
}
