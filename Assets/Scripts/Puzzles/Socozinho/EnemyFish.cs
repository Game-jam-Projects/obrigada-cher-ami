using System.Collections;
using UnityEngine;

public class EnemyFish : Fish
{
    #region Fields

    [Header("Predator Behavior")]
    [SerializeField] private float _eatRadius = 0.5f;
    [SerializeField] private Vector2 _eatRadiusOffset = Vector2.zero;
    [SerializeField] private LayerMask _fishLayerMask;
    [SerializeField] private Lane _enemyLane;

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

    #region Protected Methods

    protected override void ChaseBait()
    {
        if (ShouldAbortChase()) return;

        SwimDirectlyToBait();
    }

    protected override IEnumerator EnterCatchWindowRoutine()
    {
        yield break;
    }

    #endregion

    #region Private Methods

    private void CheckForFishToEat()
    {
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
        FishSpawnManager.Instance.HandleFishDestroyed(fish);

        Destroy(fish.gameObject);

        ScoreManager.Instance.AddEnemyScore(fish.ScoreValue);
    }

    private void SwimDirectlyToBait()
    {
        float distance = Vector2.Distance(transform.position, _currentTargetBait.transform.position);

        if (distance > _baitBiteRange)
        {
            SwimTowards(_currentTargetBait.transform.position, _patrolSpeed);
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

    #endregion
}