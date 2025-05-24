using NUnit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TangaraDance
{
    #region Fields

    private readonly MonoBehaviour _coroutineHandler;
    private readonly List<GameObject> _tangaras;
    private readonly List<Transform> _places;

    private int _currentCycle = 0;
    private bool _isDancing = false;

    #endregion

    #region Constructor

    public TangaraDance(MonoBehaviour coroutineHandler, List<GameObject> tangaras, List<Transform> places)
    {
        _coroutineHandler = coroutineHandler;
        _tangaras = tangaras;
        _places = places;
    }

    #endregion

    #region Public Methods

    public void StartDance(int totalCycles, float moveSpeed, float delayToMove)
    {
        if (!_isDancing)
        {
            _currentCycle = 0;

            _coroutineHandler.StartCoroutine(DanceRoutine(totalCycles, moveSpeed, delayToMove));
        }
    }

    #endregion

    #region Private Methods

    private IEnumerator DanceRoutine(int totalCycles, float moveSpeed, float delayToMove)
    {
        _isDancing = true;

        while (_currentCycle < totalCycles)
        {
            yield return _coroutineHandler.StartCoroutine(RotateLeft(moveSpeed, delayToMove));

            _currentCycle++;
        }

        _isDancing = false;
    }

    private IEnumerator RotateLeft(float moveSpeed, float delayToMove)
    {
        int count = _tangaras.Count;
        List<Coroutine> moveCoroutines = new();

        GameObject first = _tangaras[0];

        Coroutine moveFirst = _coroutineHandler.StartCoroutine(FlyToPosition(first, first.transform.position, _places[^1].position, moveSpeed));
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
        if (tangara.TryGetComponent(out Animator animator))
        {
            float baseAnimSpeed = 5.0f; // change this!

            animator.speed = Mathf.Clamp(moveSpeed / baseAnimSpeed, 0.5f, 2.0f);

            animator.SetTrigger("hop");
        }

        while (Vector3.Distance(tangara.transform.position, targetPosition) > 0.01f)
        {
            tangara.transform.position = Vector3.MoveTowards(tangara.transform.position, targetPosition, moveSpeed * Time.deltaTime);

            yield return null;
        }

        tangara.transform.position = targetPosition;
    }

    private IEnumerator FlyToPosition(GameObject tangara, Vector3 startPosition, Vector3 endPosition, float moveSpeed)
    {
        if (tangara.TryGetComponent(out Animator animator))
        {
            animator.SetTrigger("fly");
        }

        Vector3 midPosition = (startPosition + endPosition) / 2.0f + Vector3.up * -1.5f; // Arc height adjustable

        float distance = Vector3.Distance(startPosition, endPosition);
        float duration = distance / moveSpeed; 

        float time = 0.0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / duration);

            Vector3 m1 = Vector3.Lerp(startPosition, midPosition, t);
            Vector3 m2 = Vector3.Lerp(midPosition, endPosition, t);
            tangara.transform.position = Vector3.Lerp(m1, m2, t);

            yield return null;
        }

        tangara.transform.position = endPosition;
    }


    #endregion
}