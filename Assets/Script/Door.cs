using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [Header("Door Settings")]
    public float openAngle = 90f;
    public float openSpeed = 3f;

    [Header("Lock Settings")]
    public bool isLocked = true;

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
        Quaternion targetRotation =
            isOpen ? openRotation : closedRotation;

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
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

            if (inventory.hasKey)
            {
                isLocked = false;
                Debug.Log("열쇠로 문을 열었다!");
            }
            else
            {
                Debug.Log("문이 잠겨있다.");
                return;
            }
        }

        isOpen = !isOpen;
    }

    // 잠금 해제 함수
    public void UnlockDoor()
    {
        isLocked = false;

        Debug.Log("문 잠금 해제!");
    }
}