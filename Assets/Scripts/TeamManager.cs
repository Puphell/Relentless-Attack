using UnityEngine;
using System;

public class TeamManager : MonoBehaviour
{
    public Transform soldiersSpawnPoint;
    public Transform terroristsSpawnPoint;
    public GameObject playerPrefab;

    public Transform soldiersParent; // Soldiers parent objesi
    public Transform terroristsParent; // Terrorists parent objesi

    public Action<GameObject> onPlayerSpawned; // Event gibi kullanýlacak Action

    void Start()
    {
        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        string selectedTeam = PlayerPrefs.GetString("SelectedTeam", "Soldiers");
        Transform spawnPoint = selectedTeam == "Soldiers" ? soldiersSpawnPoint : terroristsSpawnPoint;

        GameObject player = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);

        if (selectedTeam == "Soldiers")
        {
            player.tag = "CT"; // Soldier için tag 'CT' olarak ayarlanýyor
            player.transform.SetParent(soldiersParent); // Soldiers parent objesine ekle
        }
        else if (selectedTeam == "Terrorists")
        {
            player.tag = "T"; // Terörist için tag 'T' olarak ayarlanýyor
            player.transform.SetParent(terroristsParent); // Terrorists parent objesine ekle
        }

        onPlayerSpawned?.Invoke(player); // GameManager'a haber ver
    }
}