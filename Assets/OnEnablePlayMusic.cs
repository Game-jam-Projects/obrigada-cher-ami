using UnityEngine;

public class OnEnablePlayMusic : MonoBehaviour
{
    [SerializeField] private GameEvent _playMusic;

    private void Start()
    {
        _playMusic.Broadcast();
    }
}