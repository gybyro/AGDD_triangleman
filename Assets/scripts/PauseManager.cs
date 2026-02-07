using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class PauseManager : MonoBehaviour
{
    public CanvasGroup pauseMenuUI;
    public Animator pauseAnimator;

    private bool isPaused;


    void Start()
    {

        isPaused = false;
        // pauseAnimator.Play("PauseUp_Idle");
        // pauseAnimator.SetTrigger("Go");

        pauseMenuUI.alpha = 0;
        pauseMenuUI.blocksRaycasts = false;
        pauseMenuUI.interactable = false;

        pauseAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    public void Pause() {        
        pauseMenuUI.alpha = 1;
        pauseMenuUI.blocksRaycasts = true;
        pauseMenuUI.interactable = true;
        
        pauseAnimator.SetTrigger("Pause");

        Time.timeScale = 0f;
        isPaused = true;
    }
    
    public void Resume() {
        pauseAnimator.SetTrigger("Go");

        // hide AFTER animation finishes
        StartCoroutine(HideAfterAnim());

        Time.timeScale = 1f;
        isPaused = false;
    }
    private IEnumerator HideAfterAnim() {
        // match animation length
        yield return new WaitForSecondsRealtime(0.40f);

        
        pauseMenuUI.alpha = 0;
        pauseMenuUI.blocksRaycasts = false;
        pauseMenuUI.interactable = false;
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (!context.performed) return;   // only trigger once per button push

        if (isPaused) Resume();
        else Pause();
    }

    public void BackToMainMenu() {
        Resume();    // unpause first
        SceneManager.LoadScene("MainMenuScene");
        Debug.Log("Scene MainMenuScene loaded");
    }
}
