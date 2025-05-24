using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TangaraDance
{
    private readonly MonoBehaviour _coroutineHandler;
    private readonly List<GameObject> _tangaras;
    private readonly List<Transform> _places;

    private int _currentRound = 0;
    private bool _isDancing = false;

    public TangaraDance(MonoBehaviour coroutineHandler, List<GameObject> tangaras)
    {
        _coroutineHandler = coroutineHandler;
        _tangaras = tangaras;

        _places = new List<Transform>();

        for (int i = 0; i < _tangaras.Count; i++)
        {
            Transform place = new GameObject("Position_" + i).transform;

            place.position = _tangaras[i].transform.position;

            _places.Add(place);
        }
    }

    public void StartDance(int totalRounds, float moveSpeed, float delayToMove)
    {
        if (!_isDancing)
        {
            _currentRound = 0;

            _coroutineHandler.StartCoroutine(DanceRoutine(totalRounds, moveSpeed, delayToMove));
        }
    }

    private IEnumerator DanceRoutine(int totalRounds, float moveSpeed, float delayToMove)
    {
        _isDancing = true;

        while (_currentRound < totalRounds)
        {
            yield return _coroutineHandler.StartCoroutine(RotateLeft(moveSpeed, delayToMove));

            _currentRound++;
        }

        _isDancing = false;
    }

    private IEnumerator RotateLeft(float moveSpeed, float delayToMove)
    {
        int count = _tangaras.Count;
        List<Coroutine> moveCoroutines = new();

        GameObject first = _tangaras[0];

        Coroutine moveFirst = _coroutineHandler.StartCoroutine(MoveToPosition(first, _places[^1].position, moveSpeed));
        moveCoroutines.Add(moveFirst);

        for (int i = 1; i < count; i++)
        {
            yield return new WaitForSeconds(delayToMove);

            Coroutine moveOther = _coroutineHandler.StartCoroutine(MoveToPosition(_tangaras[i], _places[i - 1].position, moveSpeed));
            moveCoroutines.Add(moveOther);
        }

        foreach (var move in moveCoroutines)
        {
            yield return move;
        }

        _tangaras.RemoveAt(0);
        _tangaras.Add(first);
    }

    private IEnumerator MoveToPosition(GameObject tangara, Vector3 targetPosition, float moveSpeed)
    {
        while (Vector3.Distance(tangara.transform.position, targetPosition) > 0.01f)
        {
            tangara.transform.position = Vector3.MoveTowards(tangara.transform.position, targetPosition, moveSpeed * Time.deltaTime);

            yield return null;
        }

        tangara.transform.position = targetPosition;
    }
}