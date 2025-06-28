using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.SimpleLocalization.Scripts
{
	/// <summary>
	/// Localize text component.
	/// </summary>
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class LocalizedTextMeshProUGUI : MonoBehaviour
    {
        public string LocalizationKey;
        private bool localized = false;

        public void Start()
        {
            if (!localized)
            {
                Localize();
                localized = true;
            }
            
            LocalizationManager.OnLocalizationChanged += Localize;
        }

        public void OnDestroy()
        {
            LocalizationManager.OnLocalizationChanged -= Localize;
        }

        public void Localize()
        {
            GetComponent<TextMeshProUGUI>().text = LocalizationManager.Localize(LocalizationKey);
            localized = true;
        }
    }
}