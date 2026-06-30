using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [Header("Door Settings")]
    public float openAngle = 90f; // -90 ?—¬?Š” ë°©í–¥ ë°˜ì „
    private float openSpeed = 3f;

    [Header("Lock Settings")]
    public bool isLocked = true;

    public KeyType requiredKey = KeyType.None;

    private bool isOpen = false;

    private Quaternion closedRotation;
    private Quaternion openRotation;

    void Start()
    {
        closedRotation = transform.rotation;

        openRotation =
            Quaternion.Euler(
                transform.eulerAngles +
                new Vector3(0f, openAngle, 0f)
            );
    }

    void Update()
    {
        Quaternion target =
            isOpen ? openRotation : closedRotation;

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            target,
            Time.deltaTime * openSpeed
        );
    }

    public void Interact()
    {
        if (isLocked)
        {
            GameObject player =
                GameObject.FindGameObjectWithTag("Player");

            PlayerInventory inventory =
                player.GetComponent<PlayerInventory>();

            if (inventory.HasKey(requiredKey))
            {
                isLocked = false;
            }
            else
            {
                return;
            }
        }

        isOpen = !isOpen;
    }
}