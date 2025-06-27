using Assets.SimpleLocalization.Scripts;
using UnityEngine;

public class LocalizationController : MonoBehaviour
{
    public LocalizationSettings LocalizationSettings;

    private void Start()
    {
        SetLocalizationKey();
    }

    private void SetLocalizationKey()
    {
        LocalizationManager.Language = LocalizationSettings.LocalizationKey.ToString();
        LocalizationManager.Read();
    }

    public void SetLocalizationKey(string key)
    {
        LocalizationSettings.LocalizationKey = key switch
        {
            "ptbr" => LocalizationKey.ptbr,
            "enus" => LocalizationKey.enus,
            _ => LocalizationSettings.LocalizationKey
        };

        SetLocalizationKey();
    }   
}
