using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class LanguageSelector : MonoBehaviour
{
    [SerializeField] private Button englishButton;
    [SerializeField] private Button italianButton;
    [SerializeField] private Button spanishButton;

    private void Start()
    {
        englishButton.onClick.AddListener(() => ChangeLanguage("en"));
        italianButton.onClick.AddListener(() => ChangeLanguage("it"));
        spanishButton.onClick.AddListener(() => ChangeLanguage("es"));
    }

    private void ChangeLanguage(string localeCode)
    {
        for (int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; i++)
        {
            if (LocalizationSettings.AvailableLocales.Locales[i].Identifier.Code == localeCode)
            {
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[i];
                break;
            }
        }
    }
}