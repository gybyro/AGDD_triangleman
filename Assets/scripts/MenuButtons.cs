using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class MenuButtons : MonoBehaviour
{
    public Animator menuAnimator;
    public Animator lvlSelectAnimator;
    public Animator SettingsAnimator;
    public Animator transissionAnimation;

    private void Start()
    {
        transissionAnimation.updateMode = AnimatorUpdateMode.UnscaledTime;
    }



    IEnumerator SwitchTo(GameState newState)
    {
        transissionAnimation.SetTrigger("TransTrigger");
        // wait for 2 seconds for the transission animation to cover the screen
        yield return new WaitForSeconds(2f);
        GameManager.instance.SetState(newState);

    }


    // ======= Main Menu =======

    public void StartGameBTN()
    {
        int lvl = GameManager.instance.GetPlayersHighestLevel();
        StartCoroutine(SwitchToGame(lvl));
    }

    public void NextGameBTN()
    {
        int currentLvl = GameManager.instance.currentLevel;
        StartCoroutine(SwitchToGame(currentLvl + 1));
    }

    public void RetryGameBTN()
    {
        StartCoroutine(SwitchToGame(GameManager.instance.currentLevel));
    }


    private IEnumerator SwitchToGame(int lvl)
    {
        yield return GameManager.instance
            .ReloadLevel(lvl, transissionAnimation);
    }


    public void LevelSelectBTN()
    {
        menuAnimator.SetTrigger("SlideOut");
        lvlSelectAnimator.SetTrigger("SlideIn");
    }
    public void BackFromLevelSelectBTN()
    {
        menuAnimator.SetTrigger("SlideIn");
        lvlSelectAnimator.SetTrigger("SlideOut");
    }


    public void SettingsBTN()
    {
        menuAnimator.SetTrigger("SlideOut");
        SettingsAnimator.SetTrigger("SlideIn");
    }
    public void BackFromSettingsBTN()
    {
        menuAnimator.SetTrigger("SlideIn");
        SettingsAnimator.SetTrigger("SlideOut");
    }
    

    public void QuitBTN()
    {
        Application.Quit();
        Debug.Log("Quit called! (This won't close the editor)");
    }


}
