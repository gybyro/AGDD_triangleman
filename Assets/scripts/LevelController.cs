using System.Collections.Generic;
using UnityEngine;


public enum LevelState
{
    Playing,
    Won,
    Lost
}

public class LevelController : MonoBehaviour
{
    public LevelState CurrentState { get; private set; }

    [Header("Owers")]
    [SerializeField] private List<GameObject> trianglePrefabs;

    [SerializeField] private List<TriangleStateControl> triangles;
    [Header("Colours")]
    public Color color1 = Color.white;
    public Color color2 = Color.white;
    public Color color3 = Color.white;


    private void Awake()
    {
        foreach (var tri in triangles)
        {
            tri.SetCornerColors(color1, color2, color3);
            tri.OnRotationChanged += OnTriangleRotated;
        }
    }

    private void OnDestroy()
    {
        // foreach (var tri in triangles) { tri.OnRotationChanged -= OnTriangleRotated; }
    }

    private void OnTriangleRotated(TriangleStateControl changed)
    {
        EvaluateLevel();
    }

    private void EvaluateLevel()
    {
        // Put logic here


        // all triangles at goal
        foreach (var tri in triangles)
        {
            if (!tri.IsAtGoal())
            {
                tri.SetState(TriangleState.Active);
                return;
            }
        }

        // All satisfied
        foreach (var tri in triangles)
            tri.SetState(TriangleState.Done);
    }

    // ===================== ======================
    private void HasWon ()
    {
        
    }





}
