using UnityEngine;
using UnityEngine.InputSystem;

public class ExitGame : MonoBehaviour
{
    public void OnExit(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Application.Quit();
        }
    }
}
