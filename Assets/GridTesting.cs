using UnityEngine;
using UnityEngine.InputSystem;
using RTS.Input;
using Game.PathFinding;
using Jonko.Utils;
using System.Collections.Generic;

public class GridTesting : MonoBehaviour
{
    private PlayerInputActionMaps playerInputActionMaps;
    private PathFinding pathFinding;

    [SerializeField] private PathFindingVisual pathFindingVisual;

    private void Awake()
    {
        playerInputActionMaps = InputManager.Instance.PlayerInputActionMap;

        //playerInputActionMaps.Gameplay.LeftMouse.started += LeftMouseTrigger;
        //playerInputActionMaps.Gameplay.RightMouse.started += RightMouseTrigger;
    }

    private void Start()
    {
        pathFinding = new PathFinding(20, 20);
        pathFindingVisual.SetGrid(pathFinding.GetGrid());
    }

    private void LeftMouseTrigger(InputAction.CallbackContext context)
    {
        Vector3 mouseWorldPosition = Utils.MouseToScreen(Mouse.current.position.ReadValue());
        pathFinding.GetGrid().GetGridPosition(mouseWorldPosition, out int x, out int y);
        List<PathNode> path = pathFinding.FindPath(0, 0, x, y);
        if (path != null) DrawPath(path);
    } 
    
    private void RightMouseTrigger(InputAction.CallbackContext context)
    {
        Vector3 mouseWorldPosition = Utils.MouseToScreen(Mouse.current.position.ReadValue());
        pathFinding.GetGrid().GetGridPosition(mouseWorldPosition, out int x, out int y);
        pathFinding.GetNode(x, y).SetIsWalkable(!pathFinding.GetNode(x, y).isWalkable);
    }

    private void DrawPath(List<PathNode> path)
    {
        for( int i = 0; i < path.Count - 1; i++)
            Debug.DrawLine(
                new Vector3(path[i].x, path[i].y) * 1f + Vector3.one * .5f, 
                new Vector3(path[i+1].x, path[i+1].y) * 1f + Vector3.one * .5f, 
                Color.red, 100f);
    }

}