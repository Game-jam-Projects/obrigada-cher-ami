using TMPro;
using UnityEngine;

public class ScoreTextHandler : MonoBehaviour
{
    [SerializeField] private Pontuacao Pontuacao;

    private TextMeshProUGUI _text;

    void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
        Pontuacao.PontuacaoAlterada += AtualizaPlacar;
    }

    private void AtualizaPlacar() => _text.text = Pontuacao.Pontos.ToString();
}
