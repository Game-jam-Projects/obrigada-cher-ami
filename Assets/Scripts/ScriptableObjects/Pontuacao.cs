using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Pontuação")]
public class Pontuacao : ScriptableObject
{
    public float Pontos = 0;

    private void OnEnable() => Pontos = 0;
}
