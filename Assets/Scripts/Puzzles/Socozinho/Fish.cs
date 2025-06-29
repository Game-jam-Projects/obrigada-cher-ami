using System.Collections;
using UnityEngine;

public class Fish : MonoBehaviour
{
    #region Fields

    [Header("Movement")]
    [SerializeField] protected float _patrolSpeed = 3.0f;
    [SerializeField] protected float _chaseSpeed = 4.0f;
    [SerializeField] private float _enteringSpeed = 6.0f;

    [Header("Score")]
    [SerializeField] private int _scoreValue = 1;

    [Header("Biting")]
    [SerializeField] protected int _biteDamage = 1;
    [SerializeField] protected float _baitBiteRange = 1.0f;
    [SerializeField] private float _timeBetweenBites = 0.5f;
    [SerializeField] private float _delayBeforeChasing = 0.5f;
    [SerializeField] private float _waitingDistance = 2.5f;

    [Header("Interaction")]
    [SerializeField] protected bool _canBeEaten = true;
    [SerializeField] protected bool _canBeScared = true;

    [Header("Catch Window")]
    [SerializeField] private float _catchWindowDuration = 0.5f;

    [Header("Scare Behavior")]
    [SerializeField] private float _scaredSpeed = 5.0f;
    [SerializeField] private float _scaredStateDuration = 1.0f;

    protected Vector2 _startPosition;
    private Vector2 _patrolPointA;
    private Vector2 _patrolPointB;
    private Vector2 _currentPatrolTarget;
    private Vector2 _fleeDirection;

    protected FishState _currentState = FishState.Patrolling;

    protected bool _hasReachedStartPosition = false;
    private bool _isPreparingToChase = false;
    private bool _isActiveChaser = false;

    private float _currentReturnSpeed;

    private float _chaseDelayTimer = 0.0f;
    private float _biteCooldownTimer = 0.0f;

    private Coroutine _scaredRoutine;

    protected Bait _currentTargetBait;
    private Lane _assignedLane;

    protected SpriteRenderer _spriteRenderer;
    protected Animator _animator;

    #endregion

    #region Properties

    public bool HasReachedStartPosition => _hasReachedStartPosition;
    public bool CanBeEaten => _canBeEaten;
    public int ScoreValue => _scoreValue;

    #endregion

    #region Unity Methods

