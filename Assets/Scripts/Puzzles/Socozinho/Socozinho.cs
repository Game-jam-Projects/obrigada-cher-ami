using UnityEngine;
using UnityEngine.InputSystem;

public class Socozinho : Singleton<Socozinho>
{
    #region Fields

    [SerializeField] private float _biteRange = 3.0f;
    [SerializeField] private float _scareRadius = 6.0f;
    [SerializeField] private Transform _beakPosition;

    private bool _isCatching = false;

    private Camera _mainCamera;
    private Animator _animator;

    #endregion

    #region Properties

    public float BiteRange => _biteRange;
    public Vector3 BeakPosition => _beakPosition.position;

    #endregion

    #region Unity Methods

    protected override void Awake()
    {
        base.Awake();

        _mainCamera = Camera.main;
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        InputSystem.actions["Attack"].performed += OnCatchPerformed;
        InputSystem.actions["Attack"].Enable();
    }

    private void OnDestroy()
    {
        InputSystem.actions["Attack"].performed -= OnCatchPerformed;
    }

#if UNITY_EDITOR

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _biteRange);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, _scareRadius);
    }

#endif

    #endregion

    #region Public Methods

    public void AddScore(int amount)
    {
        ScoreManager.Instance.AddPlayerScore(amount);
    }

    public void ScareNearbyFish()
    {
        Fish[] allFish = FindObjectsByType<Fish>(FindObjectsSortMode.None);

        foreach (Fish fish in allFish)
        {
            if (!fish.CanBeEaten) continue;

            float distance = Vector2.Distance(transform.position, fish.transform.position);

            if (distance <= _scareRadius)
            {
                fish.EnterScaredState();
            }
        }
    }

    public void OnCatchAnimationFinished()
    {
        _isCatching = false;
    }

    #endregion

    #region Private Methods

    private void OnCatchPerformed(InputAction.CallbackContext context)
    {
        if (_isCatching || !SocozinhoGameManager.Instance.IsGameActive) return;

        Vector2 mousePosition = _mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        if (SeaArea.Instance.IsInsideSea(mousePosition))
        {
            _isCatching = true;
            _animator.SetTrigger("catchFish");
        }
    }

    #endregion
}