using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    public float fadeDuration = 2;
    public float waitTime = 0.3f;

    public void LoadNextScene(int i)
    {
        SceneController.LoadScene(i, fadeDuration, waitTime);
    }

    public void LoadNextScene(string i)
    {
        SceneController.LoadScene(i, fadeDuration, waitTime);
    }

}
