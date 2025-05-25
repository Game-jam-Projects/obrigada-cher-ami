using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/GameEvent")]
public class GameEvent : ScriptableObject, IEvent
{
    List<IEventListener> Listeners;
    public List<AudioClip> Clips = new();
    public bool IsMusic;

    GameEvent SoundEvent;

    [HideInInspector] public string Name { get => name; }

    public AudioClip GetClip() => Clips.GetRandom();

    private void OnEnable()
    {
        Listeners = new List<IEventListener>();

        if (Clips.Count > 0)
            SoundEvent = IsMusic ? ResourcesHelper.GetGameEvent("_PlayMusica") : ResourcesHelper.GetGameEvent("_PlaySom");
    }

    public void Broadcast(object data)
        => BroadcastEvent(null, data);

    public void Broadcast()
        => BroadcastEvent(null, null);

    private void BroadcastEvent(Component sender, object data)
    {
        for (int i = Listeners.Count - 1; i >= 0; i--)
            Listeners[i].OnEventRaised(this, sender, data);

        if (Clips.Count > 0)
            SoundEvent.Broadcast(Clips.GetRandom());
    }

    public void Subscribe(IEventListener listener)
    {
        if (!Listeners.Contains(listener))
            Listeners.Add(listener);
    }

    public void Unsubscribe(IEventListener listener)
    {
        if (Listeners.Contains(listener))
            Listeners.Remove(listener);
    }
}
