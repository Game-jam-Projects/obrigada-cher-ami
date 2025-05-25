using UnityEngine;

public class Player : MonoBehaviour
{
    #region Fields

    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 5.0f;

    [Header("Jumping")]
    [SerializeField] private float _jumpForce = 10.0f;
    [SerializeField] private float _fallMultiplier = 2.5f;
    [SerializeField] private float _lowJumpMultiplier = 2.0f;

    [Header("Ground Check")]
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float _groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask _groundLayer;

    private bool _isGrounded;

    private Rigidbody2D _rigidBody;
    private PlayerInput _input;

    #endregion

    #region Unity Methods

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _input = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        CheckGround();
    }

    private void FixedUpdate()
    {
        Move();
        Jump();
        JumpPhysics();
    }

    private void OnDrawGizmosSelected()
    {
        if (_groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_groundCheck.position, _groundCheckRadius);
        }
    }

    #endregion

    #region Private Methods

    private void Move()
    {
        _rigidBody.linearVelocity = new Vector2(_input.Movement.x * _moveSpeed, _rigidBody.linearVelocity.y);
    }

    private void Jump()
    {
        if (_input.IsJumping && _isGrounded)
        {
            _rigidBody.linearVelocity = new Vector2(_rigidBody.linearVelocity.x, _jumpForce);
        }

        _input.IsJumping = false;
    }

    private void JumpPhysics()
    {
        if (_rigidBody.linearVelocity.y < 0)
        {
            _rigidBody.linearVelocity += (_fallMultiplier - 1) * Physics2D.gravity.y * Time.fixedDeltaTime * Vector2.up;
        }
        else if (_rigidBody.linearVelocity.y > 0 && !_input.IsJumping)
        {
            _rigidBody.linearVelocity += (_lowJumpMultiplier - 1) * Physics2D.gravity.y * Time.fixedDeltaTime * Vector2.up;
        }
    }

    private void CheckGround()
    {
        _isGrounded = Physics2D.OverlapCircle(_groundCheck.position, _groundCheckRadius, _groundLayer);
    }

    #endregion
}