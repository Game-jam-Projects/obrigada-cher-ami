using System.Collections;
using UnityEngine;

public class SocozinhoGameManager : Singleton<SocozinhoGameManager>
{
    #region Fields

    [SerializeField] private float _gameDuration = 60.0f;
    [SerializeField] private EnemyFish _enemyFish;

    private bool _isGameActive = false;
    private int _remainingTime = 0;

    private Coroutine _timerRoutine;

    #endregion

    #region Properties

    public bool IsGameActive => _isGameActive;
    public int RemainingTime => _remainingTime;

    #endregion

    #region Unity Methods

    private void Start()
    {
        StartGame();
    }

    #endregion

    #region Public Methods

    public void EndGame()
    {
        _isGameActive = false;

        if (_timerRoutine != null)
        {
            StopCoroutine(_timerRoutine);

            _timerRoutine = null;
        }

        _remainingTime = 0;

        Debug.Log("Fim do jogo!");
    }

    public void StartGame() => StartCoroutine(WaitForGameStartRoutine());

    #endregion

    #region Private Methods


    private bool AreAllFishReady()
    {
        bool isEnemyFishReady = _enemyFish != null && _enemyFish.HasReachedStartPosition;

        return isEnemyFishReady && FishSpawnManager.Instance.HaveAllFishReachedStart();
    }

    #region Coroutines

    private IEnumerator WaitForGameStartRoutine()
    {
        while (!AreAllFishReady())
        {
            yield return null;
        }

        _isGameActive = true;

        Debug.Log("Jogo iniciado!");

        _timerRoutine = StartCoroutine(TimerRoutine());
    }

    private IEnumerator TimerRoutine()
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < _gameDuration)
        {
            elapsedTime += Time.deltaTime;

            _remainingTime = Mathf.Max(0, Mathf.CeilToInt(_gameDuration - elapsedTime));

            yield return null;
        }

        EndGame();
    }

    #endregion

    #endregion
}