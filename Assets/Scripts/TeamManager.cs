using UnityEngine;
using System;

public class TeamManager : MonoBehaviour
{
    public Transform soldiersSpawnPoint;
    public Transform terroristsSpawnPoint;
    public GameObject playerPrefab;

    public Transform soldiersParent; // Soldiers parent objesi
    public Transform terroristsParent; // Terrorists parent objesi

    public Action<GameObject> onPlayerSpawned; // Event gibi kullan�lacak Action

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
            player.tag = "CT"; // Soldier i�in tag 'CT' olarak ayarlan�yor
            player.transform.SetParent(soldiersParent); // Soldiers parent objesine ekle
        }
        else if (selectedTeam == "Terrorists")
        {
            player.tag = "T"; // Ter�rist i�in tag 'T' olarak ayarlan�yor
            player.transform.SetParent(terroristsParent); // Terrorists parent objesine ekle
        }

        onPlayerSpawned?.Invoke(player); // GameManager'a haber ver
    }
}