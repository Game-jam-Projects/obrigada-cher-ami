using System.Collections;
using UnityEngine;

public class SocozinhoGameManager : Singleton<SocozinhoGameManager>
{
    [SerializeField] private EnemyFish _enemyFish;

    private bool _isGameActive = false;

    public bool IsGameActive => _isGameActive;

    private void Start()
    {
        StartCoroutine(WaitForGameStartRoutine());
    }

    public void EndGame()
    {
        _isGameActive = false;

        Debug.Log("Fim do jogo!");
    }

    private bool AreAllFishReady()
    {
        bool isEnemyFishReady = _enemyFish != null && _enemyFish.HasReachedStartPosition;

        return isEnemyFishReady && FishSpawnManager.Instance.HaveAllFishReachedStart();
    }

    private IEnumerator WaitForGameStartRoutine()
    {
        while (!AreAllFishReady())
        {
            yield return null;
        }

        _isGameActive = true;

        Debug.Log("Jogo iniciado!");
    }
}