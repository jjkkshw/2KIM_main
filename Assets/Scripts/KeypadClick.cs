using UnityEngine;

public class KeypadClick : MonoBehaviour
{
    [SerializeField] private GameObject keypadCanvas;

    private void OnMouseDown()
    {
        keypadCanvas.SetActive(true);
    }
}