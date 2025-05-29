using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneController : MonoBehaviour
{
    public Image fader1;
    public Image fader2;
    public Image fader3;
    public Image fader;
    private static SceneController instance;
    
    
    void Awake(){


        if (instance== null){
            instance = this;
            DontDestroyOnLoad(gameObject);

            float select = Random.value;
            if (select <=0.33){
                fader = fader1;
            }
            else if (select<=0.66){
                fader = fader2;
            }
            else{
                fader = fader3;
            }

            fader.rectTransform.sizeDelta = new Vector2(fader.rectTransform., Camera.main.pixelHeight);
            fader.gameObject.SetActive(false);
            fader1.gameObject.SetActive(false);
            fader2.gameObject.SetActive(false);
            fader3.gameObject.SetActive(false);
        }

        else{
            Destroy(gameObject);
        }
    }

    public static void LoadScene(int index, float duration = 1, float waitTime = 0){
        instance.StartCoroutine(instance.FadeScene(index, duration, waitTime));
    }

    public static void LoadScene(string index, float duration = 1, float waitTime = 0)
    {
        instance.StartCoroutine(instance.FadeScene(index, duration, waitTime));
    }

    private IEnumerator FadeScene(int index, float duration, float waitTime){
        fader.gameObject.SetActive(true);

        for (float t = 0; t < 1; t+= Time.deltaTime / duration){
            fader.color = new Color (1,1,1,Mathf.Lerp(0,1,t));
            yield return null;
        }
        SceneManager.LoadScene(index);

        yield return new WaitForSeconds(waitTime);

         for (float t = 0; t < 1; t+= Time.deltaTime / duration){
            fader.color = new Color (1,1,1,Mathf.Lerp(1,0,t));
            yield return null;
        }

        fader.gameObject.SetActive(false);
        fader1.gameObject.SetActive(false);
        fader2.gameObject.SetActive(false);
        fader3.gameObject.SetActive(false);
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
        fader1.gameObject.SetActive(false);
        fader2.gameObject.SetActive(false);
        fader3.gameObject.SetActive(false);
    }
}
