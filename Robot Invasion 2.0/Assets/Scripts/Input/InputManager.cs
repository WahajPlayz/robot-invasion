using System;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour {
    static PlayerInput _playerInput;
    static InputAction _moveInputActions;
    static InputAction _lookInputActions;

    private void Awake()
    {
        _playerInput = new PlayerInput();
    }

    private void Start()
    {
        _moveInputActions = _playerInput.Gameplay.Move;
        _lookInputActions = _playerInput.Gameplay.Look;
    }

    #region Functions: Public Getters

    public static InputAction moveInputActions() {
        return _moveInputActions;
    }
    public static InputAction lookInputActions() {
        return _lookInputActions;
    }

    #endregion
    
}
