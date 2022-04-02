using Jonko.Patterns;
using UnityEngine;


public class Input_Console : IStatePattern
{
    public void EnterState()
    {
    }

    public void UpdateState()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            InputManager.Instance.SwitchState(InputManager.Instance.GetPreviousState());
    }

    public void ExitState()
    {
    }

    private void OnGUI()
    {
        float y = 0f;

        GUI.Box(new Rect(0, y, Screen.width, 30), "");
        GUI.backgroundColor = Color.black;
    }
}
