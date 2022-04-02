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
    private IStatePattern previousState;

    public IStatePattern GetPreviousState() => previousState;

    private void Awake()
    {
        Instance = this;
        SwitchState(InputStateGame);
    }

    private void Update()

    {
        currentState.UpdateState();

        //if (Input.GetKeyDown(KeyCode.Slash) && currentState != InputStateConsole) 
          //  SwitchState(InputStateConsole);
    }

    public void SwitchState(IStatePattern state)
    {
        if(currentState != null) currentState.ExitState();

        previousState = currentState;
        currentState = state;

        currentState.EnterState();
    }
}