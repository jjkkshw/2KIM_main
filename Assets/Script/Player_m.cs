using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    private float walkSpeed = 5f;
    private float sprintSpeed = 9f;
    private float crouchSpeed = 2.5f;

    [Header("Mouse Look")]
    private float mouseSensitivity = 100f;
    public Transform playerCamera;

    [Header("Crouch")]
    private float standingHeight = 2f;
    private float crouchHeight = 1f;

    [Header("Step Settings")]
    private float stepHeight = 0.4f;
    private float stepSmooth = 0.1f;

    [Header("Interaction")]
    public float interactDistance = 3f;

    private Rigidbody rb;
    private CapsuleCollider col;

    private float moveX;
    private float moveZ;

    private bool isGrounded;
    private bool isCrouching;

    private float xRotation;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();

        rb.freezeRotation = true;

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        HandleInput();
        MouseLook();
        HandleCrouch();
        HandleInteraction();
    }

    void FixedUpdate()
    {
        MovePlayer();
        StepClimb();
    }

    void HandleInput()
    {
        moveX = Input.GetAxisRaw("Horizontal");
        moveZ = Input.GetAxisRaw("Vertical");
    }

    void MovePlayer()
    {
        Vector3 move =
            transform.right * moveX +
            transform.forward * moveZ;

        float currentSpeed = walkSpeed;

        // 달리기
        if (Input.GetKey(KeyCode.LeftShift) && !isCrouching)
        {
            currentSpeed = sprintSpeed;
        }

        // 앉기
        if (isCrouching)
        {
            currentSpeed = crouchSpeed;
        }

        Vector3 velocity = move.normalized * currentSpeed;

        rb.linearVelocity = new Vector3(
            velocity.x,
            rb.linearVelocity.y,
            velocity.z
        );
    }

    void MouseLook()
    {
        float mouseX =
            Input.GetAxis("Mouse X") *
            mouseSensitivity *
            Time.deltaTime;

        float mouseY =
            Input.GetAxis("Mouse Y") *
            mouseSensitivity *
            Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.localRotation =
            Quaternion.Euler(xRotation, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isCrouching = true;

            col.height = crouchHeight;

            Vector3 camPos = playerCamera.localPosition;
            camPos.y = 0.5f;
            playerCamera.localPosition = camPos;
        }

        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            isCrouching = false;

            col.height = standingHeight;

            Vector3 camPos = playerCamera.localPosition;
            camPos.y = 0.9f;
            playerCamera.localPosition = camPos;
        }
    }

    // 상호작용
    void HandleInteraction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = new Ray(
                playerCamera.position,
                playerCamera.forward
            );

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, interactDistance))
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

    // 계단 오르기
    void StepClimb()
    {
        RaycastHit hitLower;

        Vector3 dir =
            transform.forward * moveZ +
            transform.right * moveX;

        if (dir == Vector3.zero)
            return;

        // 아래 레이
        if (Physics.Raycast(
            transform.position + Vector3.up * 0.1f,
            dir,
            out hitLower,
            0.5f
        ))
        {
            // 위 레이
            if (!Physics.Raycast(
                transform.position + Vector3.up * stepHeight,
                dir,
                0.5f
            ))
            {
                rb.position += new Vector3(
                    0f,
                    stepSmooth,
                    0f
                );
            }
        }
    }
}