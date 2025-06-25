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
    [SerializeField] private float _waterSurfaceLevel = 2.15f;

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

    #endregion

    #region Private Methods

    private IEnumerator FloatToSurfaceRoutine()
    {
        while (!_isFloating)
        {
            if (_rigidbody.linearVelocity.y <= 0.01f && transform.position.y <= _waterSurfaceLevel)
            {
                _rigidbody.linearVelocity = Vector2.zero;
                _rigidbody.bodyType = RigidbodyType2D.Kinematic;

                Vector3 position = transform.position;
                position.y = _waterSurfaceLevel;

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

    #endregion
}