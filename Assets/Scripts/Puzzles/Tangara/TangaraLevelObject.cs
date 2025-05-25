using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTangaraLevel", menuName = "Scriptable Objects/Novo Level do Tangará")]
public class TangaraLevelObject : ScriptableObject
{
    #region Fields

    [SerializeField] private int _initialAmount;
    [SerializeField] private List<DanceRound> _danceRounds;

    private int _currentIndex = 0;

    #endregion

    #region Properties

    public int StartingNumber => _initialAmount;
    public DanceRound CurrentRound => _danceRounds[_currentIndex];
    public bool IsFinished => _currentIndex == _danceRounds.Count;

    #endregion

    #region Unity Methods

    private void OnEnable()
    {
        _currentIndex = 0;
    }

    #endregion

    #region Public Methods

    public void AdvanceRound()
    {
        if (_currentIndex < _danceRounds.Count) _currentIndex++;
    }

    #endregion
}

[Serializable]
public class DanceRound
{
    public int TotalCycles;
    public float MoveSpeed;
    public float FlySpeed;
    public float DelayToMove;
    public int NewAmount;
}