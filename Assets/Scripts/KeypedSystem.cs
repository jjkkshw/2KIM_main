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
        if (currentInput.Length >= 4)
        {
            return;
        }

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

            door.OpenDoor();
            keypadCanvas.SetActive(false);
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
        keypadCanvas.SetActive(false);
    }

    private void UpdateInputText()
    {
        string display = "";

        for (int i = 0; i < currentInput.Length; i++)
        {
            display += currentInput[i];
        }

        for (int i = currentInput.Length; i < 4; i++)
        {
            display += "-";
        }

        inputText.text = display;
    }
}