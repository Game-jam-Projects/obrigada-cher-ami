using UnityEngine;

public class WebsiteManager : MonoBehaviour
{
    public void OpenUrl(string url) => Application.OpenURL(url);
}