    protected virtual void Start()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        Bait.OnBaitFloating += HandleBaitPlaced;
    }

    private void OnDisable()
    {
        Bait.OnBaitFloating -= HandleBaitPlaced;
    }

    private void OnDestroy()
    {
        FishChaseManager.Instance.Unregister(this);

        if (_assignedLane != null)
        {
            LaneManager.Instance.ReleaseLane(_assignedLane);
        }
    }

    protected virtual void Update()
    {
        ClampToBounds();

        switch (_currentState)
        {
            case FishState.Patrolling:
                CheckForBait();
                Patrol();
                break;

            case FishState.Chasing:
                ChaseBait();
                break;

            case FishState.Returning:
                ReturnToStartPosition();
                break;

            case FishState.DelayBeforeChase:
                UpdateChaseDelayTimer();
                break;

            case FishState.Scared:
                SwimInDirectionOfStartPosition();
                break;

            case FishState.Catchable:
                break;
        }
    }

    private void OnMouseDown()
    {
        if (!_canBeEaten || _currentState != FishState.Catchable) return;

        float distanceToPlayer = Vector2.Distance(transform.position, Socozinho.Instance.transform.position);

        if (distanceToPlayer <= Socozinho.Instance.BiteRange)
        {
            Socozinho.Instance.AddScore(_scoreValue);
            Socozinho.Instance.ScareNearbyFish();

            FishSpawnManager.Instance.HandleFishDestroyed(this);

            Destroy(gameObject);
        }
    }

    #endregion

    #region Public Methods

    public void AssignLane(Lane lane)
    {
        _assignedLane = lane;

        _startPosition = lane.GetRandomPosition(0.3f);

        _patrolPointA = lane.PointA;
        _patrolPointB = lane.PointB;

        _currentPatrolTarget = Vector2.Distance(_startPosition, _patrolPointA) > Vector2.Distance(_startPosition, _patrolPointB)
                    ? _patrolPointB
                    : _patrolPointA;
    }

    public void EnterSea()
    {
        ChangeToReturningState(_enteringSpeed);
    }

    public void EnterScaredState()
    {
        if (!_canBeScared || _currentState == FishState.Scared || _currentState == FishState.Patrolling) return;

        if (_scaredRoutine != null)
        {
            StopCoroutine(_scaredRoutine);
        }

        _spriteRenderer.color = Color.white; // debug
        _spriteRenderer.transform.localRotation = Quaternion.identity;

        _currentTargetBait = null;
        _isPreparingToChase = false;
        _currentState = FishState.Scared;

        _fleeDirection = ((Vector2)transform.position - (Vector2)Socozinho.Instance.transform.position).normalized;

        FishChaseManager.Instance.Unregister(this);

        _scaredRoutine = StartCoroutine(ScaredStateRoutine());
    }

    public void SetAsActiveChaser(bool isChaser)
    {
        _isActiveChaser = isChaser;
    }

    #endregion

    #region Protected & Private Methods

    #region Movement Logic

    private void ClampToBounds()
    {
        Vector3 position = transform.position;

        position.x = Mathf.Clamp(position.x, SeaArea.Instance.MinBounds.x, SeaArea.Instance.MaxBounds.x);
        position.y = Mathf.Clamp(position.y, SeaArea.Instance.MinBounds.y, SeaArea.Instance.MaxBounds.y);

        transform.position = position;
    }

    protected void SwimTowards(Vector2 destination, float speed)
    {
        transform.position = Vector2.MoveTowards(transform.position, destination, speed * Time.deltaTime);
    }

    protected void UpdateFacingDirection(Vector2 targetPosition)
    {
        _spriteRenderer.flipX = targetPosition.x < transform.position.x;
    }

    #endregion

    #region Patrol Logic

    private void Patrol()
    {
        UpdateFacingDirection(_currentPatrolTarget);

        SwimTowards(_currentPatrolTarget, _patrolSpeed);

        if (Vector2.Distance(transform.position, _currentPatrolTarget) < 0.01f)
        {
            _currentPatrolTarget = _currentPatrolTarget == _patrolPointA ? _patrolPointB : _patrolPointA;
        }
    }

    private void CheckForBait()
    {
        if (!_hasReachedStartPosition) return;

        if (Bait.IsFloating && !_isPreparingToChase)
        {
            BeginChaseDelay(true);
        }
    }

    private void BeginChaseDelay(bool isPreparingToChase)
    {
        _isPreparingToChase = isPreparingToChase;
        _chaseDelayTimer = _delayBeforeChasing;
        _currentState = FishState.DelayBeforeChase;
    }

    #endregion

    #region Chase Logic

    protected virtual void ChaseBait()
    {
        if (ShouldAbortChase()) return;

        if (_isActiveChaser)
        {
            ChaseAndBiteBait();
        }
        else
        {
            WaitOrApproachBait();
        }
    }

    protected bool ShouldAbortChase()
    {
        if (Bait.CurrentBait == null)
        {
            FishChaseManager.Instance.Unregister(this);

            _spriteRenderer.transform.localRotation = Quaternion.identity;

            ChangeToReturningState(_patrolSpeed);

            _currentTargetBait = null;

            return true;
        }

        return false;
    }

    private void ChaseAndBiteBait()
    {
        float distance = Vector2.Distance(transform.position, _currentTargetBait.transform.position);

        if (distance > _baitBiteRange)
        {
            SwimTowards(_currentTargetBait.transform.position, _chaseSpeed);
            UpdateFacingDirection(_currentTargetBait.transform.position);

            _spriteRenderer.transform.localRotation = Quaternion.identity;

            return;
        }

        RotateTowardsBait();
        AttemptToBite();
    }

    protected void RotateTowardsBait()
    {
        Vector2 direction = _currentTargetBait.transform.position - _spriteRenderer.transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (_spriteRenderer.flipX)
        {
            angle += 180f;
        }

        _spriteRenderer.transform.localRotation = Quaternion.Euler(0, 0, angle);
    }

    protected void AttemptToBite()
    {
        _biteCooldownTimer -= Time.deltaTime;

        if (_biteCooldownTimer <= 0.0f)
        {
            bool isDestroyed = _currentTargetBait.TakeDamageFromBite(_biteDamage);

            _biteCooldownTimer = _timeBetweenBites;

            if (isDestroyed)
            {
                _animator.SetTrigger("eat");

                _currentTargetBait = null;

                FishChaseManager.Instance.Unregister(this);

                _spriteRenderer.transform.localRotation = Quaternion.identity;

                StartCoroutine(EnterCatchWindowRoutine());
            }
        }
    }

    private void WaitOrApproachBait()
    {
        float distanceToBait = Vector2.Distance(transform.position, _currentTargetBait.transform.position);

        if (distanceToBait <= _waitingDistance)
        {
            bool registered = FishChaseManager.Instance.TryRegister(this);

            if (registered)
            {
                SwimTowards(_currentTargetBait.transform.position, _chaseSpeed);
                UpdateFacingDirection(_currentTargetBait.transform.position);
            }
            else
            {
                Vector2 waitDirection = ((Vector2)transform.position - (Vector2)_currentTargetBait.transform.position).normalized;
                Vector2 waitPosition = (Vector2)_currentTargetBait.transform.position + waitDirection * _waitingDistance;

                SwimTowards(waitPosition, _chaseSpeed);
            }
        }
        else
        {
            SwimTowards(_currentTargetBait.transform.position, _chaseSpeed);
            UpdateFacingDirection(_currentTargetBait.transform.position);
        }

        _spriteRenderer.transform.localRotation = Quaternion.identity;
    }

    #endregion

    #region Returning and Cooldown Logic

    protected void ChangeToReturningState(float speed)
    {
        _currentReturnSpeed = speed;

        if (_hasReachedStartPosition)
        {
            _startPosition = _assignedLane.GetRandomPosition();
        }

        _currentState = FishState.Returning;
    }

    private void ReturnToStartPosition()
    {
        SwimTowards(_startPosition, _currentReturnSpeed);

        _spriteRenderer.flipX = _startPosition.x < transform.position.x;

        if (Vector2.Distance(transform.position, _startPosition) < 0.01f)
        {
            if (_currentState == FishState.Scared)
            {
                _currentState = FishState.Patrolling;
            }
            else
            {
                _currentPatrolTarget = Vector2.Distance(transform.position, _patrolPointA) < Vector2.Distance(transform.position, _patrolPointB)
                    ? _patrolPointB
                    : _patrolPointA;

                _currentState = FishState.Patrolling;
            }

            _hasReachedStartPosition = true;

            _currentReturnSpeed = _patrolSpeed;
        }
    }

    private void UpdateChaseDelayTimer()
    {
        _chaseDelayTimer -= Time.deltaTime;

        if (_chaseDelayTimer <= 0f)
        {
            if (_isPreparingToChase && Bait.CurrentBait != null)
            {
                _currentTargetBait = Bait.CurrentBait;
                _currentState = FishState.Chasing;
            }
            else
            {
                _currentTargetBait = null;

                ChangeToReturningState(_patrolSpeed);
            }
        }
    }

    #endregion

    #region Scared Logic

    private void SwimInDirectionOfStartPosition()
    {
        transform.position += (Vector3)(_scaredSpeed * Time.deltaTime * _fleeDirection);

        _spriteRenderer.flipX = _fleeDirection.x < 0.0f;
    }

    #endregion

    #region Event Handlers

    protected virtual void HandleBaitPlaced()
    {
        if (!_hasReachedStartPosition || _currentState == FishState.Scared) return;

        if (_currentState == FishState.Patrolling || _currentState == FishState.Returning)
        {
            _isPreparingToChase = true;
            _chaseDelayTimer = _delayBeforeChasing;
            _currentState = FishState.DelayBeforeChase;
        }
        else if (_currentState == FishState.DelayBeforeChase)
        {
            _isPreparingToChase = true;
        }
    }

    #endregion

    #region Coroutines

    protected virtual IEnumerator EnterCatchWindowRoutine()
    {
        FishChaseManager.Instance.Unregister(this);

        _currentState = FishState.Catchable;

        _spriteRenderer.color = Color.yellow; // debug

        yield return new WaitForSeconds(_catchWindowDuration);

        _spriteRenderer.color = Color.white; // debug

        _spriteRenderer.transform.localRotation = Quaternion.identity;

        ChangeToReturningState(_patrolSpeed);
    }

    private IEnumerator ScaredStateRoutine()
    {
        yield return new WaitForSeconds(_scaredStateDuration);

        if (Bait.IsFloating && Bait.CurrentBait != null)
        {
            _currentTargetBait = Bait.CurrentBait;

            BeginChaseDelay(true);
        }
        else
        {
            if (Vector2.Distance(transform.position, _startPosition) < 0.01f)
            {
                _currentState = FishState.Patrolling;
            }
            else
            {
                ChangeToReturningState(_patrolSpeed);
            }
        }
    }

    #endregion

    #endregion
}