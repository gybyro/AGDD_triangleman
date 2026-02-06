using UnityEngine;


public enum TriangleState { Active, Locked, Concom, Done }


public class TriangleStateControl : MonoBehaviour
{
    [Header("Triangle States")]
    public SpriteRenderer sprite;
    public TriangleState state = TriangleState.Active;

    public Color activeColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    public Color lockedColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    public Color concomColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    public Color doneColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    
    public float startRotation;
    public float goalRotation;
    public float CurrentRotation { get; private set; }

    private const float ROTATION_EPSILON = 0.5f;
    
    
    // private Renderer rend;

    void Start()
    {
 
        UpdateColor();
        SetState(state);
        SetRotation(startRotation);

        // on level start, get this triangles start and goal rotations
        // and apply them.
        transform.rotation = Quaternion.Euler(0, 0, startRotation);
        // currentRotation = startRotation;
        
    }

    void Update()
    {
        // when currentRotation == goalRotation;
        // SetState(TriangleState.Done);

    }

   
    public void SetState(TriangleState newState)
    {
        state = newState;
        UpdateColor();
    }

    public void SetRotation(float zRotation) {
        CurrentRotation = Normalize(zRotation);
        transform.rotation = Quaternion.Euler(0, 0, CurrentRotation);
    }
    public float GetGoalRotation() { return Normalize(goalRotation); }


    public bool CanRotate()
    {
        return state == TriangleState.Active;
    }

    private void UpdateColor()
    {
        
        switch(state)
        {
            case TriangleState.Active: sprite.color = activeColor; break;
            case TriangleState.Locked: sprite.color = lockedColor; break;
            case TriangleState.Concom: sprite.color = concomColor; break;
            case TriangleState.Done: sprite.color = doneColor; break;
        }
    }

    // changes later, im not sure what exactly the wincon is
    public bool IsAtGoal()
    {
        return Mathf.Abs(
            Mathf.DeltaAngle(CurrentRotation, GetGoalRotation())
        ) <= ROTATION_EPSILON;
    }
    private float Normalize(float angle) // normalizes ze angle
    {
        angle %= 360f;
        if (angle < 0) angle += 360f;
        return angle;
    }

}