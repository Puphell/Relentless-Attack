using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Playgama;
using Playgama.Modules.Platform;

public class GameReadyMessage : MonoBehaviour
{
    void Start()
    {
        AdManager adManager = FindObjectOfType<AdManager>();
        if (adManager != null)
        {
            adManager.ShowInterstitial();
        }
        else
        {
            Debug.Log("AdManager Bulunamad�");
        }

        Bridge.platform.SendMessage(PlatformMessage.GameReady);
    }
}
