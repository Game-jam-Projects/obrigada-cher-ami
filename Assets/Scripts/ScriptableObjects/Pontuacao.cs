using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Pontua��o")]
public class Pontuacao : ScriptableObject
{
    public float Pontos = 0;
    public float PontuacaoAlvo = 100;
    public GameEvent FinalizaPuzzle;

    private void OnEnable() => Pontos = 0;

    public float AddPontos(float pontos)
    {
        Pontos += pontos;
        if (Pontos >= 100)
            FinalizaPuzzle.Broadcast();
        return Pontos;
    }
}
