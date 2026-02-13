using UnityEngine;

public enum GameState
{
    MainMenu,
    Playing,
    Paused
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameState CurrentState { get; private set; }

    public int players_highest_lvl;

    public GameObject mainMenuRoot;
    public GameObject puzzleRoot;


    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        players_highest_lvl = PlayerPrefs.GetInt("PlayerLevel", 0);
        SetState(GameState.MainMenu);
    }

    public void SetState(GameState newState)
    {
        CurrentState = newState;

        switch (newState)
        {
            case GameState.MainMenu:
                mainMenuRoot.SetActive(true);
                puzzleRoot.SetActive(false);
                Time.timeScale = 1f;
                break;

            case GameState.Playing:
                mainMenuRoot.SetActive(false);
                puzzleRoot.SetActive(true);
                Time.timeScale = 1f;
                break;

            case GameState.Paused:
                Time.timeScale = 0f;
                break;
        }
    }


    public void updateLvlCount()
    { 
        players_highest_lvl++;
        PlayerPrefs.SetInt("PlayerLevel", players_highest_lvl);
        PlayerPrefs.Save();
    }

    public int GetPlayersHighestLevel()
    {
        return players_highest_lvl;
    }
}
