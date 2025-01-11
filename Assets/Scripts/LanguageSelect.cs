using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LanguageSelect : MonoBehaviour
{
    public GameObject LanguagePanel;

    public void SelectLanguage()
    {
        if (LanguagePanel.activeSelf)
        {
            LanguagePanel.SetActive(false);
        }
        else
        {
            LanguagePanel.SetActive(true);
        }
    }
}
