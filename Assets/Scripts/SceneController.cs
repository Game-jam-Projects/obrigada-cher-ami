using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    public Image fader;
    private static SceneController instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            fader.rectTransform.sizeDelta = new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight);
            fader.gameObject.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void LoadScene(int index, float duration = 1, float waitTime = 0)
        => instance.StartCoroutine(instance.FadeScene(index, duration, waitTime));

    public static void LoadScene(string index, float duration = 1, float waitTime = 0)
        => instance.StartCoroutine(instance.FadeScene(index, duration, waitTime));

    private IEnumerator FadeScene(int index, float duration, float waitTime)
    {
        fader.gameObject.SetActive(true);

        for (float t = 0; t < 1; t += Time.deltaTime / duration)
        {
            fader.color = new Color(1, 1, 1, Mathf.Lerp(0, 1, t));
            yield return null;
        }
        SceneManager.LoadScene(index);

        yield return new WaitForSeconds(waitTime);

        for (float t = 0; t < 1; t += Time.deltaTime / duration)
        {
            fader.color = new Color(1, 1, 1, Mathf.Lerp(1, 0, t));
            yield return null;
        }

        fader.gameObject.SetActive(false);
    }

    private IEnumerator FadeScene(string index, float duration, float waitTime)
    {
        fader.gameObject.SetActive(true);

        for (float t = 0; t < 1; t += Time.deltaTime / duration)
        {
            fader.color = new Color(1, 1, 1, Mathf.Lerp(0, 1, t));
            yield return null;
        }
        SceneManager.LoadScene(index);

        yield return new WaitForSeconds(waitTime);

        for (float t = 0; t < 1; t += Time.deltaTime / duration)
        {
            fader.color = new Color(1, 1, 1, Mathf.Lerp(1, 0, t));
            yield return null;
        }

        fader.gameObject.SetActive(false);
    }
}
