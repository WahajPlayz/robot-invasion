using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Vector2 mouseSensitvity;

    public Transform player;
    public Transform arms;

    float xRotation, yRotation;

    InputAction _lookInputAction;

    [SerializeField] bool scriptEnabled = true;

    public void Start() {
        DisableMouse();
        PlayerInput _playerInput = new PlayerInput();
        _lookInputAction = _playerInput.Gameplay.Look;
        _lookInputAction.Enable();
    }

    private void Update()
    {
        if (!scriptEnabled) return;
        
        Vector2 _lookInput = _lookInputAction.ReadValue<Vector2>();
        float mouseX = _lookInput.x * Time.deltaTime * mouseSensitvity.x;
        float mouseY = _lookInput.y * Time.deltaTime * mouseSensitvity.y;

        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        
        arms.rotation = Quaternion.Euler(yRotation,xRotation,0f);
    }


    #region Functions: Helper

    void DisableMouse() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    #endregion

    
}