using UnityEngine;

public class KeypadClick : MonoBehaviour
{
    [SerializeField] private GameObject keypadCanvas;
    [SerializeField] private SimpleFirstPersonPlayer playerController;

    private void Start()
    {
        if (keypadCanvas != null)
        {
            keypadCanvas.SetActive(false);
        }
    }

    private void OnMouseDown()
    {
        OpenKeypad();
    }

    private void OpenKeypad()
    {
        if (keypadCanvas != null)
        {
            keypadCanvas.SetActive(true);
        }

        if (playerController != null)
        {
            playerController.UnlockCursor();
        }
    }
}