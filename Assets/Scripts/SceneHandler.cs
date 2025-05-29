using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{    
    public void LoadNextScene(int i)
    {
        SceneController.LoadScene(i, 1, 1);
    }

    public void LoadNextScene(string i)
    {
        SceneController.LoadScene(i, 1, 1);
    }

}
