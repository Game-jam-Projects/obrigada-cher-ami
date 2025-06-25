using System.Collections.Generic;
using UnityEngine;

public class LaneManager : Singleton<LaneManager>
{
    #region Fields

    [SerializeField] private Lane[] _lanes;

    private readonly List<Lane> _availableLanes = new();

    #endregion

    #region Public Methods

    public bool TryGetAvailableLane(out Lane availableLane)
    {
        _availableLanes.Clear();

        foreach (Lane lane in _lanes)
        {
            if (!lane.IsOccupied)
            {
                _availableLanes.Add(lane);
            }
        }

        if (_availableLanes.Count > 0)
        {
            availableLane = _availableLanes[Random.Range(0, _availableLanes.Count)];

            availableLane.SetOccupied();

            return true;
        }

        availableLane = null;

        return false;
    }

    public void ReleaseLane(Lane lane)
    {
        if (lane != null)
        {
            lane.SetAvailable();
        }
    }

    #endregion
}