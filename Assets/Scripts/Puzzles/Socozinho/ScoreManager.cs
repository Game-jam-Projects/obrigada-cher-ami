using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{
    #region Fields

    private int _playerScore;
    private int _enemyScore;

    #endregion

    #region Properties

    public int PlayerScore => _playerScore;
    public int EnemyScore => _enemyScore;

    #endregion

    #region Public Methods

    public void AddPlayerScore(int amount)
    {
        _playerScore += amount;

        Debug.Log($"Player Score: {_playerScore}");
    }

    public void AddEnemyScore(int amount)
    {
        _enemyScore += amount;

        Debug.Log($"Enemy Score: {_enemyScore}");
    }

    public void ResetScores()
    {
        _playerScore = 0;
        _enemyScore = 0;
    }

    #endregion
}