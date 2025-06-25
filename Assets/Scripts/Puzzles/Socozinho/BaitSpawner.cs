using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BaitSpawner : MonoBehaviour
{
    #region Fields

    [SerializeField] private GameObject[] _baitPrefabs;
    [SerializeField] private int _queueSize = 4;

    private readonly Queue<GameObject> _baitQueue = new();

    private Camera _mainCamera;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void Start()
    {
        InitializeBaitQueue();
    }

    private void OnMouseDown()
    {
        if (!SocozinhoGameManager.Instance.IsGameActive) return;

        if (_baitQueue.Count > 0 && Bait.CurrentBait == null)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Vector2 worldPosition = _mainCamera.ScreenToWorldPoint(mousePosition);

            GameObject bait = _baitQueue.Dequeue();

            Instantiate(bait, worldPosition, Quaternion.identity);

            AddRandomBaitToQueue();
        }
    }

    #endregion

    #region Private Methods

    private void InitializeBaitQueue()
    {
        _baitQueue.Clear();

        for (int i = 0; i < _queueSize; i++)
        {
            AddRandomBaitToQueue();
        }
    }

    private void AddRandomBaitToQueue()
    {
        if (_baitPrefabs.Length == 0) return;

        GameObject randomBait = _baitPrefabs[Random.Range(0, _baitPrefabs.Length)];

        _baitQueue.Enqueue(randomBait);
    }

    #endregion
}