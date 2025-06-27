using Assets.SimpleLocalization.Scripts;
using UnityEngine;

public class LocalizationController : MonoBehaviour
{
    public string LocalizationKey;

    public void SetLocalizationKey(string key)
    {
        LocalizationKey = key;
        LocalizationManager.Language = LocalizationKey;
        LocalizationManager.Read();
    }
}
