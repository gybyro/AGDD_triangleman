using UnityEngine;
using UnityEngine.InputSystem;

public class OwerController : MonoBehaviour
{
    private bool leftClick;
    private bool rightClick;


    // performed = button clicked 
    public void OnRightClick(InputAction.CallbackContext context) {
        rightClick = context.ReadValueAsButton();
    }
    public void OnLeftClick(InputAction.CallbackContext context) {
        leftClick = context.ReadValueAsButton();
    }


    void FixedUpdate(){
        if (leftClick) {
            transform.Rotate(0, 0, 60);
        }
       
        if (rightClick) {
            transform.Rotate(0, 0, -60);
        }
        
    }
}
