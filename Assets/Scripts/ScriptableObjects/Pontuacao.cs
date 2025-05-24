using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Pontua��o")]
public class Pontuacao : ScriptableObject
{
    public float Pontos = 0;

    private void OnEnable() => Pontos = 0;
}
