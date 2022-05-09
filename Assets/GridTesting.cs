using UnityEngine;
using UnityEngine.InputSystem;
using RTS.Input;
using Game.PathFinding;

public class GridTesting : MonoBehaviour
{
    private PlayerInputActionMaps playerInputActionMaps;

    private void Awake()
    {
        playerInputActionMaps = InputManager.Instance.PlayerInputActionMap;

        //playerInputActionMaps.Gameplay.LeftMouse.started += ChangeValue;
    }

    private void Start()
    {
        PathFinding pathFinding = new PathFinding(20, 20);
    }


}