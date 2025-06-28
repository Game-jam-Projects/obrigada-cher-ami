using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Comida : MonoBehaviour
{
    [SerializeField] private TipoComida TipoComida = TipoComida.Aleatorio;
    [SerializeField] private float ValorCrustaceo = 30;
    [SerializeField] private float ValorPeixe = 10;
    [SerializeField] private float ValorInseto = 5;
    [SerializeField] private float ValorLixo = -15;

    [Header("Configuração")]
    [SerializeField] private Pontuacao Pontuacao;

    [Header("Game Events")]
    [SerializeField] private GameEvent ComeLixo;
    [SerializeField] private GameEvent ComeCrustaceo;
    [SerializeField] private GameEvent ComePeixe;


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
            TipoComida.Aleatorio => _spriteRenderer.color,
            _ => _spriteRenderer.color
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
        if(Pontuacao.Pontos < Pontuacao.PontuacaoAlvo)
        {
            Pontuacao.Pontos = TipoComida switch
            {
                TipoComida.Crustaceo => Pontuacao.AddPontos(ValorCrustaceo),
                TipoComida.Peixe => Pontuacao.AddPontos(ValorPeixe),
                TipoComida.Inseto => Pontuacao.AddPontos(ValorInseto),
                TipoComida.Lixo => Pontuacao.AddPontos(ValorLixo),
                TipoComida.Aleatorio => Pontuacao.Pontos,
                _ => Pontuacao.Pontos
            };

            var comidaEvent = TipoComida switch
            {
                TipoComida.Crustaceo => ComeCrustaceo,
                TipoComida.Peixe => ComePeixe,
                TipoComida.Inseto => ComePeixe,
                TipoComida.Lixo => ComeLixo,
                TipoComida.Aleatorio => null,
                _ => null
            };

            if (comidaEvent != null)
                StartCoroutine(Broadcast(comidaEvent, 0.2f));
        }
    }

    private IEnumerator Broadcast(GameEvent gameEvent, float delay)
    {
        yield return new WaitForSeconds(delay);
        gameEvent.Broadcast();
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