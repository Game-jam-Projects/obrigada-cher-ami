using UnityEngine;

public class AudioEventListener : MonoBehaviour, IEventListener
{
    [Header("Configurações")]
    public VolumeInfo VolumeInfo;

    AudioSource _audioSource;

    private void Awake() => LoadDependencies();

    private void OnEnable()
    {
        VolumeInfo.EventoBroadcast.Subscribe(this);
        VolumeInfo.EventoAtualizacao.Subscribe(this);
    }

    private void OnDisable()
    {
        VolumeInfo.EventoBroadcast.Unsubscribe(this);
        VolumeInfo.EventoAtualizacao.Unsubscribe(this);
    }

    public void OnEventRaised(IEvent gameEvent, Component sender, object data)
    {
        AtualizarVolume();

        if(data is AudioClip audioClip)
        {
            if (VolumeInfo.Tipo == VolumeType.Musica && _audioSource.isPlaying)
            {
                //_audioSource.Stop();
                //return;
            }

            _audioSource.PlayOneShot(audioClip);
        }
    }

    public void AtualizarVolume() => _audioSource.volume = VolumeInfo.Volume;

    private void LoadDependencies()
    {
        if(_audioSource == default)
            _audioSource = GetComponent<AudioSource>();
    }
}
