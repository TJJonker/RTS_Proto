using RTS.Input;
using UnityEngine;
using UnityEngine.InputSystem;
using static RTS.Input.PlayerInputActionMaps;

public class DebugConsole : MonoBehaviour, IDebugConsoleActions
{
    private bool showConsole;

    private InputManager inputManager;
    private PlayerInputActionMaps playerInputActionMaps;

    private void Start()
    {
        inputManager = InputManager.Instance;
        playerInputActionMaps = inputManager.PlayerInputActionMap;

        playerInputActionMaps.Gameplay.OpenConsole.canceled += EnableConsole;
        playerInputActionMaps.DebugConsole.SetCallbacks(this);
    }

    public void OnExit(InputAction.CallbackContext context)
    {
        if(context.canceled) DisableConsole(context);
    }

    public void OnSubmit(InputAction.CallbackContext context)
    {
        DisableConsole(context);
    }

    private void EnableConsole(InputAction.CallbackContext context)
    {
        Debug.Log("Enable");
        inputManager.SwitchActionMap(playerInputActionMaps.DebugConsole);
        inputManager.PrintCurrentActionMaps();
    }

    private void DisableConsole(InputAction.CallbackContext context)
    {
        Debug.Log("Disable");
        inputManager.SwitchToPreviousActionMap();
        inputManager.PrintCurrentActionMaps();
    }

    private void OnGUI()
    {
        if (!showConsole) return;

        float y = 0f;

        GUI.Box(new Rect(0, y, Screen.width, 30), "");
        GUI.backgroundColor = Color.black;
    }
}
