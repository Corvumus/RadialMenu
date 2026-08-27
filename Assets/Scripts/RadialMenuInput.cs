using UnityEngine;
using UnityEngine.InputSystem;

public class RadialMenuInput : MonoBehaviour
{
    [SerializeField] private GameObject radialMenu;

    [SerializeField] private InputActionReference positionIA;

    private bool IsUsingGamepad;
    private Vector2 screenCenterPos;

    private void Awake()
    {
        screenCenterPos = new(Screen.width / 2, Screen.height / 2);
    }

    public void OpenMenu(InputAction.CallbackContext context)
    {
        if (context.performed)
            radialMenu.SetActive(true);
        if (context.canceled)
            radialMenu.SetActive(false);
    }

    public void OnDeviceChanged(PlayerInput playerInput)
    {
        IsUsingGamepad = playerInput.currentControlScheme == "Gamepad";
    }

    public Vector2 GetNormalizedPosition()
    {
        Vector2 position = positionIA.action.ReadValue<Vector2>();

        if (IsUsingGamepad)
            return position;
        else
            return (position - screenCenterPos).normalized;
    }
}
