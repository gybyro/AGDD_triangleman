using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public enum GameState
{
    MainMenu,
    Playing,
    Paused,
    LevelEnding
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameState CurrentState { get; private set; }
    
    [Header("Levels")]
    public int players_highest_lvl;
    public int currentLevel;
    private GameObject currentLevelInstance;
    
    [SerializeField] private Transform levelContainer;
    [SerializeField] private List<GameObject> levelPrefabs;

    public GameObject mainMenuRoot;
    public GameObject puzzleRoot;

    public DropdownManager dropdownManager;


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
                levelEnding = false;
                mainMenuRoot.SetActive(false);
                puzzleRoot.SetActive(true);
                Time.timeScale = 1f;

                UnityEngine.EventSystems.EventSystem.current
                    ?.SetSelectedGameObject(null);

                break;

            case GameState.Paused:
                Time.timeScale = 0f;
                break;

            case GameState.LevelEnding:
            // stop gameplay input but DO NOT freeze time
            Time.timeScale = 1f;
            break;
        }
    }

    // ========== LEVELS ========================================

    public void LoadLevel(int index)
    {
        levelEnding = false;

        if (currentLevelInstance != null)
            Destroy(currentLevelInstance);

        if (index < 0 || index >= levelPrefabs.Count)
        {
            Debug.LogError("Invalid level index");
            return;
        }

        currentLevel = index;
        currentLevelInstance = Instantiate(levelPrefabs[index], levelContainer);
    }


    public void updateLvlCount(int completedLevel)
    {
        int unlockedLevel = completedLevel + 1;

        if (unlockedLevel > players_highest_lvl)
            players_highest_lvl = unlockedLevel;

        PlayerPrefs.SetInt("PlayerLevel", players_highest_lvl);
        PlayerPrefs.Save();

        Debug.Log("Highest level saved: " + players_highest_lvl);
    }

    public int GetPlayersHighestLevel()
    {
        return players_highest_lvl;
    }

    public void GameOver() { dropdownManager.GameOverDown(); }
    

    private bool levelEnding = false;
    public void LevelComplete()
    {
        if (levelEnding) return;
        levelEnding = true;

        StartCoroutine(LevelCompleteRoutine());
    }

    private IEnumerator LevelCompleteRoutine()
    {
        SetState(GameState.LevelEnding); // disables input instantly
        yield return new WaitForSeconds(1f); // animations still run
        dropdownManager.YouWonDown();
    }



    public IEnumerator ReloadLevel(int index, Animator transition)
    {
        transition.ResetTrigger("TransTrigger");
        transition.SetTrigger("TransTrigger");

        // wait for screen cover
        yield return new WaitForSecondsRealtime(2f);

        // destroy BEFORE loading
        if (currentLevelInstance != null)
        {
            Destroy(currentLevelInstance);
            currentLevelInstance = null;
        }

        LoadLevel(index);
        yield return null; // wait one frame (important)

        SetState(GameState.Playing);
    }
}
