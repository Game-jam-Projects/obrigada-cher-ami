using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Utilidade/VolumeInfo")]
public class VolumeInfo : ScriptableObject
{
    public VolumeType Tipo;
    [Range(0, 1)] public float Volume;

    public GameEvent EventoBroadcast;
    public GameEvent EventoAtualizacao;
}

public enum VolumeType
{
    Musica,
    Som
}
