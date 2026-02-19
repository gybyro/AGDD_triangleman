using UnityEngine;
using UnityEngine.UI;

public class LevelSelectButton : MonoBehaviour
{
    public int levelIndex;
    public Button button;

    void Start()
    {
        int unlocked = GameManager.instance.GetPlayersHighestLevel();
        button.interactable = levelIndex <= unlocked;
    }

    public void OnPressed()
    {
        GameManager.instance.StartLevel(levelIndex);
    }
}