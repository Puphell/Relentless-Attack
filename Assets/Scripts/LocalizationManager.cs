using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class LocalizationManager : MonoBehaviour
{
    [SerializeField] private Button _turkishButton;

    [SerializeField] private Button _englishButton;

    [SerializeField] private Button _spanishButton;

    [SerializeField] private Button _russianButton;

    private void Awake()
    {
        _turkishButton.onClick.AddListener(() => SetLanguage(LanguageType.Turkish));
        _englishButton.onClick.AddListener(() => SetLanguage(LanguageType.English));
        _spanishButton.onClick.AddListener(() => SetLanguage(LanguageType.Spanish));
        _russianButton.onClick.AddListener(() => SetLanguage(LanguageType.Russian));
    }

    public void SetLanguage(LanguageType languageType)
    {
        foreach (var locale in LocalizationSettings.AvailableLocales.Locales)
        {
            if (locale.LocaleName.Equals(languageType.ToString()))
            {
                LocalizationSettings.SelectedLocale = locale;
                Debug.Log("Language set to: " + locale.LocaleName);
                return;
            }
        }

        Debug.LogWarning("Locale not found for " + languageType.ToString());
    
    }
}
