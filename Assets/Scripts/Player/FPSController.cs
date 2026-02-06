using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;

    [Header("Movement Settings")]
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float jumpPower = 7f;
    public float gravity = 10f;

    [Header("Look Settings")]
    public float lookSpeed = 2f;
    public float lookXLimit = 45f;

    [Header("Toggles")]
    public bool canMove = true;
    public bool canJump = true;
    public bool canSprint = true;

    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0f;

    private CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMovement();
        HandleJumpAndGravity();
        HandleRotation();

        characterController.Move(moveDirection * Time.deltaTime);
    }

    #region Movement
    void HandleMovement()
    {
        if (!canMove) return;

        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        bool isRunning = canSprint && Input.GetKey(KeyCode.LeftShift);
        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        float moveX = currentSpeed * Input.GetAxis("Vertical");
        float moveZ = currentSpeed * Input.GetAxis("Horizontal");

        float yVelocity = moveDirection.y;

        moveDirection = (forward * moveX) + (right * moveZ);
        moveDirection.y = yVelocity;
    }
    #endregion

    #region Jump & Gravity
    void HandleJumpAndGravity()
    {
        if (characterController.isGrounded)
        {
            if (canJump && Input.GetButton("Jump"))
            {
                moveDirection.y = jumpPower;
            }
        }
        else
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }
    }
    #endregion

    #region Rotation
    void HandleRotation()
    {
        if (!canMove) return;

        // Vertical look (camera)
        rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);

        // Horizontal look (player body)
        transform.rotation *= Quaternion.Euler(
            0f,
            Input.GetAxis("Mouse X") * lookSpeed,
            0f
        );
    }
    #endregion
}
