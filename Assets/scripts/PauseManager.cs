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
        pauseMenuUI.alpha = 1;
        pauseMenuUI.blocksRaycasts = true;
        pauseMenuUI.interactable = true;
        
        pauseAnimator.SetTrigger("Pause");

        GameManager.instance.SetState(GameState.Paused);
    }
    
    public void Resume() {
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

        if (IsPaused) Resume();
        else Pause();
    }


    public void BackToMainMenu() {

        IsPaused = true;
        pauseAnimator.SetTrigger("Go");
        StartCoroutine(HideAfterAnim());

        // Switch to main menu state
        GameManager.instance.SetState(GameState.MainMenu);

        IsPaused = false;
    }

    // private IEnumerator TransitionToMainMenu()
    // {
    //     IsPaused = true;

    //     // Trigger transition animation
    //     // transissionAnimation.SetTrigger("TransTrigger");

    //     // Keep game paused while transition plays
    //     // yield return new WaitForSecondsRealtime(2f); // use Realtime so Time.timeScale = 0 doesn't stop it

    //     // Hide pause menu
    //     pauseMenuUI.alpha = 0;
    //     pauseMenuUI.blocksRaycasts = false;
    //     pauseMenuUI.interactable = false;

    //     // Switch to main menu state
    //     GameManager.instance.SetState(GameState.MainMenu);

    //     // Optional: reset pause flag
    //     IsPaused = false;
    // }


    public void QuitBTN()
    {
        Application.Quit();
        Debug.Log("Quit called! (This won't close the editor)");
        
    }

    
}
