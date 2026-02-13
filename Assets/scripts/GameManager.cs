using UnityEngine;
using System.Collections.Generic;

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
    
    [Header("Levels")]
    public int players_highest_lvl;
    private GameObject currentLevelInstance;
    
    [SerializeField] private Transform levelContainer;
    [SerializeField] private List<GameObject> levelPrefabs;

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

    // ========== STATES ========================================

    public void SetState(GameState newState)
    {
        CurrentState = newState;

        switch (newState)
        {
            case GameState.MainMenu:
                mainMenuRoot.SetActive(true);
                puzzleRoot.SetActive(false);
                Time.timeScale = 1f;

                if (currentLevelInstance != null) Destroy(currentLevelInstance);
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

    // ========== LEVELS ========================================

    public void LoadLevel(int index)
    {
        if (currentLevelInstance != null)
            Destroy(currentLevelInstance);

        if (index < 0 || index >= levelPrefabs.Count)
        {
            Debug.LogError("Invalid level index");
            return;
        }

        currentLevelInstance = Instantiate(levelPrefabs[index], levelContainer);
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
