using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    [SerializeField] private float openAngle = 90f;
    [SerializeField] private float openSpeed = 2f;

    private bool isOpen;
    private Quaternion openRotation;

    private void Start()
    {
        openRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0f, openAngle, 0f));
    }

    private void Update()
    {
        if (!isOpen) return;

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            openRotation,
            Time.deltaTime * openSpeed
        );
    }

    public void OpenDoor()
    {
        isOpen = true;
    }
}