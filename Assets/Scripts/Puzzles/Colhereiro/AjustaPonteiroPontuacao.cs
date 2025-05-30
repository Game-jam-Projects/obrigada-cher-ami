using UnityEngine;
using UnityEngine.UI;

public class AjustaPonteiroPontuacao : MonoBehaviour
{
    public Pontuacao Pontuacao;
    private Slider _slider;

    private void Start()
    {
        _slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        _slider.value = Pontuacao.Pontos / 100;
    }
}
