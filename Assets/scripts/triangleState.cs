using UnityEngine;


public enum TriangleState { Active, Locked, Done, Concom, ConDone }

public enum TriangleCorner
{
    A = 0,
    B = 1,
    C = 2
}


public class TriangleStateControl : MonoBehaviour
{
    [Header("Triangle States")]
    [SerializeField] private SpriteRenderer mainSpriteRenderer;
    public SpriteRenderer northRenderer;
    public SpriteRenderer nwRenderer;
    public SpriteRenderer neRenderer;
    public TriangleState state = TriangleState.Active;

    [Header("Corner Colours")]
    public Color[] cornerColors = new Color[3];

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


    private void Awake()
    {
        if (mainSpriteRenderer == null)
            mainSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }


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

        UpdateOpenerColor();
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

    public void SetCornerColors(Color c1, Color c2, Color c3)
    {
        cornerColors[0] = c1;
        cornerColors[1] = c2;
        cornerColors[2] = c3;

        UpdateOpenerColor();
    }

    private void UpdateColor()
    {
        
        switch(state)
        {
            case TriangleState.Active: mainSpriteRenderer.color = activeColor; break;
            case TriangleState.Locked: mainSpriteRenderer.color = lockedColor; break;
            case TriangleState.Concom: mainSpriteRenderer.color = concomColor; break;
            case TriangleState.Done: mainSpriteRenderer.color = doneColor; break;
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

    public TriangleCorner GetUpCorner()
    {
        float rot = Normalize(CurrentRotation);

        // each 120 degrees = next corner
        int index = Mathf.RoundToInt(rot / 120f) % 3;
        if (index < 0) index += 3;

        return (TriangleCorner)index;
    }

    private void UpdateOpenerColor()
    {
        if (cornerColors == null || cornerColors.Length < 3)
            return;

        TriangleCorner up = GetUpCorner();
        Color c = cornerColors[(int)up];

        if (northRenderer) northRenderer.color = c;
        if (nwRenderer) nwRenderer.color = c;
        if (neRenderer) neRenderer.color = c;
    }

}