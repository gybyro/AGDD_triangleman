using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;



public class DropdownManager : MonoBehaviour
{
    public static bool IsPaused { get; private set; }

    [Header("Game Over")]
    public CanvasGroup gameOverMenuUI;
    public Animator gameOverAnimator;
    [Header("Level Complete")]
    public CanvasGroup youWonMenuUI;
    public Animator youWonAnimator;
    [Header("Transmission")]
    public MenuButtons menuButtons;

    void Start()
    {
        IsPaused = false;

        gameOverMenuUI.alpha = 0;
        gameOverMenuUI.blocksRaycasts = false;
        gameOverMenuUI.interactable = false;

        youWonMenuUI.alpha = 0;
        youWonMenuUI.blocksRaycasts = false;
        youWonMenuUI.interactable = false;

        gameOverAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
        youWonAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    public void Pause(CanvasGroup menuUI, Animator animator) {

        if (GameManager.instance.CurrentState == GameState.MainMenu) return;

        menuUI.alpha = 1;
        menuUI.blocksRaycasts = true;
        menuUI.interactable = true;
        
        animator.SetTrigger("Pause");

        GameManager.instance.SetState(GameState.Paused);
    }


    private IEnumerator HideAfterAnim(CanvasGroup menuUI) {
        // match animation length
        yield return new WaitForSecondsRealtime(0.40f);

        menuUI.alpha = 0;
        menuUI.blocksRaycasts = false;
        menuUI.interactable = false;
    }

    public void BackToMainMenu(CanvasGroup menuUI, Animator animator) {

        IsPaused = true;
        animator.SetTrigger("Go");
        StartCoroutine(HideAfterAnim(menuUI));

        // Switch to main menu state
        GameManager.instance.SetState(GameState.MainMenu);

        IsPaused = false;
    }


    public void GameOverBTM() {
        BackToMainMenu(gameOverMenuUI, gameOverAnimator);
    }
    public void youWonBTM() {
        BackToMainMenu(youWonMenuUI, youWonAnimator);
    }


    public void GameOverDown() {
        Pause(gameOverMenuUI, gameOverAnimator);
    }
    public void YouWonDown() {
        Pause(youWonMenuUI, youWonAnimator);
    }


    // public void Retry()
    // {
    //     IsPaused = true;
    //     gameOverAnimator.SetTrigger("Go");
    //     StartCoroutine(HideAfterAnim(gameOverMenuUI));

    //     menuButtons.RetryGameBTN();

    //     IsPaused = false;
    // }
    public void Retry() { StartCoroutine(RetryRoutine()); }
    private IEnumerator RetryRoutine()
    {
        gameOverAnimator.SetTrigger("Go");
        yield return new WaitForSecondsRealtime(0.4f);

        gameOverMenuUI.alpha = 0;
        gameOverMenuUI.blocksRaycasts = false;
        gameOverMenuUI.interactable = false;

        menuButtons.RetryGameBTN();
    }

    public void NextLevel() { StartCoroutine(NextLevelRoutine()); }
    private IEnumerator NextLevelRoutine()
    {
        youWonAnimator.SetTrigger("Go");
        yield return new WaitForSecondsRealtime(0.4f);

        youWonMenuUI.alpha = 0;
        youWonMenuUI.blocksRaycasts = false;
        youWonMenuUI.interactable = false;

        menuButtons.NextGameBTN();
    }    
}
