using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawnManager : Singleton<FishSpawnManager>
{
    #region Fields

    [SerializeField] private GameObject[] _fishPrefabs;
    [SerializeField] private int _initialFishCount = 6;
    [SerializeField] private float _respawnDelay = 2.0f;
    [SerializeField, Range(0f, 1f)] private float _minimumActiveFishRatio = 0.5f;

    private readonly List<Fish> _activeFish = new();

    private bool _isRespawning = false;

    #endregion

    #region Unity Methods

    private void Start()
    {
        for (int i = 0; i < _initialFishCount; i++)
        {
            SpawnFish();
        }
    }

    #endregion

    #region Public Methods

    public bool HaveAllFishReachedStart()
    {
        foreach (Fish fish in _activeFish)
        {
            if (!fish.HasReachedStartPosition)
            {
                return false;
            }
        }

        return true;
    }

    public void HandleFishDestroyed(Fish fish)
    {
        _activeFish.Remove(fish);

        float currentFishRatio = (float)_activeFish.Count / _initialFishCount;

        bool shouldRespawn = !_isRespawning && currentFishRatio < _minimumActiveFishRatio;

        if (shouldRespawn)
        {
            StartCoroutine(RespawnRoutine());
        }
    }

    #endregion

    #region Private Methods

    private void SpawnFish()
    {
        if (!LaneManager.Instance.TryGetAvailableLane(out Lane availableLane)) return;

        GameObject prefab = _fishPrefabs[Random.Range(0, _fishPrefabs.Length)];

        Vector2 spawnPosition = FishSpawnArea.Instance.GetRandomSpawnPosition();

        GameObject instance = Instantiate(prefab, spawnPosition, Quaternion.identity);

        Fish fish = instance.GetComponent<Fish>();

        fish.AssignLane(availableLane);
        fish.EnterSea();

        _activeFish.Add(fish);
    }

    #region Coroutines

    private IEnumerator RespawnRoutine()
    {
        _isRespawning = true;

        while (_activeFish.Count < _initialFishCount)
        {
            SpawnFish();

            yield return new WaitForSeconds(_respawnDelay);
        }

        _isRespawning = false;
    }

    #endregion

    #endregion
}