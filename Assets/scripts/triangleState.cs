using System.Collections.Generic;
using UnityEngine;

// Active = is clickable and will turn
// Locked = is not clickable and will not rotate
// Concom = not clickable but will rotate with others

public enum TriangleState { Active, Concom, Concom2, LockedClosed, LockedOpen }
public enum PosibleRotations {
    d0 = 0,
    d60 = 60,
    d120 = 120,
    d180 = 180,
    d240 = 240,
    d300 = 300,
}
public enum TriangleCorner
{
    southC = 0,
    leftC = 1,
    rightC = 2
}

public class TriangleStateControl : MonoBehaviour
{
    [Header("Triangle Sprites")]
    [SerializeField] private SpriteRenderer mainSpriteRenderer;
    [Header("Flaps")]
    public SpriteRenderer northFlap;
    public SpriteRenderer leftFlap;
    public SpriteRenderer rightFlap;
    [Header("Corners")]
    public SpriteRenderer southCorner;
    public SpriteRenderer leftCorner;
    public SpriteRenderer rightCorner;

    [Header("State stuff")]
    public TriangleState state;
    public PosibleRotations winCon;

    [Header("Corner Colours")]
    public Color[] cornerColors = new Color[3]; // given by level control
    public Color activeColor = Color.white;
    public Color concomColor = Color.white;


    // public float startRotation;
    public float CurrentRotation { get; private set; }
    public event System.Action<TriangleStateControl> OnRotationChanged;

    [Header("Opener triangles")]
    public bool isOpen = false;

    // the pos the triangle will open at, get from level controller
    public List<PosibleRotations> openingRotations;
 
    public WPivotMover northWP;
    public WPivotMover leftWP;
    public WPivotMover rightWP;

    private const float ROTATION_EPSILON = 0.5f;


    private void Awake()
    {
        if (mainSpriteRenderer == null)
            mainSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
    public void Initialize(TriangleState initialState,List<PosibleRotations> openPositions)
    {
        openingRotations = openPositions;

        // read scene rotation
        CurrentRotation = Normalize(transform.eulerAngles.z);

        SetState(initialState);
        UpdateOpenerColor();
    }


    void Start()
    {
        CornerColors();
        // UpdateColor();
        // SetState(state);     
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

        if (IsOpeningRotation()) SetOpen();
        else SetClose();
    }
    private bool IsOpeningRotation()
    {
        foreach (var rot in openingRotations)
        {
            // if (Mathf.Abs(CurrentRotation - (float)rot) < ROTATION_EPSILON)
            //     return true;
            if (Mathf.Abs(Mathf.DeltaAngle(CurrentRotation, (float)rot)) < ROTATION_EPSILON)
                return true;
        }
        return false;
    }

    public void SetRotation(float zRotation)
    {
        // if (state == TriangleState.LockedClosed ||
        //     state == TriangleState.LockedOpen)
        //     return;

        CurrentRotation = Normalize(zRotation);
        transform.rotation = Quaternion.Euler(0, 0, CurrentRotation);

        if (IsOpeningRotation()) SetOpen();
        else SetClose();

        UpdateOpenerColor();
        OnRotationChanged?.Invoke(this); // tell the world 
    }


    public bool CanRotateOnClick() { return state == TriangleState.Active; }
    public void SetOpen()
    {
        if (northWP) northWP.targetRotationx = 180f;
        if (leftWP) { leftWP.targetRotationy = 180f; leftWP.targetRotationz = 60f; }
        if (rightWP) { rightWP.targetRotationy = -180f; rightWP.targetRotationz = 300f; }

        northWP?.PlayFlip();
        leftWP?.PlayFlip();
        rightWP?.PlayFlip();

        isOpen = true;
    }

    public void SetClose()
    {
        if (northWP) northWP.targetRotationx = 0f;
        if (leftWP) { leftWP.targetRotationy = 0f; leftWP.targetRotationz = 120f; }
        if (rightWP) { rightWP.targetRotationy = 0f; rightWP.targetRotationz = 240f; }

        northWP?.PlayFlip();
        leftWP?.PlayFlip();
        rightWP?.PlayFlip();

        isOpen = false;
    }

    public void CornerColors()
    {
        southCorner.color = cornerColors[0];
        leftCorner.color = cornerColors[1];
        rightCorner.color = cornerColors[2];
    }

     public void SetCornerColors(Color c0, Color c1, Color c2)
    {
        cornerColors[0] = c0;
        cornerColors[1] = c1;
        cornerColors[2] = c2;
        CornerColors();
    }

    private void UpdateColor()
    {
        switch(state)
        {
            case TriangleState.Active: mainSpriteRenderer.color = activeColor; break;
            case TriangleState.Concom: mainSpriteRenderer.color = concomColor; break;
            case TriangleState.LockedClosed: SetClose(); Lock(); break;
            case TriangleState.LockedOpen: SetOpen(); Lock(); break;
        }
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

        if (northFlap) northFlap.color = c;
        if (leftFlap) leftFlap.color = c;
        if (rightFlap) rightFlap.color = c;
    }

    public void Lock()
    {// Locked triangles Are only 1 colour

        if (cornerColors == null || cornerColors.Length < 3)
        return;

        // Determine colour based on orientation
        TriangleCorner up = GetUpCorner();
        Color lockColor = cornerColors[(int)up];

        // Main body
        if (mainSpriteRenderer)
            mainSpriteRenderer.color = lockColor;

        SetCornerColors(lockColor, lockColor, lockColor);
        CornerColors();

        // Flaps
        if (northFlap) northFlap.color = lockColor;
        if (leftFlap)  leftFlap.color  = lockColor;
        if (rightFlap) rightFlap.color = lockColor;

        // Corners
        if (southCorner) southCorner.color = lockColor;
        if (leftCorner)  leftCorner.color  = lockColor;
        if (rightCorner) rightCorner.color = lockColor;
    }

}