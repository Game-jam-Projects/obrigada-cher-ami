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

    [Header("Events")]
    [SerializeField] private GameEvent Pulo;

    private bool _isGrounded;

    private Rigidbody2D _rigidBody;
    private PlayerInput _input;
    private Animator _animator;

    #endregion

    #region Unity Methods

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _input = GetComponent<PlayerInput>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        CheckGround();
        UpdateAnimator();
    }

    private void FixedUpdate()
    {
        Move();
        Jump();
        JumpPhysics();
    }

    private void UpdateAnimator()
    {
        var xPos = _rigidBody.linearVelocityX;
        var yPos = _rigidBody.linearVelocityY > 0 ? _rigidBody.linearVelocityY : 0;

        _animator.SetFloat("horizontal", xPos);
        _animator.SetFloat("vertical", yPos);

        Debug.Log($"x:{xPos} y:{yPos}");

        if (_rigidBody.linearVelocity != Vector2.zero)
        {
            _animator.SetFloat("ultHorizontal", xPos);
            _animator.SetFloat("ultVertical", yPos);
        }
    }

    //private void OnDrawGizmosSelected()
    //{
    //    if (_groundCheck != null)
    //    {
    //        Gizmos.color = Color.red;
    //        Gizmos.DrawWireSphere(_groundCheck.position, _groundCheckRadius);
    //    }
    //}

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
            Pulo.Broadcast();
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