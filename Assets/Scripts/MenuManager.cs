using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject selectionPanel;

    private void Start()
    {
        selectionPanel.SetActive(false);
    }

    public void Selection()
    {
        selectionPanel.SetActive(true);

        menuPanel.SetActive(false);
    }

    public void Menu()
    {
        menuPanel.SetActive(true);

        selectionPanel.SetActive(false);
    }
}
