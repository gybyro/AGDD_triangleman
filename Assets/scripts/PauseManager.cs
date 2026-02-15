using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;



public class PauseManager : MonoBehaviour
{
    public static bool IsPaused { get; private set; }

    public CanvasGroup pauseMenuUI;
    public Animator pauseAnimator;
    public Animator transissionAnimation;




    void Start()
    {

        IsPaused = false;

        pauseMenuUI.alpha = 0;
        pauseMenuUI.blocksRaycasts = false;
        pauseMenuUI.interactable = false;

        pauseAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    public void Pause() {

        if (GameManager.instance.CurrentState == GameState.MainMenu) return;
        if (IsPaused) return; // prevent double pause

        IsPaused = true;

        pauseMenuUI.alpha = 1;
        pauseMenuUI.blocksRaycasts = true;
        pauseMenuUI.interactable = true;
        
        pauseAnimator.SetTrigger("Pause");

        GameManager.instance.SetState(GameState.Paused);
    }
    
    public void Resume() {
        if (!IsPaused) return; // prevent double resume
        IsPaused = false;

        pauseAnimator.SetTrigger("Go");

        // hide AFTER animation finishes
        StartCoroutine(HideAfterAnim());

        GameManager.instance.SetState(GameState.Playing);
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

        if (GameManager.instance.CurrentState == GameState.Paused)
            Resume();
        else
            Pause();
    }


    public void BackToMainMenu() {

        IsPaused = true;
        pauseAnimator.SetTrigger("Go");
        StartCoroutine(HideAfterAnim());


        // Switch to main menu state
        GameManager.instance.SetState(GameState.MainMenu);

        IsPaused = false;
    }

    public void QuitBTN()
    {
        Application.Quit();
        Debug.Log("Quit called! (This won't close the editor)");
        
    }

    public void GameOverDown() {

        if (GameManager.instance.CurrentState == GameState.MainMenu) return;

        pauseMenuUI.alpha = 1;
        pauseMenuUI.blocksRaycasts = true;
        pauseMenuUI.interactable = true;
        
        pauseAnimator.SetTrigger("Pause");
    }

    
}
