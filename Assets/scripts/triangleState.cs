using UnityEngine;


public enum TriangleState { Idle, Active, Completed }

public class PuzzleTriangle : MonoBehaviour
{
    [Header("Triangle States")]
    public SpriteRenderer sprite;
    // public Transform overTransform;
    public TriangleState state = TriangleState.Idle;

    public Color idleColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    public Color activeColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    public Color completedColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    

    [Header("Rotation (in degrees)")]
    public float rotationStep = 60f; // degrees per click
    // public float rotationSpeed = 200f; // smooth rotation speed

    
    // public float idleSpinSpeed = 0.1f; // degrees per second

    private Quaternion targetRotation;
    // private Renderer rend;


    public Rigidbody2D body;
    public Vector2 direction;
    public float impulse;
    public Vector2 levelBounds;
    public int spinAmount;
    Vector2 startPosition;

    void Start()
    {
        // rend = GetComponent<Renderer>();
        targetRotation = transform.rotation;
        UpdateColor();
        SetState(state);

        startPosition = transform.position;
        Reset();
        body.linearVelocity = direction.normalized * impulse;
        body.angularVelocity = spinAmount;

        // Randomize idle spin direction and speed slightly
        // idleSpinSpeed *= Random.Range(0.5f, 1.5f) * (Random.value > 0.5f ? 1 : -1);
        
    }

    void Update()
    {
        // Smooth rotation
        // transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Idle spin
        // if (state == TriangleState.Idle)
        // {
        //     transform.Rotate(0, 0, idleSpinSpeed * Time.deltaTime);
        // }



        float ballAngle = Vector2.Angle(transform.position, body.linearVelocity);
        if (ballAngle < 90 &&
        (transform.position.x < -levelBounds.x ||
        transform.position.x > levelBounds.x ||
        transform.position.y < -levelBounds.y ||
        transform.position.y > levelBounds.y 
        ))
        {
            Reset();
        }
    }

    public void RotateLeft()
    {
        if (state == TriangleState.Idle) SetState(TriangleState.Active);

        targetRotation *= Quaternion.Euler(0, 0, rotationStep);
    }

    public void RotateRight()
    {
        if (state == TriangleState.Idle) SetState(TriangleState.Active);

        targetRotation *= Quaternion.Euler(0, 0, -rotationStep);
    }

    public void SetState(TriangleState newState)
    {
        state = newState;
        UpdateColor();
    }

    private void UpdateColor()
    {
        
        switch(state)
        {
            case TriangleState.Idle: sprite.color = idleColor; break;
            case TriangleState.Active: sprite.color = activeColor; break;
            case TriangleState.Completed: sprite.color = completedColor; break;
        }
    }

    


    public void Reset()
    {
        transform.position = startPosition;
        body.linearVelocity = direction.normalized * impulse;

        // reset score
        GameManager.instance.score = 0;
    }
}