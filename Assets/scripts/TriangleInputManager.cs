using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class TriangleInputManager : MonoBehaviour
{
    [SerializeField] private InputActionReference leftClick;
    [SerializeField] private InputActionReference rightClick;
    [SerializeField] private float rotationStep = 60f;
    [SerializeField] private float rotationDuration = 0.2f;
    [SerializeField] private AnimationCurve rotationCurve;
    

    private TriangleStateControl stateControl;
    private PolygonCollider2D polyCollider;
    public Camera mainCam;

    private float targetRotation;
    private bool isRotating = false;


    private void Awake()
    {
        polyCollider = GetComponent<PolygonCollider2D>();
        stateControl = GetComponent<TriangleStateControl>();
        mainCam = Camera.main;

    }

    private void Start()
    {
        // Initialize targetRotation
        targetRotation = stateControl.startRotation;
    }

    private void OnEnable()
    {
        // leftClick.action.performed += OnLeftClick;
        // rightClick.action.performed += OnRightClick;

        leftClick.action.performed +=_=> TryRotate(rotationStep);
        rightClick.action.performed +=_=> TryRotate(-rotationStep);

        leftClick.action.Enable();
        rightClick.action.Enable();

        TriangleRotationEvents.OnActiveTriangleRotated += OnOtherTriangleRotated;
    }

    private void OnDisable()
    {
        // leftClick.action.performed -= OnLeftClick;
        // rightClick.action.performed -= OnRightClick;

        leftClick.action.Disable();
        rightClick.action.Disable();

        TriangleRotationEvents.OnActiveTriangleRotated -= OnOtherTriangleRotated;
    }


    private bool MouseOverThis()
    {
        Vector2 mouseWorldPos =
            mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        return polyCollider.OverlapPoint(mouseWorldPos);
    }


    private void RotateRequest(float delta)
    {
        // Update target rotation
        targetRotation += delta;

        // acrivates the concom event   vvv
        if (stateControl.CanRotate())
            TriangleRotationEvents.OnActiveTriangleRotated?.Invoke(delta);

        // Start rotation if not already running
        if (!isRotating)
            StartCoroutine(RotateToTarget());
    }

    private void TryRotate(float delta)
    {
        if (!MouseOverThis()) return;
        if (!stateControl.CanRotate()) return;

        RotateRequest(delta);
    }


    // if this triangle is Concom:
    private void OnOtherTriangleRotated(float delta)
    {
        // is the current state concom?
        if (stateControl.state != TriangleState.Concom)
            return;

        RotateRequest(delta);
    }



    private IEnumerator RotateToTarget()
    {
        isRotating = true;

        while (Mathf.Abs(Mathf.DeltaAngle(stateControl.CurrentRotation, targetRotation)) > 0.01f)
        {
            float from = stateControl.CurrentRotation;
            float to = targetRotation;


            // Smooth rotation function   vvv
            float t = 0f;
            float duration = rotationDuration;

            while (t < 1f)
            {
                t += Time.deltaTime / duration;
                float eased = rotationCurve.Evaluate(t);
                float z = Mathf.LerpAngle(from, to, eased);
                transform.rotation = Quaternion.Euler(0, 0, z);

                yield return null;
            }

            // Ensure exact rotation and update authoritative state
            // tells state control of the rotation

            // transform.rotation = Quaternion.Euler(0, 0, to);
            stateControl.SetRotation(to);
  
            yield return null;
        }

        isRotating = false;

        // Check goal
        if (stateControl.IsAtGoal()) {
            stateControl.SetState(TriangleState.Done);
        }
    }
}