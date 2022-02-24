using Jonko.Patterns;
using UnityEngine;

public class Input_Console : IStatePattern
{
    private GameObject background;

    public void EnterState()
    {
        background = background ? background : CreateBackground();
        Debug.Log("whew");
    }

    public void ExitState()
    {
    }

    public void UpdateState()
    {
    }

    private GameObject CreateBackground()
    {
        GameObject background = new GameObject("background");
        background.transform.parent = InputManager.Instance.transform;
        GameObject sprite = new GameObject("sprite");
        sprite.transform.parent = background.transform;
        var sr = sprite.AddComponent<SpriteRenderer>();

        return background;
    }
}
