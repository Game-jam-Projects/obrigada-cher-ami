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

    public void StartDance(int totalCycles, float moveSpeed, float flySpeed, float delayToMove)
    {
        if (!_isDancing)
        {
            _currentCycle = 0;

            _coroutineHandler.StartCoroutine(DanceRoutine(totalCycles, moveSpeed, flySpeed, delayToMove));
        }
    }

    #endregion

    #region Private Methods

    private IEnumerator DanceRoutine(int totalCycles, float moveSpeed, float flySpeed, float delayToMove)
    {
        _isDancing = true;

        while (_currentCycle < totalCycles)
        {
            yield return _coroutineHandler.StartCoroutine(RotateLeft(moveSpeed, flySpeed, delayToMove));

            _currentCycle++;
        }

        foreach (var tangara in _tangaras)
        {
            tangara.GetComponent<Collider2D>().enabled = true;
        }

        TangaraManager.Instance.ActivateUI();

        _isDancing = false;
    }

    private IEnumerator RotateLeft(float moveSpeed, float flySpeed, float delayToMove)
    {
        int count = _tangaras.Count;
        List<Coroutine> moveCoroutines = new();

        GameObject first = _tangaras[0];

        Coroutine moveFirst = _coroutineHandler.StartCoroutine(MoveToPosition(first,
                                                                              first.transform.position,
                                                                              _places[^1].position,
                                                                              flySpeed,
                                                                              DanceValues.Instance.FlyArcHeight,
                                                                              "fly"));
        moveCoroutines.Add(moveFirst);

        for (int i = 1; i < count; i++)
        {
            yield return new WaitForSeconds(delayToMove);

            Coroutine moveOther = _coroutineHandler.StartCoroutine(MoveToPosition(_tangaras[i],
                                                                                  _tangaras[i].transform.position,
                                                                                  _places[i - 1].position,
                                                                                  moveSpeed,
                                                                                  DanceValues.Instance.HopArcHeight,
                                                                                  "hop"));
            moveCoroutines.Add(moveOther);
        }

        foreach (var move in moveCoroutines)
        {
            yield return move;
        }

        _tangaras.RemoveAt(0);
        _tangaras.Add(first);
    }

    private IEnumerator MoveToPosition(GameObject tangara,
                                      Vector3 startPosition,
                                      Vector3 endPosition,
                                      float speed,
                                      float arcHeight,
                                      string animationTrigger)
    {
        if (tangara.TryGetComponent(out Animator animator))
        {
            animator.SetTrigger(animationTrigger);
        }

        Vector3 midPosition = (startPosition + endPosition) / 2.0f + Vector3.up * arcHeight;

        float distance = Vector3.Distance(startPosition, endPosition);
        float duration = distance / speed;

        float time = 0.0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / duration);

            //Vector3 m1 = Vector3.Lerp(startPosition, midPosition, t);
            //Vector3 m2 = Vector3.Lerp(midPosition, endPosition, t);
            //tangara.transform.position = Vector3.Lerp(m1, m2, t);

            Vector3 pos = Vector3.Lerp(startPosition, endPosition, t);

            float arc = Mathf.Sin(t * Mathf.PI) * arcHeight;
            pos.y += arc;

            tangara.transform.position = pos;

            yield return null;
        }

        tangara.transform.position = endPosition;

        if (animationTrigger == "fly")
        {
            animator.SetTrigger("rest");
            TangaraManager.Instance.StopSound();
        }
    }

    #endregion
}