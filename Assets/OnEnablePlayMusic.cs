using UnityEngine;

public class OnEnablePlayMusic : MonoBehaviour
{
    [SerializeField] private GameEvent _playMusic;

    private void Start()
    {
        if(_playMusic != null)
            _playMusic.Broadcast();
    }
}