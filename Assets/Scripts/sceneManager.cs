using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneManager : MonoBehaviour
{
    
        public void LoadNextScene(int i)
        {
            SceneController.LoadScene(i, 1, 1);
        }
    
    
}
