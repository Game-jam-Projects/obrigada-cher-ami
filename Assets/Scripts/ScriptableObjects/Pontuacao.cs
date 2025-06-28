using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Pontuação")]
public class Pontuacao : ScriptableObject
{
    public float Pontos = 0;
    public float PontuacaoAlvo = 100;
    public GameEvent FinalizaPuzzle;
    public Action PontuacaoAlterada;

    private void OnEnable() => Pontos = 0;

    public float AddPontos(float pontos)
    {
        Pontos += pontos;
        PontuacaoAlterada?.Invoke();
        if (Pontos >= PontuacaoAlvo)
            FinalizaPuzzle.Broadcast();
        return Pontos;
    }

    public void Resetar()
    {
        Pontos = 0;
        PontuacaoAlterada?.Invoke();
    }
}
