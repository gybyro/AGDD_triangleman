using UnityEngine;

public enum TriangleState { Idle, Active, Completed }

public class PuzzleTriangle : MonoBehaviour
{
    [Header("Triangle States")]
    public TriangleState state = TriangleState.Idle;

    public Color idleColor = Color.gray;
    public Color activeColor = Color.yellow;
    public Color completedColor = Color.green;
    
    [Header("Rotation (in degrees)")]
    public float rotationStep = 60f; // degrees per click
    public float rotationSpeed = 200f; // smooth rotation speed

    
    public float idleSpinSpeed = 0.1f; // degrees per second

    private Quaternion targetRotation;
    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
        targetRotation = transform.rotation;
        UpdateColor();

        // Randomize idle spin direction and speed slightly
        // idleSpinSpeed *= Random.Range(0.5f, 1.5f) * (Random.value > 0.5f ? 1 : -1);
        
    }

    void Update()
    {
        // Smooth rotation
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Idle spin
        if (state == TriangleState.Idle)
        {
            transform.Rotate(0, 0, idleSpinSpeed * Time.deltaTime);
        }
    }

    public void RotateLeft()
    {
        targetRotation *= Quaternion.Euler(0, 0, rotationStep);
    }

    public void RotateRight()
    {
        targetRotation *= Quaternion.Euler(0, 0, -rotationStep);
    }

    public void SetState(TriangleState newState)
    {
        state = newState;
        UpdateColor();
    }

    private void UpdateColor()
    {
        if (rend != null)
        {
            switch(state)
            {
                case TriangleState.Idle: rend.material.color = idleColor; break;
                case TriangleState.Active: rend.material.color = activeColor; break;
                case TriangleState.Completed: rend.material.color = completedColor; break;
            }
        }
    }
}