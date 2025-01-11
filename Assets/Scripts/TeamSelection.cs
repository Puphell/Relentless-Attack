using UnityEngine;
using UnityEngine.SceneManagement;

public class TeamSelection : MonoBehaviour
{
    public void SelectTeam(string team)
    {
        // Tak�m� kaydet
        PlayerPrefs.SetString("SelectedTeam", team);
        PlayerPrefs.Save(); // Veriyi kal�c� hale getir

        Debug.Log("Selected Team: " + team);

        // Oyun sahnesine ge�i� yap
        SceneManager.LoadScene("Game");
    }
}
