using UnityEngine;
using UnityEngine.InputSystem;

public class PaddleController : MonoBehaviour
{
    public float rotationSpeed;

    private bool rotateClock;
    private bool rotateCounter;
    private bool arrowLeft;
    private bool arrowRight;

    // performed = button held, canceled = released
    public void OnRotateClock(InputAction.CallbackContext context) {
        rotateClock = context.ReadValueAsButton();
    }
    public void OnRotateCounter(InputAction.CallbackContext context) {
        rotateCounter = context.ReadValueAsButton();
    }
    public void OnRotateLeftKey(InputAction.CallbackContext context) {
        arrowLeft = context.ReadValueAsButton();
    }
    public void OnRotateRightKey(InputAction.CallbackContext context) {
        arrowRight = context.ReadValueAsButton();
    }


    void FixedUpdate(){
        if (rotateClock || arrowRight) {
            transform.Rotate(0, 0, rotationSpeed * Time.fixedDeltaTime);
        }
       
        if (rotateCounter || arrowLeft) {
            transform.Rotate(0, 0, -rotationSpeed * Time.fixedDeltaTime);
        }
        
    }
}
