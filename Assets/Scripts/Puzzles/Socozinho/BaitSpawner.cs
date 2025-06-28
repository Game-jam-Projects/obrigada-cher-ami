using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BaitSpawner : Singleton<BaitSpawner>
{
    #region Fields

    [Header("Bait Settings")]
    [SerializeField] private GameObject[] _baitPrefabs;
    [SerializeField] private int _queueSize = 4;

    [Header("Throw Arc Settings")]
    [SerializeField] private float _arcHeight = 0.1f;
    [SerializeField] private float _arcDistanceMultiplier = 0.15f;
    [SerializeField] private float _throwSpeed = 3.0f;

    [Header("Water Settings")]
    [SerializeField] private float _waterSurfaceLevel = 2.15f;

    private readonly Queue<GameObject> _baitQueue = new();

    private Camera _mainCamera;

    #endregion

    #region Properties

    public float WaterSurfaceLevel => _waterSurfaceLevel;

    #endregion

    #region Unity Methods

    protected override void Awake()
    {
        base.Awake();

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
            Vector2 cursorPosition = PlayerInput.Cursor;
            Vector2 worldPosition = _mainCamera.ScreenToWorldPoint(cursorPosition);

            Vector2 targetPoint = new(worldPosition.x, _waterSurfaceLevel);

            GameObject baitPrefab = _baitQueue.Dequeue();
            GameObject baitInstance = Instantiate(baitPrefab, Socozinho.Instance.BeakPosition, Quaternion.identity);

            AddRandomBaitToQueue();

            Bait bait = baitInstance.GetComponent<Bait>();
            bait.Launch(Socozinho.Instance.BeakPosition, targetPoint, _arcHeight, _arcDistanceMultiplier, _throwSpeed);
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