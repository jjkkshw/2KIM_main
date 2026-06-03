using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class SimpleFirstPersonPlayer : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float gravity = -9.81f;

    [Header("Camera")]
    [SerializeField] private Transform playerCamera;
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private float cameraPitchLimit = 80f;

    [Header("Interaction")]
    [SerializeField] private float interactDistance = 3f;

    private CharacterController characterController;
    private Vector3 velocity;
    private float cameraPitch;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();

        if (playerCamera == null && Camera.main != null)
        {
            playerCamera = Camera.main.transform;
        }
    }

    private void Start()
    {
        LockCursor();
    }

    private void Update()
    {
        MovePlayer();
        LookAround();
        HandleInteraction();
    }

    private void MovePlayer()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 moveDirection =
            transform.right * horizontal +
            transform.forward * vertical;

        moveDirection.Normalize();

        characterController.Move(
            moveDirection *
            moveSpeed *
            Time.deltaTime
        );

        if (characterController.isGrounded && velocity.y < 0f)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;

        characterController.Move(
            velocity * Time.deltaTime
        );
    }

    private void LookAround()
    {
        float mouseX =
            Input.GetAxis("Mouse X") *
            mouseSensitivity;

        float mouseY =
            Input.GetAxis("Mouse Y") *
            mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        cameraPitch -= mouseY;

        cameraPitch = Mathf.Clamp(
            cameraPitch,
            -cameraPitchLimit,
            cameraPitchLimit
        );

        if (playerCamera != null)
        {
            playerCamera.localRotation =
                Quaternion.Euler(
                    cameraPitch,
                    0f,
                    0f
                );
        }
    }

    // 상호작용
    private void HandleInteraction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = new Ray(
                playerCamera.position,
                playerCamera.forward
            );

            RaycastHit hit;

            // 디버그용 빨간 선
            Debug.DrawRay(
                playerCamera.position,
                playerCamera.forward * interactDistance,
                Color.red,
                1f
            );

            if (Physics.Raycast(
                ray,
                out hit,
                interactDistance
            ))
            {
                IInteractable interactable =
                    hit.collider.GetComponentInParent<IInteractable>();

                if (interactable != null)
                {
                    interactable.Interact();
                }
            }
        }
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        enabled = true;
    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        enabled = false;
    }
}