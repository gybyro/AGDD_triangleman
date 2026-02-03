using UnityEngine;
using UnityEngine.InputSystem;

public class TriangleInputManager : MonoBehaviour
{
    private TriangleControls controls;

    public Camera mainCam;
    public LayerMask triangleLayerMask;

    private void Awake()
    {
        controls = new TriangleControls();

        // Subscribe to actions
        controls.Gameplay.LeftClick.performed += ctx => Rotate(true);
        controls.Gameplay.RightClick.performed += ctx => Rotate(false);
    }

    private void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    private void OnDisable()
    {
        controls.Gameplay.Disable();
    }

    private void Rotate(bool left)
    {
        Ray ray = mainCam.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, triangleLayerMask))
        {
            PuzzleTriangle tri = hit.collider.GetComponent<PuzzleTriangle>();
            if (tri != null && tri.state == TriangleState.Active)
            {
                if (left) tri.RotateLeft();
                else tri.RotateRight();
            }
        }
    }
}