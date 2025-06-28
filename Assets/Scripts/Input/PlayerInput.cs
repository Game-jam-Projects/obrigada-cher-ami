using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    #region Fields

    private static Vector2 _movement;
    private static Vector2 _cursor;
    private static InputAction _attackAction;
    private static InputAction _interactAction;
    private static bool _isJumping;

    private InputSystem_Actions _inputActions;

    #endregion

    #region Properties

    public static Vector2 Movement => _movement;
    public static Vector2 Cursor => _cursor;
    public static InputAction AttackAction => _attackAction;
    public static InputAction InteractAction => _interactAction;
    public static bool IsJumping { get => _isJumping; set => _isJumping = value; }

    #endregion

    #region Unity Methods

    private void Awake()
    {
        _inputActions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        _inputActions.Enable();

        _inputActions.Player.Move.performed += OnMovePerformed;
        _inputActions.Player.Move.canceled += OnMoveCanceled;

        _inputActions.Player.Cursor.performed += OnCursorPerformed;
        _inputActions.Player.Cursor.canceled += OnCursorCanceled;

        _inputActions.Player.Jump.performed += OnJumpPerformed;

        _attackAction = _inputActions.Player.Attack;
        _interactAction = _inputActions.Player.Interact;
    }

    private void OnDisable()
    {
        _inputActions.Player.Move.performed -= OnMovePerformed;
        _inputActions.Player.Move.canceled -= OnMoveCanceled;

        _inputActions.Player.Look.performed -= OnCursorPerformed;
        _inputActions.Player.Look.canceled -= OnCursorCanceled;

        _inputActions.Player.Jump.performed -= OnJumpPerformed;

        _inputActions.Disable();
    }

    #endregion

    #region Input Actions

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        _movement = context.ReadValue<Vector2>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        _movement = Vector2.zero;
    }

    private void OnCursorPerformed(InputAction.CallbackContext context)
    {
        _cursor = context.ReadValue<Vector2>();
    }

    private void OnCursorCanceled(InputAction.CallbackContext context)
    {
        _cursor = Vector2.zero;
    }

    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        _isJumping = true;
    }

    #endregion
}