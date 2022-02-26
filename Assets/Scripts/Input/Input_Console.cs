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


    public void ExitState()
    {
        background.SetActive(false);
        textField.gameObject.SetActive(false);
        blinkTimer.SetActive(false);
    }

    private void BlinkInputField()
    {
        if (!blinkOn) BlinkOn();
        else BlinkOff();
    }
    private void BlinkOn()
    {
        inputString += "_";
        UpdateField(inputString);
        blinkOn = true;
    }
    private void BlinkOff()
    {
        inputString = inputString.Substring(0, inputString.Length - 1);
        UpdateField(inputString);
        blinkOn = false;
    }

    public void UpdateState()
    {
        GetInput();

        // Back out of the Console menu
        if (Input.GetKeyDown(KeyCode.Escape)) ExitConsole();

        blinkTimer.UpdateTimer();
    }

    private void GetInput()
    {
        var hasInput = false;
        foreach(char character in Input.inputString)
        {
            hasInput = true;
            if (blinkOn) BlinkOff();
            UseInput(character);
        }
        if (hasInput) UpdateField(inputString);
    }

    private void UseInput(char character)
    {
        // Backspace is getting pressed
        if(character == '\b'){
            if (inputString.Length > 0){
                inputString = inputString.Substring(0, inputString.Length - 1);
            }
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

    private void UpdateField(string str) => textField.SetText(textPreset + str);

    private void RunCommand(string command, bool resetString = false)
    {


        if (resetString) inputString = "";
        ExitConsole();
    }

    private void ExitConsole() => 
        InputManager.Instance.SwitchState(InputManager.Instance.GetPreviousState());
}
