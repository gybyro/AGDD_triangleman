using UnityEngine;


public enum TriangleState { Active, Locked, Done, Concom, ConDone }


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
    public event System.Action<TriangleStateControl> OnRotationChanged;

    [Header("Opener triangles")]
    public bool isOpen = false;
    // to get south,sw and se, just rotate the parent ower by 60 on z
    public WPivotMover north;
    public WPivotMover nw;
    public WPivotMover ne;

    private const float ROTATION_EPSILON = 0.5f;

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

    public void SyncRotation() { SetRotation(CurrentRotation); } // currently not being used ===

   
    public void SetState(TriangleState newState)
    {
        state = newState;
        UpdateColor();



        if ((newState == TriangleState.Done || newState == TriangleState.ConDone) && !isOpen)
        {

            SetOpen();
        }
        else if ((newState != TriangleState.Done || newState != TriangleState.ConDone) && isOpen)
        {
            SetClose();
        }
    }

    public void SetRotation(float zRotation) {
        CurrentRotation = Normalize(zRotation);
        transform.rotation = Quaternion.Euler(0, 0, CurrentRotation);

        OnRotationChanged?.Invoke(this); // tell the world 
    }
    public float GetGoalRotation() { return Normalize(goalRotation); }


    public bool CanRotate()
    {
        return state == TriangleState.Active || state == TriangleState.Done;
        
    }

    public void SetOpen()
    {
        // set openers rotation if aplycaple
        if (north) north.targetRotationx = 180f;
        if (nw) { nw.targetRotationy = 180f; nw.targetRotationz = 60f; }
        if (ne) { ne.targetRotationy = -180f; ne.targetRotationz = 300f; }

        north?.PlayFlip();
        nw?.PlayFlip();
        ne?.PlayFlip();

        isOpen = true;
    }

    public void SetClose()
    {
        if (north) north.targetRotationx = 0f;
        if (nw) { nw.targetRotationy = 0f; nw.targetRotationz = 120f; }
        if (ne) { ne.targetRotationy = 0f; ne.targetRotationz = 240f; }

        north?.PlayFlip();
        nw?.PlayFlip();
        ne?.PlayFlip();

        isOpen = false;
        
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