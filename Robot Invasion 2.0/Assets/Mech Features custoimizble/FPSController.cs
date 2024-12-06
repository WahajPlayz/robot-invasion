using UnityEngine;

public class MechController : MonoBehaviour
{
    public float moveSpeed = 5f; // Normal movement speed
    public float sprintSpeed = 10f; // Sprinting speed
    public float jumpForce = 10f; // Jump force
    public float gravity = 20f; // Gravity for the jump

    private CharacterController characterController;
    private Vector3 moveDirection;
    private float verticalVelocity; // Used for jump and gravity
    private bool isSprinting;
    private bool isGrounded;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked; // Lock cursor to the center of the screen
        Cursor.visible = false; // Hide the cursor
    }

    void Update()
    {
        isGrounded = characterController.isGrounded;

        // Movement input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 move = transform.right * horizontal + transform.forward * vertical;
        float currentSpeed = isSprinting ? sprintSpeed : moveSpeed;

        moveDirection = move * currentSpeed;

        // Jump input
        if (isGrounded && Input.GetButton("Jump"))
        {
            verticalVelocity = jumpForce;
        }
        else if (!isGrounded)
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }

        moveDirection.y = verticalVelocity;

        // Apply the movement
        characterController.Move(moveDirection * Time.deltaTime);
    }
}
