using RTS.Input;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static RTS.Input.PlayerInputActionMaps;

public class DebugConsole : MonoBehaviour, IDebugConsoleActions
{
    private bool showConsole;
    private string input;

    private InputManager inputManager;
    private PlayerInputActionMaps playerInputActionMaps;



    public static DebugCommand KILL_ALL;
    public List<object> commandList;

    private void Awake()
    {
        KILL_ALL = new DebugCommand("kill_all", "Removes all hoomans from the scene.", "kill_all", () =>
        {
            Debug.Log("Hier nen euro");
        });

        commandList = new List<object>
        {
            KILL_ALL
        };
    }


    private void Start()
    {
        inputManager = InputManager.Instance;
        playerInputActionMaps = inputManager.PlayerInputActionMap;

        playerInputActionMaps.Gameplay.OpenConsole.canceled += EnableConsole;
        playerInputActionMaps.DebugConsole.SetCallbacks(this);
    }

    #region Input Actions
    public void OnExit(InputAction.CallbackContext context)
    {
        if(context.canceled) DisableConsole(context);
    }

    public void OnSubmit(InputAction.CallbackContext context)
    {
        HandleInput();
        input = "";
        DisableConsole(context);
    }
    #endregion

    private void EnableConsole(InputAction.CallbackContext context)
    {
        inputManager.SwitchActionMap(playerInputActionMaps.DebugConsole);
        showConsole = true;       
    }

    private void DisableConsole(InputAction.CallbackContext context)
    {
        inputManager.SwitchToPreviousActionMap();
        showConsole = false;
    }

    private void OnGUI()
    {
        if (!showConsole) return;

        float y = Screen.height;
        float lineHeight = 50f;
        float lineMargin = 5f;

        GUI.Box(new Rect(0, y - lineHeight, Screen.width, lineHeight), "");
        GUI.backgroundColor = new Color(0, 0, 0, 0);
        GUI.skin.textField.fontSize = 25;

        GUI.SetNextControlName("Console");
        input = GUI.TextField(new Rect(10f, y - (lineHeight - lineMargin), Screen.width - (lineHeight - (lineMargin * 2)), lineHeight - (lineMargin * 2)), input);
        GUI.FocusControl("Console");
    }

    private void HandleInput()
    {

    }
}
