using System.Collections.Generic;
using UnityEngine;
using Playgama.Common;
using Playgama.Modules.Advertisement;
using UnityEngine.SceneManagement;
using Playgama;

public class AdManager : MonoBehaviour
{
    private void Start()
    {
        Bridge.advertisement.interstitialStateChanged += OnInterstitialStateChanged;

        HandleInterstitialStateOnStart();
    }

    private void HandleInterstitialStateOnStart()
    {
        if (Bridge.advertisement.interstitialState == InterstitialState.Opened)
        {
            MuteGame();
        }
        else if (Bridge.advertisement.interstitialState == InterstitialState.Closed ||
                 Bridge.advertisement.interstitialState == InterstitialState.Failed)
        {
            UnmuteGame();
        }
    }

    public void ShowInterstitial()
    {
        Bridge.advertisement.ShowInterstitial();
    }

    private void OnInterstitialStateChanged(InterstitialState state)
    {
        Debug.Log("Interstitial durumu: " + state);

        switch (state)
        {
            case InterstitialState.Opened:
                MuteGame();
                break;
            case InterstitialState.Closed:
            case InterstitialState.Failed:
                UnmuteGame();
                break;
        }
    }

    private void MuteGame()
    {
        AudioListener.pause = true;

        Time.timeScale = 0f;

        Debug.Log("Oyun sesi kapat ld  ve oyun duraklat ld .");
    }

    private void UnmuteGame()
    {
        AudioListener.pause = false;

        Time.timeScale = 1f;
        Debug.Log("Oyun sesi a  ld  ve oyun devam ediyor.");
    }
}