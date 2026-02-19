using System.Collections.Generic;
using UnityEngine;


public enum LevelState
{
    Playing,
    Won,
    Lost
}
// public enum PosibleRotations {
//     d0 = 0,
//     d60 = 60,
//     d120 = 120,
//     d180 = 180,
//     d240 = 240,
//     d300 = 300,
// }

[System.Serializable]
public class TriangleWinCondition
{
    public TriangleStateControl triangle;
    public PosibleRotations winRotation;
    public List<PosibleRotations> openAtPos;
    public TriangleState triangleState;
}

public class LevelController : MonoBehaviour
{
    public LevelState CurrentState { get; private set; }
    public int levelNumber;

    [Header("Owers")]
    public List<TriangleWinCondition> winConditions;

    [Header("Colours")]
    public Color color1 = Color.white;
    public Color color2 = Color.white;
    public Color color3 = Color.white;

    [SerializeField] private TimerDisplay timer;


    private void Awake()
    {
        timer.OnTimerExpired += OnTimerExpired;

        foreach (var tri in winConditions)
        {
            tri.triangle.SetCornerColors(color1, color2, color3);
            tri.triangle.OnRotationChanged += OnTriangleRotated;

            tri.triangle.winCon = tri.winRotation;
            tri.triangle.openingRotations = tri.openAtPos;

            // IMPORTANT: evaluate AFTER data exists
            tri.triangle.SetState(tri.triangleState);

            tri.triangle.Initialize(
                tri.triangleState,
                tri.openAtPos
            );
        }
        CurrentState = LevelState.Playing;
    }

    private void OnDestroy()
    {
        if (timer != null) timer.OnTimerExpired -= OnTimerExpired;
    }

    private void OnTriangleRotated(TriangleStateControl changed)
    {
        if (CurrentState != LevelState.Playing) return;
        if (CheckWin()) HasWon();
    }



    public bool CheckWin()
    {
        foreach (var condition in winConditions)
        {
            float target = (float)condition.winRotation;

            if (Mathf.Abs(
                Mathf.DeltaAngle(
                    condition.triangle.CurrentRotation,
                    target)) > 0.5f)
            {
                return false;
            }
        }

        return true;
    }

    // ===================== ======================
    private void HasWon ()
    {
        CurrentState = LevelState.Won;
        timer.enabled = false;

        Debug.Log(":]");

        GameManager.instance.UpdateLvlCount(levelNumber);

        GameManager.instance.LevelComplete();
        
    }

    private void OnTimerExpired()
    {
        if (CurrentState != LevelState.Playing)
            return;

        HasLost();
    }
    private void HasLost()
    {
        CurrentState = LevelState.Lost;

        Debug.Log("Game Over");
        GameManager.instance.GameOver();
    }

}
