using System.Collections;
using UnityEngine;

public class EnemyFish : Fish
{
    #region Fields

    [Header("Predator Behavior")]
    [SerializeField] private float _eatRadius = 0.5f;
    [SerializeField] private Vector2 _eatRadiusOffset = Vector2.zero;
    [SerializeField] private LayerMask _fishLayerMask;

    [Header("Lane")]
    [SerializeField] private Lane _enemyLane;

    [Header("Full Belly Cooldown")]
    [SerializeField] private int _maxFishBeforeFull = 6;
    [SerializeField] private float _fullBellyDuration = 5.0f;

    private int _fishEaten = 0;
    private bool _isFull = false;

    private Coroutine _fullBellyRoutine;

    #endregion

    #region Unity Methods

    protected override void Start()
    {
        base.Start();

        _canBeScared = false;
        _canBeEaten = false;
        _biteDamage = 999;

        _enemyLane.SetOccupied();

        AssignLane(_enemyLane);
    }

    protected override void Update()
    {
        base.Update();

        if (_currentState == FishState.Idle) return;

        if (!_hasReachedStartPosition && Vector2.Distance(transform.position, _startPosition) < 0.01f)
        {
            _hasReachedStartPosition = true;
        }

        if (_hasReachedStartPosition)
        {
            CheckForFishToEat();
        }
    }

#if UNITY_EDITOR

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        if (!Application.isPlaying)
        {
            Gizmos.DrawWireSphere((Vector2)transform.position + _eatRadiusOffset, _eatRadius);

            return;
        }

        Vector2 eatCenter = GetEatRadiusCenter();

        Gizmos.DrawWireSphere(eatCenter, _eatRadius);
    }

#endif

    #endregion

    #region Protected Overriden Methods

    protected override void ChaseBait()
    {
        if (ShouldAbortChase()) return;

        SwimDirectlyToBait();
    }

    protected override void HandleBaitPlaced()
    {
        if (_isFull) return;

        base.HandleBaitPlaced();
    }

    protected override IEnumerator EnterCatchWindowRoutine()
    {
        yield break;
    }

    #endregion

    #region Private Methods

    private void CheckForFishToEat()
    {
        if (_isFull) return;

        Vector2 eatCenter = GetEatRadiusCenter();

        Collider2D[] hits = Physics2D.OverlapCircleAll(eatCenter, _eatRadius, _fishLayerMask);

        foreach (Collider2D hit in hits)
        {
            Fish otherFish = hit.GetComponent<Fish>();

            if (otherFish != null
                && otherFish != this
                && otherFish.CanBeEaten
                && otherFish.HasReachedStartPosition)
            {
                EatFish(otherFish);
            }
        }
    }

    private void EatFish(Fish fish)
    {
        _animator.SetTrigger("eat");

        FishSpawnManager.Instance.HandleFishDestroyed(fish);

        Destroy(fish.gameObject);

        ScoreManager.Instance.AddEnemyScore(fish.ScoreValue);

        _fishEaten++;

        if (_fishEaten >= _maxFishBeforeFull)
        {
            _isFull = true;
            _fishEaten = 0;

            _animator.SetBool("isFull", true);

            if (_fullBellyRoutine != null)
            {
                StopCoroutine(_fullBellyRoutine);
            }

            _fullBellyRoutine = StartCoroutine(FullBellyCooldownRoutine());
        }
    }

    private void SwimDirectlyToBait()
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

    private Vector2 GetEatRadiusCenter()
    {
        Vector2 offset = _spriteRenderer.flipX
            ? new Vector2(-_eatRadiusOffset.x, _eatRadiusOffset.y)
            : _eatRadiusOffset;

        return (Vector2)transform.position + offset;
    }

    #region Coroutines

    private IEnumerator FullBellyCooldownRoutine()
    {
        ChangeToReturningState(_patrolSpeed);

        while (Vector2.Distance(transform.position, _startPosition) > 0.01f)
        {
            yield return null;
        }

        _currentState = FishState.Idle;

        yield return new WaitForSeconds(_fullBellyDuration);

        _isFull = false;

        _animator.SetBool("isFull", false);

        _currentState = FishState.Patrolling;

        _spriteRenderer.color = Color.white; // debug
    }

    #endregion

    #endregion
}