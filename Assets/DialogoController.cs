using TMPro;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class DialogoController : MonoBehaviour
{
    [Header("Configuração")]
    public TextMeshProUGUI textoDialogo;
    public GameObject[] paineis; // Array de todos os painéis de diálogo
    public float velocidadeDigitacao = 0.05f;

    [Header("Eventos")]
    public UnityEvent aoIniciarDialogo;
    public UnityEvent aoCompletarDialogo;
    public UnityEvent aoFinalizarTodosDialogos;

    private int indicePainelAtual = 0;
    private int indiceTextoAtual = 0;
    private bool emDigitacao = false;
    private bool textoCompleto = false;
    private string textoAtual;
    private bool dialogoIncompleto = true;

    void Start()
    {
        IniciarSistemaDialogo();
    }

    void IniciarSistemaDialogo()
    {
        // Desativa todos os painéis exceto o primeiro
        foreach (var painel in paineis)
        {
            painel.SetActive(false);
        }

        paineis[0].SetActive(true);
        textoAtual = textoDialogo.text;
        textoDialogo.text = "";
        StartCoroutine(DigitacaoTexto());
        aoIniciarDialogo?.Invoke();
    }

    void Update()
    {
        if (dialogoIncompleto && Input.GetMouseButtonDown(0))
        {
            if (emDigitacao)
            {
                // Completar texto imediatamente
                StopAllCoroutines();
                textoDialogo.text = textoAtual;
                emDigitacao = false;
                textoCompleto = true;
                aoCompletarDialogo?.Invoke();
            }
            else if (textoCompleto)
            {
                AvancarParaProximoPainel();
            }
        }
    }

    IEnumerator DigitacaoTexto()
    {
        emDigitacao = true;
        textoCompleto = false;

        foreach (char letra in textoAtual.ToCharArray())
        {
            textoDialogo.text += letra;
            yield return new WaitForSeconds(velocidadeDigitacao);
        }

        emDigitacao = false;
        textoCompleto = true;
        aoCompletarDialogo?.Invoke();
    }

    void AvancarParaProximoPainel()
    {
        // Desativa painel atual
        paineis[indicePainelAtual].SetActive(false);
        indicePainelAtual++;

        if (indicePainelAtual < paineis.Length)
        {
            // Configura novo painel
            paineis[indicePainelAtual].SetActive(true);
            textoDialogo = paineis[indicePainelAtual].GetComponentInChildren<TextMeshProUGUI>();
            textoAtual = textoDialogo.text;
            textoDialogo.text = "";
            StartCoroutine(DigitacaoTexto());
            aoIniciarDialogo?.Invoke();
        }
        else
        {
            // Fim dos diálogos
            aoFinalizarTodosDialogos?.Invoke();
            dialogoIncompleto = false;
        }
    }
}