using UnityEngine;

public class CameraControllerWithDrag : MonoBehaviour
{
    public Transform cameraPivot; // Pivot point for up/down rotation
    public Transform cameraTransform; // Main camera object to follow the pivot
    public float verticalSpeed = 2f; // Speed of up/down camera rotation
    public float horizontalSpeed = 2f; // Speed of mech rotation
    public float verticalLookLimit = 45f; // Limit for camera's up/down rotation
    public float followSpeed = 5f; // How quickly the camera catches up to the pivot
    public float maxFollowDistance = 2f; // Max distance camera can lag behind pivot

    private float verticalRotation = 0f; // Tracks the vertical rotation of the pivot

    void Start()
    {
        // Lock and hide the cursor for gameplay
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Rotate the mech horizontally (left/right)
        transform.Rotate(Vector3.up * mouseX * horizontalSpeed);

        // Adjust vertical rotation of the pivot point (clamped)
        verticalRotation -= mouseY * verticalSpeed;
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalLookLimit, verticalLookLimit);

        if (cameraPivot != null)
        {
            cameraPivot.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
        }

        // Smoothly move the camera towards the pivot position, maintaining max distance
        if (cameraTransform != null)
        {
            Vector3 targetPosition = cameraPivot.position + cameraPivot.forward * -maxFollowDistance;
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, targetPosition, Time.deltaTime * followSpeed);

            // Ensure the camera looks at the pivot point
            cameraTransform.LookAt(cameraPivot);
        }
    }
}
