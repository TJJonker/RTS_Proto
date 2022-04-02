using TMPro;
using UnityEngine;
using Jonko.Patterns;
using Jonko.Visualisation;
using Jonko.Utils;

public class Input_Console : IStatePattern
{
    private GameObject background;
    private TextMeshProUGUI textField;

    private string inputString = "";
    private string textPreset = "> ";

    private Vector2 textOffset = new Vector2(15, 35);

    private TimeBasedAction blinkTimer;
    private bool blinkOn;

    public void EnterState()
    {
        // Creating or enabling Console background
        background = background ? background : Visualisation.CreateBackgroundGUI(
            InputManager.Instance.transform, Vector3.zero, new Vector3(500, 80), new Color(0, 0, 0, .6f));
        background.SetActive(true);

        // Creating or enabling inputField
        textField = textField ? textField : Visualisation.CreateTextFieldGUI(
            background.transform.parent, textOffset, "> ");
        textField.gameObject.SetActive(true);

        // Enabling blinking
        if (blinkTimer == null) blinkTimer = new TimeBasedAction(BlinkInputField, .5f, true);
        else blinkTimer.SetActive(true);
    }

    public void UpdateState()
    {
        GetInput();

        // Back out of the Console menu
        if (Input.GetKeyDown(KeyCode.Escape)) ExitConsole();

        blinkTimer.UpdateTimer();
    }

    public void ExitState()
    {
        background.SetActive(false);
        textField.gameObject.SetActive(false);
        blinkTimer.SetActive(false);
    }


    #region Blinking
    /// <summary>
    ///     Function that can be called to toggle the blink
    /// </summary>
    private void BlinkInputField()
    {
        if (!blinkOn) BlinkOn();
        else BlinkOff();
    }

    /// <summary>
    ///     Turns on the blink effect
    /// </summary>
    private void BlinkOn()
    {
        inputString += "_";
        UpdateField(inputString);
        blinkOn = true;
    }

    /// <summary>
    ///     Turns of the blink effect
    /// </summary>
    private void BlinkOff()
    {
        inputString = inputString.Substring(0, inputString.Length - 1);
        UpdateField(inputString);
        blinkOn = false;
    }
    #endregion

    /// <summary>
    ///     Function that checks if there was keyboard input and processes the input.
    /// </summary>
    private void GetInput()
    {
        // Checks if a key has been pressed
        foreach(char character in Input.inputString)
        {
            if (blinkOn) BlinkOff();
            ProcessInput(character);
            UpdateField(inputString);
        }
    }

    /// <summary>
    ///     Function that checks if special or normal keys are pressed and acts accordingly
    /// </summary>
    /// <param name="character"> Character that is pressed </param>
    private void ProcessInput(char character)
    {
        // Backspace is getting pressed
        if(character == '\b'){
            if (inputString.Length > 0)
                inputString = inputString.Substring(0, inputString.Length - 1);
            return;
        }
        // Enter or Return is getting pressed
        if (character == '\n' || character == '\r')
        {
            RunCommand(inputString, true);
            return;
        }
        // Any other character is pressed
        inputString += character;
    }

    /// <summary>
    ///     Updates the text object on the screen
    /// </summary>
    /// <param name="str"> String that should be displayed </param>
    private void UpdateField(string str) 
        => textField.SetText(textPreset + str);

    /// <summary>
    ///     Runs the command that's currently in the field
    /// </summary>
    /// <param name="command"> the command string </param>
    /// <param name="resetString"> Whether the command field should be reset </param>
    private void RunCommand(string command, bool resetString = false)
    {
        if (resetString) inputString = "";
        ExitConsole();
    }

    private void ExitConsole() => 
        InputManager.Instance.SwitchState(InputManager.Instance.GetPreviousState());
}
