using UnityEngine;
using UnityEngine.SceneManagement;

public class TeamSelection : MonoBehaviour
{
    public void SelectTeam(string team)
    {
        // Takýmý kaydet
        PlayerPrefs.SetString("SelectedTeam", team);
        PlayerPrefs.Save(); // Veriyi kalýcý hale getir

        Debug.Log("Selected Team: " + team);

        // Oyun sahnesine geçiþ yap
        SceneManager.LoadScene("Game");
    }
}
