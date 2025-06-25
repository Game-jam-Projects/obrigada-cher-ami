using System.Collections.Generic;
using UnityEngine;

public class FishChaseManager : Singleton<FishChaseManager>
{
    #region Fields

    [SerializeField] private int _maxChasers = 3;

    private readonly HashSet<Fish> _activeChasers = new();

    #endregion

    #region Properties

    public ICollection<Fish> ActiveChasers => _activeChasers;

    #endregion

    #region Public Methods

    public bool TryRegister(Fish fish)
    {
        if (_activeChasers.Contains(fish))
        {
            fish.SetAsActiveChaser(true);

            return true;
        }

        if (_activeChasers.Count >= _maxChasers)
        {
            fish.SetAsActiveChaser(false);

            return false;
        }

        _activeChasers.Add(fish);

        fish.SetAsActiveChaser(true);

        return true;
    }

    public void Unregister(Fish fish)
    {
        _activeChasers.Remove(fish);

        fish.SetAsActiveChaser(false);
    }

    #endregion
}