using UnityEngine;
using TMPro;

public class KeypadSystem : MonoBehaviour
{
    [Header("Password Settings")]
    [SerializeField] private string correctPassword = "1227";

    [Header("UI")]
    [SerializeField] private GameObject keypadCanvas;
    [SerializeField] private TMP_Text inputText;
    [SerializeField] private TMP_Text messageText;

    [Header("Door")]
    [SerializeField] private DoorOpen door;

    [Header("Player")]
    [SerializeField] private SimpleFirstPersonPlayer playerController;

    private string currentInput = "";

    private void Start()
    {
        UpdateInputText();

        if (messageText != null)
        {
            messageText.text = "";
        }
    }

    public void PressNumber(string number)
    {
        if (currentInput.Length >= 4) return;

        currentInput += number;
        UpdateInputText();
    }

    public void ClearInput()
    {
        currentInput = "";
        UpdateInputText();

        if (messageText != null)
        {
            messageText.text = "";
        }
    }

    public void EnterPassword()
    {
        if (currentInput == correctPassword)
        {
            if (messageText != null)
            {
                messageText.text = "OPEN";
            }

            if (door != null)
            {
                door.OpenDoor();
            }

            CloseKeypad();
        }
        else
        {
            if (messageText != null)
            {
                messageText.text = "WRONG";
            }

            currentInput = "";
            UpdateInputText();
        }
    }

    public void CloseKeypad()
    {
        if (keypadCanvas != null)
        {
            keypadCanvas.SetActive(false);
        }

        if (playerController != null)
        {
            playerController.LockCursor();
        }
    }

    private void UpdateInputText()
    {
        string display = currentInput;

        while (display.Length < 4)
        {
            display += "-";
        }

        if (inputText != null)
        {
            inputText.text = display;
        }
    }
}