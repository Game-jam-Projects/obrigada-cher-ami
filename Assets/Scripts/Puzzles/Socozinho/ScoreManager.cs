using System;
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{
    #region Fields

    [SerializeField] private Pontuacao PontuacaoSocozinho;
    [SerializeField] private Pontuacao PontuacaoBaiacu;

    #endregion

    #region Properties

    public int PlayerScore => (int)Math.Round(PontuacaoSocozinho.Pontos);
    public int EnemyScore => (int)Math.Round(PontuacaoBaiacu.Pontos);

    #endregion

    #region Public Methods

    public void AddPlayerScore(int amount)
    {
        PontuacaoSocozinho.AddPontos(amount);
    }

    public void AddEnemyScore(int amount)
    {
        PontuacaoBaiacu.AddPontos(amount);
    }

    public void ResetScores()
    {
        PontuacaoSocozinho.Resetar();
        PontuacaoBaiacu.Resetar();
    }

    #endregion
}