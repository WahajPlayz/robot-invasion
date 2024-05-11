/*
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.InputSystem;
using PlayerSet = PlayerInputs.PlayerActions;


public class PlayerController : MonoBehaviour
{
	
     
    [Header("Movements")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float jumpForce;
    private Vector3 moveDirection = Vector3.zero;

    private CharacterController controller;
    [Header("Gravity")]
    [SerializeField] private float gravity;

    [SerializeField] private float groundDistance;
    [SerializeField] private LayerMask GroundMask;
    bool isCharacterGrounded = false;
    private Vector3 velocity = Vector3.zero;

    [Header("Animations")]
    Animator anim;

    
    bool isMoving;

	#region Inputs Variable
    	PlayerInputs playerInputs;
        
	    PlayerSet PlayerControlSet;
	    InputAction _move, _run,_jump, _look;

	#endregion


	void Awake() {
        InitAwakeVariables();
        GetRefereces();
	}

	void Start() {
        InitVariables();
    }

	void OnDisable() {
        TurnOffPlayerControlSet();
	}

	void OnDestroy() {
        TurnOffPlayerControlSet();	
	}

	void Update() {
        isMoving = false;
        Vector2 _moveDirection = PlayerControlSet.Move.ReadValue<Vector2>();

        if (WasThisTriggered(_move)) Move(_moveDirection);

        if (_run.WasReleasedThisFrame()) SetWalkSpeed();

        if (WasThisTriggered(_run) && IsMoving() && isCharacterGrounded) SetRunSpeed();

        if (WasThisTriggered(_jump)) Jump();

        UpdateValues();
    }

	void UpdateValues() {
		HandleIsGrounded();
		HandleGravity();
		HandleAnimations();
	}

	void Move(Vector2 inputMoveDir) {
        isMoving = true;
        moveDirection = transform.TransformDirection(new Vector3(inputMoveDir.x, 0, inputMoveDir.y).normalized);
        controller.Move(moveDirection * moveSpeed * Time.deltaTime);
    }

    void SetRunSpeed() {
            moveSpeed = runSpeed;
    }
    void SetWalkSpeed() {
		moveSpeed = walkSpeed;
	}
    
    private void HandleAnimations()
    {
        if(moveDirection == Vector3.zero)
        {
            anim.SetFloat("Speed", 0, 0.2f, Time.deltaTime);
        }
        else if(moveDirection != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))
        {
            anim.SetFloat("Speed", 0.5f, 0.2f, Time.deltaTime);
        }
        else if (moveDirection != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
        {
            anim.SetFloat("Speed", 1f, 0.2f, Time.deltaTime);
        }
    }
    void HandleIsGrounded()
    {
        isCharacterGrounded = Physics.CheckSphere(transform.position, groundDistance);
    }

    void HandleGravity()
    {
        if(isCharacterGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void Jump() {
        //controller.
        
        velocity.y += Mathf.Sqrt(jumpForce * -2f * gravity) ;
    }

    


    void GetRefereces() {
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        playerInputs = GetComponentInChildren<PlayerInputs>();
    }

    void InitVariables() { 
        moveSpeed = walkSpeed;
		PlayerControlSet = playerInputs.Player;
	}

    void InitAwakeVariables() {
        playerInputs = new PlayerInputs();
		
        _move = PlayerControlSet.Move;
		_run = PlayerControlSet.Run;
		_jump = PlayerControlSet.Jump;
		_look = PlayerControlSet.Look;
        TurnOnPlayerControlSet();
	}

    void TurnOnPlayerControlSet() => PlayerControlSet.Enable();
    void TurnOffPlayerControlSet() => PlayerControlSet.Disable();
    bool WasThisTriggered(InputAction action) => action.WasPerformedThisFrame();

    bool IsMoving() => isMoving;
}
*/