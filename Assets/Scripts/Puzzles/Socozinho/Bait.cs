using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bait : MonoBehaviour
{
    #region Delegates

    public static event Action OnBaitConsumed;

    public static event Action OnBaitFloating;

    #endregion

    #region Fields

    public static Bait CurrentBait;

    [SerializeField] private int _hitPoints = 1;

    private bool _isFloating = false;

    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;

    #endregion

    #region Properties

    public static bool IsFloating => CurrentBait != null && CurrentBait._isFloating;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        CurrentBait = this;

        StartCoroutine(FloatToSurfaceRoutine());
    }

    private void OnDestroy()
    {
        if (CurrentBait == this)
        {
            CurrentBait = null;

            OnBaitConsumed?.Invoke();
        }
    }

    #endregion

    #region Public Methods

    public bool TakeDamageFromBite(int damageAmount)
    {
        _hitPoints -= damageAmount;

        UpdateColor();

        if (_hitPoints <= 0)
        {
            if (CurrentBait == this)
            {
                CurrentBait = null;

                OnBaitConsumed?.Invoke();
            }

            Destroy(gameObject);
            return true;
        }

        return false;
    }

    public void Launch(Vector2 startPosition, Vector2 endPosition, float arcHeight, float speed)
    {
        _rigidbody.bodyType = RigidbodyType2D.Kinematic;

        StartCoroutine(LaunchArcRoutine(startPosition, endPosition, arcHeight, speed));
    }

    #endregion

    #region Private Methods

    private IEnumerator FloatToSurfaceRoutine()
    {
        while (!_isFloating)
        {
            if (_rigidbody.linearVelocity.y <= 0.01f && transform.position.y <= BaitSpawner.Instance.WaterSurfaceLevel)
            {
                _rigidbody.linearVelocity = Vector2.zero;
                _rigidbody.bodyType = RigidbodyType2D.Kinematic;

                Vector3 position = transform.position;
                position.y = BaitSpawner.Instance.WaterSurfaceLevel;

                transform.position = position;

                _isFloating = true;

                OnBaitFloating?.Invoke();
            }

            yield return null;
        }
    }

    private void UpdateColor()
    {
        if (_spriteRenderer == null) return;

        _spriteRenderer.color = _hitPoints switch
        {
            3 => Color.green,
            2 => Color.yellow,
            1 => Color.red,
            _ => Color.gray,
        };
    }

    private IEnumerator LaunchArcRoutine(Vector2 startPosition, Vector2 endPosition, float height, float speed)
    {
        float distance = Vector2.Distance(startPosition, endPosition);
        float adjustedSpeed = speed + distance * 1.0f;
        float duration = distance / adjustedSpeed;

        float time = 0.0f;

        while (time < duration)
        {
            float t = time / duration;

            Vector2 position = Vector2.Lerp(startPosition, endPosition, t);
            position.y += height * 4 * (t - t * t);

            transform.position = position;

            time += Time.deltaTime;

            yield return null;
        }

        transform.position = endPosition;

        _rigidbody.bodyType = RigidbodyType2D.Dynamic;
    }

    #endregion
}