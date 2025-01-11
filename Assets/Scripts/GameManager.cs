using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Localization.Components;

public class GameManager : MonoBehaviour
{
    public int totalRounds = 10;
    public int currentRound = 0;

    public GameObject gameOverPanel;
    public TextMeshProUGUI roundWinnerText;
    public Text soldiersScoreText;
    public Text terroristsScoreText;

    public Transform soldiersParent;
    public Transform terroristsParent;

    public int soldiersScore = 0;
    public int terroristsScore = 0;

    public GameObject freeCamera;

    private PlayerHealth playerHealth;
    private bool isRoundOver = false;

    private TeamManager teamManager;

    public LocalizeStringEvent roundWinnerLocalizeStringEvent;

    void Start()
    {
        teamManager = FindObjectOfType<TeamManager>();
        teamManager.onPlayerSpawned += OnPlayerSpawned; // Dinamik olarak baðlanma

        // Skorlarý ve round bilgisini yükle veya baþlangýç deðerlerini ayarla
        soldiersScore = PlayerPrefs.GetInt("SoldiersScore", 0);
        terroristsScore = PlayerPrefs.GetInt("TerroristsScore", 0);
        currentRound = PlayerPrefs.GetInt("CurrentRound", 0);

        // Skorlarý UI'a güncelle
        UpdateScores();

        StartRound();
    }

    void StartRound()
    {
        if (currentRound >= totalRounds)
        {
            EndGame();
            return;
        }

        currentRound++;
        SaveCurrentRound();
        isRoundOver = false;
        Debug.Log("Round " + currentRound + " started.");

        if (currentRound % 3 == 0)
        {
            AdManager adManager = FindObjectOfType<AdManager>();
            if (adManager != null)
            {
                adManager.ShowInterstitial();
            }
            else
            {
                Debug.Log("AdManager Bulunamadý");
            }
        }

        if (freeCamera != null)
        {
            freeCamera.SetActive(false);
        }
    }

    void Update()
    {
        if (!isRoundOver)
        {
            CheckRoundEnd();
        }
        CheckPlayerDead();
    }

    void CheckRoundEnd()
    {
        if (IsTeamDefeated(soldiersParent))
        {
            terroristsScore++;
            SaveScores();
            UpdateScores();
            roundWinnerLocalizeStringEvent.StringReference.SetReference("LocalizationString", "key_WinTextT");
            isRoundOver = true;
            Invoke(nameof(HandleRoundEnd), 2f);
        }
        else if (IsTeamDefeated(terroristsParent))
        {
            soldiersScore++;
            SaveScores();
            UpdateScores();
            roundWinnerLocalizeStringEvent.StringReference.SetReference("LocalizationString", "key_WinTextCT");
            isRoundOver = true;
            Invoke(nameof(HandleRoundEnd), 2f);
        }
    }

    bool IsTeamDefeated(Transform teamParent)
    {
        foreach (Transform member in teamParent)
        {
            if (member.gameObject.activeSelf)
            {
                return false;
            }
        }
        return true;
    }

    void UpdateScores()
    {
        soldiersScoreText.text = " " + soldiersScore;
        terroristsScoreText.text = " " + terroristsScore;
    }

    void SaveScores()
    {
        PlayerPrefs.SetInt("SoldiersScore", soldiersScore);
        PlayerPrefs.SetInt("TerroristsScore", terroristsScore);
        PlayerPrefs.Save();
    }

    void SaveCurrentRound()
    {
        PlayerPrefs.SetInt("CurrentRound", currentRound);
        PlayerPrefs.Save();
    }

    void CheckPlayerDead()
    {
        if (playerHealth != null && playerHealth.die)
        {
            freeCamera.SetActive(true);
        }
    }

    public void SetPlayerHealth(PlayerHealth health)
    {
        playerHealth = health;
    }

    void HandleRoundEnd()
    {
        ReloadScene();
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Invoke(nameof(SpawnPlayerAfterReload), 0.5f);
    }

    void SpawnPlayerAfterReload()
    {
        if (teamManager != null)
        {
            teamManager.SpawnPlayer();
        }
        else
        {
            Debug.LogError("TeamManager bulunamadý!");
        }
    }

    void OnPlayerSpawned(GameObject player)
    {
        if (player.CompareTag("CT") || player.CompareTag("T"))
        {
            playerHealth = player.GetComponent<PlayerHealth>();
            SetPlayerHealth(playerHealth);
            Debug.Log("PlayerHealth script assigned to GameManager.");
        }
        else
        {
            Debug.LogWarning("Spawned object is not tagged as 'CT' or 'T'.");
        }
    }

    void EndGame()
    {
        Debug.Log("Game Over");
        gameOverPanel.SetActive(true);

        PlayerPrefs.DeleteKey("SoldiersScore");
        PlayerPrefs.DeleteKey("TerroristsScore");
        PlayerPrefs.DeleteKey("CurrentRound"); // Round bilgisini sýfýrla

        Invoke("GameOver", 2f);
    }

    private void GameOver()
    {
        SceneManager.LoadScene("Menu");
    }

    void OnApplicationQuit()
    {
        PlayerPrefs.DeleteKey("SoldiersScore");
        PlayerPrefs.DeleteKey("TerroristsScore");
        PlayerPrefs.DeleteKey("CurrentRound"); // Round bilgisini sýfýrla
        PlayerPrefs.Save();
    }
}
