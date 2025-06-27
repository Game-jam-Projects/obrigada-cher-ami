using UnityEngine;

[CreateAssetMenu(fileName = "LocalizationSettings", menuName = "Scriptable Objects/LocalizationSettings")]
public class LocalizationSettings : ScriptableObject
{
    public LocalizationKey LocalizationKey;
}

public enum LocalizationKey
{
    ptbr,
    enus
}