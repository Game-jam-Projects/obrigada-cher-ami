using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    #region Fields

    private Vector2 _movement;
    private bool _isJumping;

    private InputSystem_Actions _inputActions;

    #endregion

    #region Properties

    public Vector2 Movement => _movement;

    public bool IsJumping { get => _isJumping; set => _isJumping = value; }

    #endregion

    #region Unity Methods

    private void Awake()
    {
        _inputActions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        _inputActions.Enable();

        _inputActions.Player.Move.performed += ctx => _movement = ctx.ReadValue<Vector2>();
        _inputActions.Player.Move.canceled += ctx => _movement = Vector2.zero;

        _inputActions.Player.Jump.performed += ctx => _isJumping = true;
    }

    private void OnDisable()
    {
        _inputActions.Player.Move.performed -= ctx => _movement = ctx.ReadValue<Vector2>();
        _inputActions.Player.Move.canceled -= ctx => _movement = Vector2.zero;

        _inputActions.Player.Jump.performed -= ctx => _isJumping = true;

        _inputActions.Disable();
    }

    #endregion
}