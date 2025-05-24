using System.Collections.Generic;
using UnityEngine;

public class TangaraManager : MonoBehaviour
{
    [SerializeField] private int _totalTangaras = 3;
    [SerializeField] private GameObject _tangaraPrefab;
    [SerializeField] private Transform _startingPosition;
    [SerializeField] private int _totalRounds = 2;
    [SerializeField] private float _moveSpeed = 5.0f;
    [SerializeField] private float _delayToMove = 0.1f;

    private TangaraDance _dance;

    private void Start()
    {
        List<GameObject> tangaras = new();
        Vector3 position = _startingPosition.position;

        for (int i = 0; i < _totalTangaras; i++)
        {
            Vector3 newPosition = position + new Vector3(1.5f, 0, 0);

            GameObject tangara = Instantiate(_tangaraPrefab, newPosition, Quaternion.identity);

            tangaras.Add(tangara);
        }

        _dance = new TangaraDance(this, tangaras);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _dance.StartDance(_totalRounds, _moveSpeed, _delayToMove);
        }
    }
}