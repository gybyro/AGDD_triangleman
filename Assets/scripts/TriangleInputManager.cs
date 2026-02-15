using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class TriangleInputManager : MonoBehaviour
{
    private System.Action<InputAction.CallbackContext> leftClickHandler;
    private System.Action<InputAction.CallbackContext> rightClickHandler;

    [SerializeField] private InputActionReference leftClick;
    [SerializeField] private InputActionReference rightClick;
    [SerializeField] private float rotationStep = 60f;
    [SerializeField] private float rotationDuration = 0.2f;
    [SerializeField] private AnimationCurve rotationCurve;
    

    [SerializeField] private TriangleStateControl stateControl;
    [SerializeField] private PolygonCollider2D polyCollider;
    public Camera mainCam;

    private float targetRotation;
    private bool isRotating = false;


    private void Awake()
    {
        polyCollider = GetComponentInChildren<PolygonCollider2D>();
        stateControl = GetComponentInChildren<TriangleStateControl>();
        mainCam = Camera.main;

    }

    private void Start()
    {
        // Initialize targetRotation
        // targetRotation = stateControl.startRotation;
    }

    // private void OnEnable()
    // {
    //     leftClick.action.performed +=_=> TryRotate(rotationStep);
    //     rightClick.action.performed +=_=> TryRotate(-rotationStep);

    //     leftClick.action.Enable();
    //     rightClick.action.Enable();

    //     TriangleRotationEvents.OnActiveTriangleRotated += OnOtherTriangleRotated;
    // }
    private void OnEnable()
    {
        leftClickHandler = OnLeftClick;
        rightClickHandler = OnRightClick;

        leftClick.action.performed += leftClickHandler;
        rightClick.action.performed += rightClickHandler;

        leftClick.action.Enable();
        rightClick.action.Enable();

        TriangleRotationEvents.OnActiveTriangleRotated += OnOtherTriangleRotated;
    }

    private void OnDisable()
    {
        if (leftClick != null)
            leftClick.action.performed -= leftClickHandler;

        if (rightClick != null)
            rightClick.action.performed -= rightClickHandler;

        TriangleRotationEvents.OnActiveTriangleRotated -= OnOtherTriangleRotated;
    }


    // private void OnDisable()
    // {
    //     leftClick.action.Disable();
    //     rightClick.action.Disable();

    //     TriangleRotationEvents.OnActiveTriangleRotated -= OnOtherTriangleRotated;
    // }

    private void OnLeftClick(InputAction.CallbackContext ctx)
    { TryRotate(rotationStep); }

    private void OnRightClick(InputAction.CallbackContext ctx)
    { TryRotate(-rotationStep); }


    private bool MouseOverThis()
    {
        if (polyCollider == null || mainCam == null)
            return false;


        Vector2 mouseWorldPos =
            mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        return polyCollider.OverlapPoint(mouseWorldPos);
    }


    private void RotateRequest(float delta)
    {
        // Update target rotation
        targetRotation += delta;

        // acrivates the concom event   vvv
        if (stateControl.CanRotateOnClick())
            TriangleRotationEvents.OnActiveTriangleRotated?.Invoke(delta);

        // Start rotation if not already running
        if (!isRotating)
            StartCoroutine(RotateToTarget());
    }

    private void TryRotate(float delta)
    {
        if (!MouseOverThis()) return;
        if (GameManager.instance == null ||
            GameManager.instance.CurrentState != GameState.Playing)
            return;
        if (!stateControl.CanRotateOnClick()) return;

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
    }
}