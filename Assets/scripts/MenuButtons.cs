using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class MenuButtons : MonoBehaviour
{
    public Animator menuAnimator;
    public Animator lvlSelectAnimator;
    public Animator transissionAnimation;
    


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

    private IEnumerator SwitchToGame(int lvl)
    {
        transissionAnimation.SetTrigger("TransTrigger");

        yield return new WaitForSecondsRealtime(2f);

        GameManager.instance.LoadLevel(lvl);
        GameManager.instance.SetState(GameState.Playing);
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
        Debug.Log("Play the animation trigger 'SlideIn' for the settings canvas");
    }

    public void QuitBTN()
    {
        Application.Quit();
        Debug.Log("Quit called! (This won't close the editor)");
    }


}
