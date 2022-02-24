using Jonko.Patterns;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    // Instancing Singleton
    public static InputManager Instance;

    // SerializeFields
    [SerializeField] public GameObject selectionSquare;     

    // Input States
    private IStatePattern InputStateGame = new Input_Game();
    private IStatePattern InputStateConsole = new Input_Console();

    // State Variables
    private IStatePattern currentState;

    private void Awake()
    {
        Instance = this;
        SwitchState(InputStateGame);
    }

    private void Update()
    {
        currentState.UpdateState();

        if (Input.GetKeyDown(KeyCode.Slash)) 
            SwitchState(InputStateConsole);
    }

    private void SwitchState(IStatePattern state)
    {
        if(currentState != null) currentState.ExitState();
        currentState = state;
        Debug.Log("Whew");
        currentState.EnterState();
    }
}