using UnityEngine;

public class AjustaCorCorpo : MonoBehaviour
{
    public Pontuacao Pontuacao;
    private SpriteRenderer _sprite;

    private void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        _sprite.color = new Color(_sprite.color.r, _sprite.color.g, _sprite.color.b, Pontuacao.Pontos*0.008f);
    }
}
