using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class TriangleInputManager : MonoBehaviour
{
    [SerializeField] private InputActionReference leftClick;
    [SerializeField] private InputActionReference rightClick;
    [SerializeField] private float rotationStep = 60f;
    [SerializeField] private float rotationDuration = 0.25f;
    [SerializeField] private AnimationCurve rotationCurve;
    

    private TriangleStateControl stateControl;
    private PolygonCollider2D polyCollider;
    public Camera mainCam;
    private Coroutine rotateRoutine;



    private void Awake()
    {
        polyCollider = GetComponent<PolygonCollider2D>();
        stateControl = GetComponent<TriangleStateControl>();
        mainCam = Camera.main;

        // currentRotation = transform.eulerAngles.z;

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

    private void TryRotate(float delta)
    {
        if (!MouseOverThis()) return;
        if (!stateControl.CanRotate()) return;
        
        // float targetRotation = currentRotation + delta;
        float from = stateControl.CurrentRotation;
        float to = from + delta;

        if (rotateRoutine != null)
            StopCoroutine(rotateRoutine);

        rotateRoutine = StartCoroutine(RotateSmooth(from, to));

        // rotateRoutine = StartCoroutine(
        //     RotateSmooth(currentRotation, targetRotation)
        // );

        // currentRotation = targetRotation;

        // tells state control of the rotation
        stateControl.SetRotation(to);
        if (stateControl.IsAtGoal()) {
            stateControl.SetState(TriangleState.Done);
        }

        // acrivates the concom event
        TriangleRotationEvents.OnActiveTriangleRotated?.Invoke(delta);
    }

    private IEnumerator RotateSmooth(float from, float to)
    {
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / rotationDuration;
            float eased = rotationCurve.Evaluate(t);
            float z = Mathf.LerpAngle(from, to, eased);
            transform.rotation = Quaternion.Euler(0, 0, z);
            yield return null;
        }

        // transform.rotation = Quaternion.Euler(0, 0, to);
        stateControl.SetRotation(to);
    }


    private bool MouseOverThis()
    {
        Vector2 mouseWorldPos =
            mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        return polyCollider.OverlapPoint(mouseWorldPos);
    }


    // if this triangle is Concom state:
    private void OnOtherTriangleRotated(float delta)
    {
        if (stateControl.state != TriangleState.Concom)
            return;

        // float targetRotation = currentRotation + delta;
        float from = stateControl.CurrentRotation;
        float to = from + delta;

        if (rotateRoutine != null)
            StopCoroutine(rotateRoutine);

        // rotateRoutine = StartCoroutine(
        //     RotateSmooth(currentRotation, targetRotation)
        // );

        // currentRotation = targetRotation;

        rotateRoutine = StartCoroutine(RotateSmooth(from, to));
    }
}