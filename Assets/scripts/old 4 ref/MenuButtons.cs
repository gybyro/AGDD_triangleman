using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuButtons : MonoBehaviour
{
    public string toLoad;
    public TMP_Text highScoreText;
    
    public void LoadScene()
    {
        SceneManager.LoadScene(toLoad);
        Debug.Log($"Scene {toLoad} loaded");
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quit called! (This won't close the editor)");
    }

    public void Reset()
    {
        PlayerPrefs.DeleteKey("HighScore");
        PlayerPrefs.DeleteAll();
        Debug.Log("All locally stored data has been deleted");

        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreText.text = "" + highScore;
    }
}
