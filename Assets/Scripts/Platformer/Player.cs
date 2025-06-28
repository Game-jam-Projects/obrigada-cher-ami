using UnityEngine;

public class Player : MonoBehaviour, IEventListener
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
    [SerializeField] private GameEvent TravaMovimentacao;
    [SerializeField] private GameEvent DestravaMovimentacao;

    private bool _canMove = true;
    private bool _isGrounded;

    private Rigidbody2D _rigidBody;
    private Animator _animator;
    private bool _isFacingRight = true;
    private float _currentDirection = 1f;

    #endregion

    #region Unity Methods

    private void OnEnable()
    {
        TravaMovimentacao.Subscribe(this);
        DestravaMovimentacao.Subscribe(this);
    }

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        CheckGround();
        UpdateAnimator();
    }

    private void FixedUpdate()
    {
        if (_canMove)
        {
            Move();
            Jump();
            JumpPhysics();
        }
        else
            StopMovement();
    }

    private void OnDisable()
    {
        TravaMovimentacao.Unsubscribe(this);
        DestravaMovimentacao.Unsubscribe(this);
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

    #region Public Methods

    public void LockMovement() => _canMove = false;

    public void UnlockMovement() => _canMove = true;

    public void OnEventRaised(IEvent gameEvent, Component sender, object data)
    {
        gameEvent = (GameEvent)gameEvent;
        if (gameEvent.Name == TravaMovimentacao.Name)
            LockMovement();

        if (gameEvent.Name == DestravaMovimentacao.Name)
            UnlockMovement();
    }

    #endregion

    #region Private Methods

    private void Move()
    {
        _rigidBody.linearVelocity = new Vector2(PlayerInput.Movement.x * _moveSpeed, _rigidBody.linearVelocity.y);
    }

    private void Jump()
    {
        if (PlayerInput.IsJumping && _isGrounded)
        {
            _rigidBody.linearVelocity = new Vector2(_rigidBody.linearVelocity.x, _jumpForce);
            Pulo.Broadcast();
        }

        PlayerInput.IsJumping = false;
    }

    private void JumpPhysics()
    {
        if (_rigidBody.linearVelocity.y < 0)
        {
            _rigidBody.linearVelocity += (_fallMultiplier - 1) * Physics2D.gravity.y * Time.fixedDeltaTime * Vector2.up;
        }
        else if (_rigidBody.linearVelocity.y > 0 && !PlayerInput.IsJumping)
        {
            _rigidBody.linearVelocity += (_lowJumpMultiplier - 1) * Physics2D.gravity.y * Time.fixedDeltaTime * Vector2.up;
        }
    }

    private void StopMovement()
    {
        _rigidBody.linearVelocity = Vector2.zero;
        _animator.SetFloat("Speed", 0);
        _animator.SetFloat("VerticalVelocity", 0);
    }

    private void CheckGround()
    {
        _isGrounded = Physics2D.OverlapCircle(_groundCheck.position, _groundCheckRadius, _groundLayer);
    }

    private void UpdateAnimator()
    {
        float xSpeed = Mathf.Abs(_rigidBody.linearVelocity.x);
        float ySpeed = _rigidBody.linearVelocity.y;

        // Atualiza direção
        if (PlayerInput.Movement.x > 0.1f) _currentDirection = 1f;
        else if (PlayerInput.Movement.x < -0.1f) _currentDirection = -1f;

        _animator.SetFloat("Speed", xSpeed);
        _animator.SetFloat("VerticalVelocity", ySpeed);
        _animator.SetBool("IsGrounded", _isGrounded);

        if (_canMove)
            _animator.SetFloat("Direction", _currentDirection);

        // Controle manual do pulo quando necessário
        if (!_isGrounded)
        {
            if (ySpeed > 0.5f)
            {
                _animator.Play(_currentDirection > 0 ? "comeco voo direito" : "comeco voo esquerdo");
            }
            else if (ySpeed < -0.5f)
            {
                _animator.Play(_currentDirection > 0 ? "fim voo direito" : "fim voo esquerdo");
            }
        }
    }

    #endregion
}