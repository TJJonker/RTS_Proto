using RTS.Input;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    public PlayerInputActionMaps PlayerInputActionMap { get; private set; }

    private List<InputActionMap> activeActionMaps;
    private List<InputActionMap> previousActiveActionMaps;

    private void Awake()
    {
        Instance = this;    
        PlayerInputActionMap = new PlayerInputActionMaps();
        activeActionMaps = new List<InputActionMap>();
        previousActiveActionMaps = new List<InputActionMap>();  
    }

    private void Start()
    {
        SwitchActionMap(PlayerInputActionMap.Gameplay);
    }

    public void SwitchActionMap(InputActionMap actionMap)
    {
        previousActiveActionMaps = new List<InputActionMap>(activeActionMaps);
        DisableAllActionMaps();        
        EnableActionMap(actionMap);
    }

    public void EnableActionMap(InputActionMap actionMap)
    {
        actionMap.Enable();
        activeActionMaps.Add(actionMap);
    }

    public void DisableActionMap(InputActionMap actionMap)
    {
        if (!activeActionMaps.Contains(actionMap)) return;
        actionMap.Disable();    
        activeActionMaps.Remove(actionMap);
    }

    public void DisableAllActionMaps()
    {
        PlayerInputActionMap.Disable();
        activeActionMaps.Clear();
    }

    public void SwitchToPreviousActionMap()
    {
        DisableAllActionMaps();
        
        foreach (InputActionMap actionMap in previousActiveActionMaps)
          EnableActionMap(actionMap);
        

    }

    public void PrintCurrentActionMaps()
    {
        if (activeActionMaps.Count == 0) Debug.Log("Empty");
        foreach (InputActionMap inputMap in activeActionMaps)
            Debug.Log(inputMap);
    }
}
