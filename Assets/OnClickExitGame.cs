using UnityEngine;

public class OnClickExitGame : MonoBehaviour
{
    private void OnMouseDown()
    {
        Application.Quit();
    }

    public void QuitGame() => Application.Quit();
}
