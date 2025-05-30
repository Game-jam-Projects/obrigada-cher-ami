using UnityEngine;
using UnityEngine.InputSystem;

public class Comida : MonoBehaviour
{
    [SerializeField] private TipoComida TipoComida = TipoComida.Aleatorio;
    [SerializeField] private Pontuacao Pontuacao;

    private SpriteRenderer _spriteRenderer;
    private InputAction _jumpAction;
    private float _timer = 0;
    private float _resetTime;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _jumpAction = InputSystem.actions.FindAction("Jump");
        _resetTime = Random.Range(3, 7);

        if (TipoComida == TipoComida.Aleatorio)
            TrocaComida();

        CarregaVisualComida();
    }

    private void TrocaComida()
    {
        TipoComida = (TipoComida)Random.Range(1, 5);
        CarregaVisualComida();
    }

    private void CarregaVisualComida()
    {
        _spriteRenderer.color = TipoComida switch
        {
            TipoComida.Crustaceo => new Color(255, 0, 0, 0.3f),
            TipoComida.Peixe => new Color(0, 0, 255, 0.3f),
            TipoComida.Inseto => new Color(0, 255, 0, 0.3f),
            TipoComida.Lixo => new Color(0, 0, 0, 0.3f),
            TipoComida.Aleatorio => _spriteRenderer.color
        };
    }

    void Update()
    {
        if (_jumpAction.triggered) TrocaComida();

        _timer += Time.deltaTime;
        if(_timer >= _resetTime)
        {
            TrocaComida();
            _timer = 0;
        }

    }

    private void OnMouseDown()
    {
        Pontuacao.Pontos = TipoComida switch
        {
            TipoComida.Crustaceo => Pontuacao.Pontos += 10,
            TipoComida.Peixe => Pontuacao.Pontos += 2,
            TipoComida.Inseto => Pontuacao.Pontos += 1,
            TipoComida.Lixo => Pontuacao.Pontos -= 10,
            TipoComida.Aleatorio => Pontuacao.Pontos
        };
    }
}

public enum TipoComida
{
    Aleatorio = 0,
    Crustaceo = 1,
    Peixe = 2,
    Inseto = 3,
    Lixo = 4
}