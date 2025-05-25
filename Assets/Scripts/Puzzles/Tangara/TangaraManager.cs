using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TangaraManager : Singleton<TangaraManager>
{
    #region Fields

    [SerializeField] private TangaraLevelObject _level;
    [SerializeField] private GameObject _tangaraPrefab;
    [SerializeField] private Transform _startingPosition;
    [SerializeField] private float _distanceBetween = 1.5f;

    [Header("Sound Effects")]
    [SerializeField] private GameEvent _winSound;

    private Tangara _alpha;
    private List<GameObject> _tangaras;
    private List<Transform> _places;

    private bool _canStart = false;

    private TangaraDance _dance;
    private InputAction _startAction;

    #endregion

    #region Unity Methods

    private void Start()
    {
        _tangaras = CreateTangaras();
        _places = CreatePlaces();

        _dance = new TangaraDance(this, _tangaras, _places);

        _startAction = InputSystem.actions.FindAction("Jump");

        _canStart = true;
    }

    private void Update()
    {
        if (_startAction.triggered && _canStart && !_level.IsFinished)
        {
            _canStart = false;

            if (_alpha.TryGetComponent(out Animator animator))
            {
                animator.SetTrigger("flip");
            }
        }
    }

    #endregion

    #region Public Methods

    public void StartDance()
    {
        _dance.StartDance(_level.CurrentRound.TotalCycles,
                          _level.CurrentRound.MoveSpeed,
                          _level.CurrentRound.FlySpeed,
                          _level.CurrentRound.DelayToMove);
    }

    public void PlayerWins()
    {
        _winSound.Broadcast();

        _level.AdvanceRound();

        if (_level.IsFinished)
        {
            Debug.Log("ACABOU!");
        }
        else if (_level.CurrentRound.NewAmount > 0)
        {
            AddMore(_level.CurrentRound.NewAmount);

            _dance = new TangaraDance(this, _tangaras, _places);

            _canStart = true; //remover

            DisableClicks();
        }
        else
        {
            _canStart = true;

            DisableClicks();
        }
    }

    public void PlayerLoses()
    {
        Debug.Log("Errou rude!");

        _canStart = true;

        DisableClicks();
    }

    #endregion

    #region Private Methods

    private List<GameObject> CreateTangaras()
    {
        List<GameObject> tangaras = new();
        Vector3 position = _startingPosition.position;

        int alphaIndex = Random.Range(0, _level.StartingNumber);

        for (int i = 0; i < _level.StartingNumber; i++)
        {
            position += new Vector3(_distanceBetween, 0, 0);

            GameObject tangara = Instantiate(_tangaraPrefab, position, Quaternion.identity);

            if (alphaIndex == i)
            {
                _alpha = tangara.GetComponent<Tangara>();
                _alpha.SetIsAlpha();
            }

            tangaras.Add(tangara);
        }

        return tangaras;
    }

    private List<Transform> CreatePlaces()
    {
        List<Transform> places = new();

        for (int i = 0; i < _tangaras.Count; i++)
        {
            Transform place = new GameObject("Position_" + i).transform;

            place.position = _tangaras[i].transform.position;

            places.Add(place);
        }

        return places;
    }

    private void AddMore(int amount)
    {
        int lastIndex = _tangaras.Count - 1;
        Vector3 position = _tangaras[lastIndex].transform.position;

        for (int i = 0; i < amount; i++)
        {
            position += new Vector3(_distanceBetween, 0, 0);

            GameObject tangara = Instantiate(_tangaraPrefab, position, Quaternion.identity);

            _tangaras.Add(tangara);

            Transform place = new GameObject("Position_" + (lastIndex + i + 1)).transform;

            place.position = tangara.transform.position;
            _places.Add(place);
        }
    }

    private void DisableClicks()
    {
        foreach (var tangara in _tangaras)
        {
            tangara.GetComponent<Collider2D>().enabled = false;
        }
    }

    #endregion
}