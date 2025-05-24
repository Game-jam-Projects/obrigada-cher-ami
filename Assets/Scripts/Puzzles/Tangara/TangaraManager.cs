using System.Collections.Generic;
using UnityEngine;

public class TangaraManager : Singleton<TangaraManager>
{
    #region Fields

    [SerializeField] private TangaraLevelObject _level;
    [SerializeField] private GameObject _tangaraPrefab;
    [SerializeField] private Transform _startingPosition;

    private List<GameObject> _tangaras;
    private List<Transform> _places;

    private TangaraDance _dance;

    #endregion

    #region Unity Methods

    private void Start()
    {
        _tangaras = CreateTangaras();
        _places = CreatePlaces();

        _dance = new TangaraDance(this, _tangaras, _places);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !_level.IsFinished)
        {
            _dance.StartDance(_level.CurrentRound.TotalCycles, _level.CurrentRound.MoveSpeed, _level.CurrentRound.DelayToMove);
        }
    }

    #endregion

    #region Public Methods

    public void PlayerWins()
    {
        Debug.Log("Aeee, você acertou!");
        _level.AdvanceRound();

        if (_level.IsFinished) Debug.Log("ACABOU!");
        else if (_level.CurrentRound.NewAmount > 0)
        {
            AddMore(_level.CurrentRound.NewAmount);

            _dance = new TangaraDance(this, _tangaras, _places);
        }
    }

    public void PlayerLoses()
    {
        Debug.Log("Errou rude!");
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
            position += new Vector3(1.5f, 0, 0);

            GameObject tangara = Instantiate(_tangaraPrefab, position, Quaternion.identity);

            if (alphaIndex == i)
            {
                tangara.GetComponent<Tangara>().SetIsAlpha();
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
            position += new Vector3(1.5f, 0, 0);

            GameObject tangara = Instantiate(_tangaraPrefab, position, Quaternion.identity);

            _tangaras.Add(tangara);

            Transform place = new GameObject("Position_" + (lastIndex + i + 1)).transform;

            place.position = tangara.transform.position;
            _places.Add(place);
        }
    }

    #endregion
}