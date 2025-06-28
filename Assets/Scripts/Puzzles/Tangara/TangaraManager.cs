using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class TangaraManager : Singleton<TangaraManager>
{
    #region Fields

    [SerializeField] private TangaraLevelObject _level;
    [SerializeField] private GameObject _tangaraPrefab;
    [SerializeField] private Transform _startingPosition;
    [SerializeField] private float _distanceBetween = 1.5f;
    [SerializeField] private List<Transform> _predefinedPlaces;
    [SerializeField] private RectTransform _uiStart;
    [SerializeField] private RectTransform _uiChoose;
    [SerializeField] private RectTransform _uiRight;
    [SerializeField] private RectTransform _uiWrong;
    [SerializeField] private GameObject _uiWin;
    [SerializeField] private UnityEvent winAction;

    [Header("Sound Effects")]
    [SerializeField] private AudioSource _sfx;
    [SerializeField] private GameEvent _winSound;
    [SerializeField] private GameEvent _wrongSound;
    [SerializeField] private GameEvent _piruetaSound;
    [SerializeField] private GameEvent _fimPuzzle;

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

        _uiStart.gameObject.SetActive(true);
        _uiChoose.gameObject.SetActive(false);
        _uiRight.gameObject.SetActive(false);
        _uiWrong.gameObject.SetActive(false);

        _canStart = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            SceneController.LoadScene("Hub 3");

        if (_startAction.triggered && _canStart && !_level.IsFinished)
        {
            _canStart = false;

            _uiStart.gameObject.SetActive(false);
            _uiRight.gameObject.SetActive(false);
            _uiWrong.gameObject.SetActive(false);
            _uiWin.SetActive(false);

            if (_alpha.TryGetComponent(out Animator animator))
            {
                animator.SetTrigger("flip");
                _piruetaSound.Broadcast();
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

        _uiChoose.gameObject.SetActive(false);
        _uiRight.gameObject.SetActive(true);

        _level.AdvanceRound();

        if (_level.IsFinished)
        {
            _uiRight.gameObject.SetActive(false);
            _fimPuzzle.Broadcast();
            _uiWin.SetActive(true);
        }
        else if (_level.CurrentRound.NewAmount > 0)
        {
            AddMore(_level.CurrentRound.NewAmount);

            _dance = new TangaraDance(this, _tangaras, _places);

            _canStart = true; //remover
            _uiStart.gameObject.SetActive(true);

            DisableClicks();
        }
        else
        {
            _canStart = true;
            _uiStart.gameObject.SetActive(true);

            DisableClicks();
        }
    }

    public void PlayerLoses()
    {
        _wrongSound.Broadcast();

        _uiWrong.gameObject.SetActive(true);
        _uiChoose.gameObject.SetActive(false);
        _uiStart.gameObject.SetActive(true);

        _canStart = true;

        DisableClicks();
    }

    public void ActivateUI()
    {
        _uiChoose.gameObject.SetActive(true);
    }

    public void StopSound()
    {
        _sfx.Stop();
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
            //position += new Vector3(_distanceBetween, 0, 0);

            GameObject tangara = Instantiate(_tangaraPrefab, _predefinedPlaces[i].position, Quaternion.identity);

            if (alphaIndex == i)
            {
                _alpha = tangara.GetComponent<Tangara>();
                _alpha.SetIsAlpha();
            }

            tangaras.Add(tangara);

            if (tangara.TryGetComponent(out Animator animator))
            {
                animator.SetTrigger("summon");
            }
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
            //position += new Vector3(_distanceBetween, 0, 0);

            GameObject tangara = Instantiate(_tangaraPrefab, _predefinedPlaces[lastIndex + i + 1].position, Quaternion.identity);

            _tangaras.Add(tangara);

            Transform place = new GameObject("Position_" + (lastIndex + i + 1)).transform;

            place.position = tangara.transform.position;
            _places.Add(place);

            if (tangara.TryGetComponent(out Animator animator))
            {
                animator.SetTrigger("summon");
            }
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