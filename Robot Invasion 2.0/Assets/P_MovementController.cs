using UnityEngine;
using GameplayActions = PlayerInput.GameplayActions;

[RequireComponent(typeof(CharacterController))]
public class P_MovementController : MonoBehaviour
{
	public bool invertLookInput;
	public Camera playerCamera;
	public float walkSpeed = 6f;
	public float runSpeed = 12f;
	public float jumpPower = 7f;
	public float gravity = 10f;

	public float lookSpeed = 2f;
	public float lookXLimit = 10f;

	private Vector3 _moveDirection = Vector3.zero;
	private float _rotationX = 0;

	public bool _canMove = true;

	private CharacterController _characterController;

	private GameplayActions _gameplayActionMap;

	private void Start()
	{
		PlayerInput playerInput = new PlayerInput();
		_gameplayActionMap = playerInput.Gameplay;
		_gameplayActionMap.Enable();
		_characterController = GetComponent<CharacterController>();
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	private void Update()
	{
		#region HandleMovement

		Vector3 forward = transform.TransformDirection(Vector3.forward);
		Vector3 right = transform.TransformDirection(Vector3.right);

		bool isRunning = _gameplayActionMap.Run.IsPressed();
		Vector2 movementInput = _gameplayActionMap.Move.ReadValue<Vector2>();

		float curSpeedX = _canMove ? (isRunning ? runSpeed : walkSpeed) * movementInput.y : 0;
		float curSpeedY = _canMove ? (isRunning ? runSpeed : walkSpeed) * movementInput.x : 0;
		float movementDirectionY = _moveDirection.y;
		_moveDirection = (forward * curSpeedX) +(right * curSpeedY);

		#endregion

		#region Handles Jumping

		bool pressedJump = _gameplayActionMap.Jump.IsPressed();

		if (pressedJump && _canMove && _characterController.isGrounded)
		{
			_moveDirection.y = jumpPower;
		}
		else
		{
			_moveDirection.y = movementDirectionY;
		}

		if (!_characterController.isGrounded)
		{
			_moveDirection.y -= gravity * Time.deltaTime;
		}

		#endregion
		
		#region Handles Rotation

		_characterController.Move(_moveDirection * Time.deltaTime);

		float rotationXInput = _gameplayActionMap.Look.ReadValue<Vector2>().x;
		float rotationYInput = _gameplayActionMap.Look.ReadValue<Vector2>().y;
		
		if (_canMove)
		{
			float invertionLookControl = invertLookInput ? -1 : 1;
			_rotationX += invertionLookControl * rotationYInput * lookSpeed;
			_rotationX = Mathf.Clamp(_rotationX, -lookXLimit, lookXLimit);
			playerCamera.transform.localRotation = Quaternion.Euler(_rotationX,0,0);
			transform.rotation *= Quaternion.Euler(0,rotationYInput * lookSpeed,0);
		}

		#endregion
	}
}
