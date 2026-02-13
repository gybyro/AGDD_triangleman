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
        Debug.Log("Go to whatever lvl the player is on, curr_player_lvl in GameManager");
        StartCoroutine(SwitchTo(GameState.Playing));
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
